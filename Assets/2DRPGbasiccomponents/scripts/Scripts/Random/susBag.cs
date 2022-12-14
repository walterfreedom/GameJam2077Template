using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class susBag : MonoBehaviour
{
    public float time = 0;
    private void Start()
    {
        InvokeRepeating("flip", 1, 0.3f);
    }
    void update()
    {
        
        if (time > 10f)
        {
            flip();
            time = 0.0f;
        }
        else
        {
            time += Time.deltaTime * 1;
        }
        
    }

    void flip()
    {
        if (gameObject.GetComponent<SpriteRenderer>().flipX)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}
