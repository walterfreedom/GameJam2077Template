using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class craftButton : MonoBehaviour
{
    public blueprint bp;
    public void craftitem(GameObject player)
    {

        
        playerStats pstats = player.GetComponent<playerStats>();
        List<GameObject> tempinventory=null;
        List<GameObject> itemstodestroylist = new List<GameObject>();
        int x = 0;
        foreach (var material in bp.materials)
        {
            
            foreach (var item in pstats.inventory)
            {
                bool matchfound = false;
                foreach (var resource in item.GetComponent<inventorySlot>().storedItems)
                {
                        Debug.Log("comparing " + material.GetComponent<pickle>().itemname + " to " + resource.GetComponent<pickle>().itemname);
                        if (material.GetComponent<pickle>().itemname == resource.GetComponent<pickle>().itemname&& !itemstodestroylist.Contains(resource))
                        {
                            itemstodestroylist.Add(item);
                            Debug.Log("found match");
                            x++;
                            matchfound = true;
                            break;
                        }
                }
                if (matchfound)
                {
                    break;
                }
                    
            }
            
        }

        

        if (itemstodestroylist.Count == bp.materials.Count)
        {
            Debug.Log("Item can be crafted!");
            foreach (var item in itemstodestroylist)
            {
                var itd = item.GetComponent<inventorySlot>().storedItems[0];
                item.GetComponent<inventorySlot>().dropItem();
                Destroy(itd);
            }
            var a = Instantiate(bp.output[0]);
            player.GetComponent<playerStats>().addtoinventory(a);
        }
        
    }
}
