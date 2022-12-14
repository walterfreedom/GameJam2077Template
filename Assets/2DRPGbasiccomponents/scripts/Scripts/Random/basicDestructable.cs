using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicDestructable : MonoBehaviour
{
    public int health = 100;
    public GameObject drops;

    public void damageordestroy(int damage)
    {
        if (health - damage > 0)
        {
          
                health -= damage;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        if (drops != null)
        {
           GameObject dropped = Instantiate(drops);
            dropped.transform.position = gameObject.transform.position;
        }
    }
}
