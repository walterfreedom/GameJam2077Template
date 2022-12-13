using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hightemp : MonoBehaviour
{
    public float temperature=45.0f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Stats>(out Stats stats))
        {
            stats.ambtemp = temperature;
            Debug.LogWarning("Player is in the hot zone");
        }
        Debug.Log("trigger");
    }
}
