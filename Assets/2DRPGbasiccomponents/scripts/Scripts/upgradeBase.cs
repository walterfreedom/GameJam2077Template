using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class upgradeBase : MonoBehaviour
{
    public List<GameObject> levels;
    public string upgradername;
    public int currentlevel = 0;
    GameObject slot;
    public GameObject spawnloc;
    public void checkupgrade()
    {
        slot = gameObject.transform.Find("Canvas").Find("upslot").gameObject;
        if (slot.GetComponent<inventorySlot>().storedItems.Count != 0)
        {
            if (slot.GetComponent<inventorySlot>().storedItems[0].GetComponent<pickle>().itemname == upgradername && levels.Count >= currentlevel + 1)
            {
                gameObject.transform.Find("Canvas").Find("upbutton").GetComponent<Image>().color = Color.green;
                gameObject.transform.Find("Canvas").Find("upbutton").GetComponent<Button>().interactable = true;
            }
            else
            {
                gameObject.transform.Find("Canvas").Find("upbutton").GetComponent<Image>().color = Color.red;
                gameObject.transform.Find("Canvas").Find("upbutton").GetComponent<Button>().interactable = false;
            }
        }
        
        else
        {
            gameObject.transform.Find("Canvas").Find("upbutton").GetComponent<Image>().color = Color.red;
            gameObject.transform.Find("Canvas").Find("upbutton").GetComponent<Button>().interactable = false;
        }
    }
    public void upgrade()
    {
        if (slot.GetComponent<inventorySlot>().storedItems[0].GetComponent<pickle>().itemname == upgradername && levels.Count >= currentlevel + 1)
        {
            Destroy(slot.GetComponent<inventorySlot>().storedItems[0]);
            slot.GetComponent<inventorySlot>().clearSlot();
            currentlevel += 1;
            Destroy(spawnloc.transform.Find("spawn").gameObject);
           
            var newlevel = Instantiate(levels[currentlevel - 1], spawnloc.transform);
            newlevel.name = "spawn";
            Invoke("checkupgrade",0.2f);

        }
    }

    public void canvasToggle (){
        GameObject canvas = transform.Find("Canvas").gameObject;
        canvas.SetActive(!canvas.activeSelf);

    }

    public void bandaidfix()
    {
        Invoke("checkupgrade",0.3f);
    }
}
