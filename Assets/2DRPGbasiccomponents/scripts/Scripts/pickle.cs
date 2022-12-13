using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickle : MonoBehaviour
{
    public string id="";
    public int stacksize = 4;
    public string itemname;
    public int value = 100;
    public bool infiniteitemspawn;
    public string category;
    public int durability = 100;
    public List<string> itemperks;
    public int slotindex;
    public string ownerID;

    private void Awake()
    {
        
            id = System.Guid.NewGuid().ToString();
        GameObject.Find("Astarpath").GetComponent<savesystem>().saveables2.Add(gameObject);
        GameObject.Find("Astarpath").GetComponent<savesystem>().saveables.Add(id,gameObject);
        GameObject.Find("Astarpath").GetComponent<savesystem>().ids.Add(id);
    }
    public void damageItem(int damage)
    {
        durability -= damage;
        if (durability <= 0)
        {
            Destroy(gameObject);
        }
    }

    public picklesave createSaveData()
    {
        return new picklesave(this);
    }
    private void OnDestroy()
    {
        GameObject.Find("Astarpath").GetComponent<savesystem>().saveables2.Remove(gameObject);
    }

    public void loadData(picklesave data)
    {
        StartCoroutine(loadDT(data));
    }

    IEnumerator loadDT(picklesave data)
    {
        yield return new WaitForEndOfFrame();

        if (data.ownerID != "")
        {
            ownerID = data.ownerID;
            gameObject.active = false;
            slotindex = data.slotindex;
            var owner = GameObject.Find("Astarpath").GetComponent<savesystem>().saveables[ownerID];

            if (owner.tag == "Player")
            {
                //owner ID's are buggy, inventory slots wont have them. 
                List<GameObject> item = new List<GameObject>();
                item.Add(gameObject);
                owner.GetComponent<playerStats>().inventory[data.slotindex].GetComponent<inventorySlot>().additem(item);

            }
            else
            {
                owner.GetComponent<chest>().storeditems[slotindex].data.Add(gameObject);
            }
        }

        else
        {
            gameObject.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);

        }
    }
}
[System.Serializable]
public class picklesave
{
    public string itemname;
    public int slotindex;
    public string ownerID;
    public float[] position = new float[3];
    public picklesave(pickle pickle)
    {
        itemname = pickle.itemname;
        slotindex = pickle.slotindex;
        ownerID = pickle.ownerID;
        position[0] = pickle.gameObject.transform.position.x;
        position[1] = pickle.gameObject.transform.position.y;
        position[2] = pickle.gameObject.transform.position.z;
    }
}
