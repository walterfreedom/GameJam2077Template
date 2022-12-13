using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shoopKeeper : MonoBehaviour
{
    public List<GameObject> inventory;
    public List<GameObject> inventoryPrefabs;
    Button closeshop;
    private void Start()    
    {

        for (int i = 0; i <= inventoryPrefabs.Count; i++)
        {
            GameObject prefabtoadd = inventoryPrefabs[Random.Range(0, inventoryPrefabs.Count)];
            inventory.Add(prefabtoadd);
            inventoryPrefabs.Remove(prefabtoadd);

        }
        
        //foreach (Transform child in a)
        //{
        //    //look at child of child
        //    if (child.gameObject.TryGetComponent<inventorySlot>(out inventorySlot inventoryslot))
        //    {
        //        inventory.Add(child.gameObject);
        //    }
        //}
    }

    public void showShop()
    {
        gameObject.transform.Find("Canvas").gameObject.active = true;
    }
    public void closeShop()
    {
        gameObject.transform.Find("Canvas").gameObject.active = false;
    }
}
