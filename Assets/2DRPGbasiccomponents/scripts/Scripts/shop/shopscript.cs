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
        //    if (storedItems.Count > 0)
        //    {
        //        var itemcount = gameObject.transform.Find("Text (TMP)");
        //        int previousnum = int.Parse(itemcount.gameObject.GetComponent<TMP_Text>().text);
        //        if (previousnum > 1)
        //        {
        //            itemcount.gameObject.GetComponent<TMP_Text>().text = (previousnum - 1).ToString();
        //        }
        //        else
        //        {
        //            itemcount.gameObject.GetComponent<TMP_Text>().text = "";
        //        }
        //        storedItems[0].transform.position = gameObject.transform.parent.parent.position;
        //        storedItems[0].active = true;
        //        storedItems.RemoveAt(0);


        //        if (storedItems.Count < 1)
        //        {
        //            gameObject.GetComponent<Image>().sprite = default;
        //            gameObject.GetComponent<Image>().color = Color.white;
        //        }
        //    }
        var playerstats = transform.parent.parent.gameObject.GetComponent<playerStats>();
        if (playerstats.money >= storedItems[0].GetComponent<pickle>().value)
        {

            //TODO: show money on UI.
            transform.parent.parent.gameObject.GetComponent<playerStats>().money -= storedItems[0].GetComponent<pickle>().value;
            playerstats.updateMoney();
            playerstats.addtoinventory(Instantiate(storedItems[0]));

            
        }
        
    }
}
