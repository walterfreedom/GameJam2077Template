using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car : MonoBehaviour
{
    public Rigidbody2D rb;
    public int drip = 0;
    public AudioClip whatsappsoundeffect;
    float speed = 6;
    float turnspeed = 90;
    float speedmod = 1;
    public bool willmove = false;
    void Update()
    {
        if (willmove)
        {
           
               
            if (Input.GetKey(KeyCode.Space))
            {
                turnspeed = 180;
                speedmod = 0.5f;
            }
            else
            {
                turnspeed = 90;
                speedmod = 1;
            }
            Vector3 v3Force = speed * transform.right * Input.GetAxisRaw("Vertical") *speedmod; ;
            rb.velocity = v3Force;
            if (Input.GetKey(KeyCode.D))
            {
                transform.rotation *= Quaternion.Euler(Vector3.forward * turnspeed * Time.deltaTime*-1);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.rotation *= Quaternion.Euler(Vector3.forward * turnspeed * Time.deltaTime);
            }
        }
       
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Stats>(out Stats stats))
        {
            if (stats.health < 5000)
                drip++;
            stats.DamageOrKill(5000, collision.gameObject, 1, gameObject);
        }
        if(drip<=3)
        gameObject.GetComponent<AudioSource>().PlayOneShot(whatsappsoundeffect);
        else if(drip==4)
        {
            gameObject.GetComponent<AudioSource>().Play();
            speed = 16;
        }

    }


}
