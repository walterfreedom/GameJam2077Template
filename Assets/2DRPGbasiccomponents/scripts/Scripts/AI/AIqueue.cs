using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIqueue : MonoBehaviour
{
    public List<GameObject> queue;
    public List<GameObject> players;


    private void Update()
    {
        runAI();
    }

    public void addqueue(GameObject AI)
    {
        if (!queue.Contains(AI))
        {
            queue.Add(AI);
        }    
    }

    void runAI()
    {
        if (queue.Count != 0)
        {
            for (int i = 0; i < 10; i++)
            {
                if (queue.Count != 0)
                {
                    if (queue[0] != null)
                    {
                        queue[0].GetComponent<AImovement>().AImove();
                        queue.RemoveAt(0);
                    }
                    else
                    {
                        queue.RemoveAt(0);
                    }
                    
                }
            }
        }
       
    }
}
