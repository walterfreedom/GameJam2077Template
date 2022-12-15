using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class chest : MonoBehaviour , Iinteractable
{

    public List<chestdata> storeditems=new List<chestdata>();
    public List<GameObject> storeditems2 = new List<GameObject>();
    playerStats pstats;
    GameObject chestUI;
    GameObject chestButton;
    public int chestItemCount;
    public string chestID = "a";
    private void Awake()
    {
        StartCoroutine(startroutine());
    }

    IEnumerator startroutine()
    {
        yield return new WaitForEndOfFrame();
        if (gameObject.TryGetComponent<Stats>(out Stats stats))
        {
            chestID = stats.id;
        }
        else if (chestID == "a")
        {
            chestID = System.Guid.NewGuid().ToString();
        }
        GameObject.Find("Astarpath").GetComponent<savesystem>().saveables.Add(chestID, gameObject);
        GameObject.Find("Astarpath").GetComponent<savesystem>().saveables2.Add(gameObject);


        pstats = GameObject.Find("Player").GetComponent<playerStats>();
        chestUI = pstats.chestUI;

        for (int i = 0; i < 27; i++)
        {
            storeditems.Add(new chestdata(new List<GameObject>()));
        }
        yield return new WaitForEndOfFrame();
    }
    

    public void openChest()
    {
        chestUI.active = true;
        print(storeditems.Count);
        print("Slot 1= "+storeditems[0].data.Count);
        print("Slot 3= "+storeditems[2].data.Count);
        //THATS VERY BUGGY FIX IT
        //IT MIGHT DUPLICATE ITEMS, AND IT ERASE IETMS YOU PUT IN IT


        //update: fixed it
        int x = 1;
        chestItemCount = 0;
        foreach (var item in storeditems)   
        {
            var slot=chestUI.transform.Find("Button (" +x+ ")");
            slot.GetComponent<inventorySlot>().ownerID = chestID;
            slot.GetComponent<inventorySlot>().slotindex = x - 1;
            slot.GetComponent<inventorySlot>().clearSlot();
            if (item.data.Count != 0)
            {
                print("AAA:"+slot.GetComponent<inventorySlot>().storedItems.Count);
                print(slot.GetComponent<inventorySlot>().slotindex);
                slot.GetComponent<inventorySlot>().additem(item.data);
                chestItemCount++;
            }

           
            x++;
            
        }
        
      
    }

    public void closeChest()
    {
        int x = 0;
        foreach (var item in storeditems)
        {
            var slot = chestUI.transform.Find("Button (" + (x + 1) + ")");
            if (slot.GetComponent<inventorySlot>().storedItems.Count != 0)
            {
                item.data = new List<GameObject>(slot.GetComponent<inventorySlot>().storedItems);

            }

            else
            {
                item.data = new List<GameObject>();
            }

            x++;

        }
        chestUI.active = false;
       
    }



    public void pickputItem()
    {
        //if (pstats.tempitems.Count != 0)
        //{
            
        //    if (storeditems[id].data.Count != 0)
        //    {
        //        print("a");
        //        List<GameObject> a = new List<GameObject>();
        //        a.AddRange(storeditems[id].data);
        //        storeditems[id].GetComponent<inventorySlot>().clearSlot();
        //        storeditems[id].GetComponent<inventorySlot>().additem(pstats.tempitems);
        //        pstats.templistClear();
        //        pstats.templistAdd(a);
        //    }
        //    else
        //    {
        //        print("b");
        //        storeditems[id].GetComponent<inventorySlot>().additem(pstats.tempitems);
        //        pstats.templistClear();
        //    }
        //}
        //else
        //{
            
        //    if (storeditems[id].GetComponent<inventorySlot>().storedItems.Count != 0)
        //    {
        //        print("c");
        //        pstats.templistAdd(storeditems[id].GetComponent<inventorySlot>().storedItems);
        //        storeditems[id].GetComponent<inventorySlot>().clearSlot();
        //    }
        //}

        ////print("walter");
        ////if (storeditems[id].GetComponent<inventorySlot>().storedItems.Count != 0)
        ////{
        ////    //newbutton.GetComponent<Image>().sprite = item.storedItems[0].GetComponent<SpriteRenderer>().sprite;
        ////    //newbutton.GetComponent<Image>().color = item.storedItems[0].GetComponent<SpriteRenderer>().color;
        ////    newbutton.GetComponent<Button>().onClick.AddListener(delegate { pickputItem(x); });
        ////}
        ////else
        ////{
        ////    newbutton.GetComponent<Button>().onClick.AddListener(delegate { pickputItem(x); });
        ////}

        ////    if (pstats.tempitems.Count != 0)
        ////    {
        ////        List<GameObject> tlist = new List<GameObject>();
        ////        if (storeditems[id].storedItems.Count != 0)
        ////        {
        ////            tlist.AddRange(storeditems[id].storedItems);
        ////        }
        ////        storeditems[id].storedItems = pstats.tempitems;
        ////        buttons[id].GetComponent<Image>().sprite = pstats.tempitems[0].GetComponent<SpriteRenderer>().sprite;
        ////        buttons[id].GetComponent<Image>().color = pstats.tempitems[0].GetComponent<SpriteRenderer>().color;
        ////        pstats.tempitems = tlist;
        ////    }
        ////    else
        ////    {
        ////        if (storeditems[id].storedItems.Count != 0)
        ////        {
        ////            pstats.putItemInTempItem(storeditems[id].storedItems);
        ////            storeditems[id].storedItems.Clear();
        ////            buttons[id].GetComponent<Image>().sprite = default;
        ////            buttons[id].GetComponent<Image>().color = default;
        ////        }
        ////    }

    }

    public void interactionTrigger(GameObject gameobject)
    {
        openChest();
        gameobject.GetComponent<playerStats>().lastchest = this;
    }
}

public class chestdata
{

    //why do I have this class?
    public List<GameObject> data;
    public chestdata(List<GameObject> data1)
    {
        data = data1;
    }
}

public class savechestID
{
    string cID;
    public savechestID(chest chest)
    {
        cID = chest.chestID;
    }
}
