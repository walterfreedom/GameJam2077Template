using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public int damage = 50;
    public GameObject spawns = null;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Stats>(out Stats stats))
        {
            stats.DamageOrKill(damage,collision.gameObject,0,gameObject);
        }
        StartCoroutine(toDestroy());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(TryGetComponent<Stats>(out Stats stats))
        {
            if (stats.enemylist.Contains(collision.transform.tag))
            {
                if (collision.gameObject.TryGetComponent<Stats>(out Stats stats2))
                {
                    stats2.DamageOrKill(damage, collision.gameObject, 0, gameObject);
                }
                StartCoroutine(toDestroy());
            }
        }
        else if (TryGetComponent<car>(out car car))
        {
            if (car.enemylist.Contains(collision.gameObject.tag))
            {
                if (collision.gameObject.TryGetComponent<Stats>(out Stats stats2))
                {
                    stats2.DamageOrKill(damage, collision.gameObject, 0, gameObject);
                    StartCoroutine(toDestroy());
                }
            }
        }
    }


    IEnumerator toDestroy()
    {
        if (spawns != null)
        {
            GameObject spawned = Instantiate(spawns);
            spawned.transform.position = transform.position;

        }
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
