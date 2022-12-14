using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaEffects : MonoBehaviour
{
    public string id = System.Guid.NewGuid().ToString();
    public bool slow = false;
    public bool speed = false;
    public bool damage = false;
    public bool heal = false;
    public AudioClip Audio;
    AudioSource asource;

    Dictionary<GameObject,List<Status>> toremovelist = new Dictionary<GameObject, List<Status>>();

    private void Awake()
    {
        if (Audio != null)
        {
            asource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            asource.clip = Audio;
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Audio != null)
        {
            if(collision.CompareTag("Player"))
            asource.Play();
        }
            

        if(collision.TryGetComponent<Stats>(out Stats stats))
        {

            if (collision.CompareTag("redteam"))
                print("aaaaaaa");
            if(slow)
                stats.changespeed(0.5f);
            if (speed)
                stats.changespeed(2f);
            if (damage)
                stats.hpregen -= 30;
            if (heal)
                stats.hpregen += 30;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Stats>(out Stats stats))
        {
            if (slow)
                stats.changespeed(2f);
            if (speed)
                stats.changespeed(0.5f);
            if (damage)
                stats.hpregen += 30;
            if (heal)
                stats.hpregen -= 30;

            if (Audio != null)
            {
                if (collision.CompareTag("Player"))
                    asource.Stop();
            }
        }
    }

  

}
