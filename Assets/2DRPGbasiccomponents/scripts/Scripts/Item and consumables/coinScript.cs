using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinScript : MonoBehaviour
{
    public int value;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<playerStats>().money += value;
            collision.gameObject.GetComponent<playerStats>().updateMoney();
            Destroy(gameObject);
        }

        else if(collision.TryGetComponent<coinScript>(out coinScript coinScript))
        {
            coinScript.value += value;
            Destroy(gameObject);
        }
    }
}
