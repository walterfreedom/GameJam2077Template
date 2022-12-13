using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class shopscript : MonoBehaviour
{
    public List<GameObject> storedItems;

    private void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(dropItem);
    }

    private void dropItem()
    {
        
        var playerstats = transform.parent.parent.parent.gameObject.GetComponent<playerStats>();
        if (storedItems[0].TryGetComponent<Stats>(out Stats stats))
        {
            if (playerstats.money >= 100)
            {

                playerstats.money -= 100;
                playerstats.updateMoney();
                GameObject newdude = Instantiate(storedItems[0]);
                newdude.transform.position = playerstats.transform.position;
                playerstats.followerList.Add(newdude);

            }
        }
        else
        {
            if (playerstats.money >= storedItems[0].GetComponent<pickle>().value)
            {
                playerstats.money -= storedItems[0].GetComponent<pickle>().value;
                playerstats.updateMoney();
                playerstats.addtoinventory(Instantiate(storedItems[0]));
            }

        }

    }
}
