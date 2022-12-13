using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faketeleport : MonoBehaviour
{
    public GameObject hospitalTP;
    public GameObject FollowTP;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.position = hospitalTP.transform.position;
            collision.GetComponent<playerStats>().followerList[0].transform.position = FollowTP.transform.position;
        }
    }
}
