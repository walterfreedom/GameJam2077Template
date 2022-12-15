using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teamChanger : MonoBehaviour
{
    public List<string> newfriendlytags;
    public List<string> newenemytags;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Stats>(out Stats stats))
        {
            stats.enemylist.RemoveAll(x => newfriendlytags.Contains(x));
            stats.enemylist = newenemytags;
           if(collision.TryGetComponent<AImovement>(out AImovement aImovement))
            {
                aImovement.Allylist.Clear();
                aImovement.Allylist = newfriendlytags;
                stats.gameObject.tag = newfriendlytags[0];
            }
         
        }
    }
}
