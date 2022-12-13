using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

public class CustomerAI : MonoBehaviour
{
    public List<item> itemwanted;
    public List<GameObject> possibleitems;
    public GameObject itemframe;
    public List<GameObject> itemframelist;
    AIDestinationSetter destinationSetter;
    bool waiting = false;
    float othercolors =255;
    int shopid=1;
    string newscene = "gameOver";
    Spawner spawner;
    
    void Start()
    {
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        for(int i = 0;i< Random.Range(1, 4); i++)
        {
            GameObject selecteditem = possibleitems[Random.Range(0, possibleitems.Count - 1)];
            item itemtoput = new item(selecteditem.GetComponent<pickle>().itemname, selecteditem.GetComponent<SpriteRenderer>().sprite, selecteditem.GetComponent<SpriteRenderer>().color);
            itemwanted.Add(itemtoput);
        }
        float x = 0.1f-0.1f*itemwanted.Count;
        foreach (var thing in itemwanted)
        {
            GameObject newframe = Instantiate(itemframe, new Vector2(transform.position.x+x, transform.position.y+0.5f), Quaternion.identity);
            newframe.transform.parent = gameObject.transform;
            newframe.GetComponent<SpriteRenderer>().sprite = thing.sprite;
            newframe.GetComponent<SpriteRenderer>().color = thing.color;
            x += 0.3f;
            itemframelist.Add(newframe);
        }
        var queue=GameObject.Find("shop"+shopid.ToString()).GetComponent<shopQueue>();
        destinationSetter = gameObject.GetComponent<AIDestinationSetter>();
        destinationSetter.target =queue.queue[queue.customers.IndexOf(gameObject)].transform;
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "queue")
        {
            waiting = true;
           // othercolors -= Time.deltaTime * (10+ 3*(spawner.day));
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, othercolors / 255, othercolors / 255, 255);
            if (othercolors <= 0)
            {
                SceneManager.LoadScene(newscene);
            }
        }

        if (collision.gameObject.TryGetComponent<pickle>(out pickle pickle))
        {
            if (pickle.itemname == "corncube"&&waiting)
            {
                serve();
                pickle.value -= 100;
                if (pickle.value == 0)
                {
                    Destroy(pickle.gameObject);
                }
            }
            else
            {
                int x = 0;
                foreach (var item in itemwanted)
                {

                    if (item.name == pickle.itemname)
                    {
                        Destroy(collision.gameObject);
                        Destroy(itemframelist[x]);
                        itemframelist.RemoveAt(x);
                        itemwanted.Remove(item);

                        if (itemwanted.Count <= 0)
                        {
                            serve();
                        }
                        break;
                    }
                    x++;
                }
            }
        }       
    }

    void serve()
    {
        //spawner.servedCustomerCheck();
        var exit = GameObject.Find("exit");
        destinationSetter.target = exit.transform;
        var pathfinder = GameObject.Find("shop1");
        int queueindex = pathfinder.GetComponent<shopQueue>().customers.IndexOf(gameObject);
        
        pathfinder.GetComponent<shopQueue>().customers.Remove(gameObject);
        var queue = pathfinder.GetComponent<shopQueue>();
        for (int i = 0; i < pathfinder.GetComponent<shopQueue>().customers.Count; i++)
        {
            queue.customers[i].GetComponent<AIDestinationSetter>().target = queue.queue[i].transform;
        }
        GameObject.Find("Player").GetComponent<playerStats>().increaseScore(1);
        waiting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.name=="exit")
        {
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public class item
{
    public string name { get; set; }
    public Sprite sprite { get; set; }
    public Color color { get; set; }
    public item(string namee, Sprite spritee,Color colorr)
    {
        name = namee;
        sprite = spritee;
        color = colorr;
    }
}


