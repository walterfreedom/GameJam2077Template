using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airQuality : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Stats>(out Stats stats))
        {
            stats.breathable = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Stats>(out Stats stats))
        {
            stats.breathable = true;
        }
    }
}
