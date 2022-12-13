using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeMatic : MonoBehaviour
{
    public GameObject robot;
    GameObject player;
    GameObject canvas;
    GameObject healthtext;
    GameObject dmgtext;
    GameObject astext;

    private void Start()
    {
        player = GameObject.Find("Player");
        canvas = transform.Find("Canvas").gameObject;
        
    }

    public void increaseHP()
    {
        if (player.GetComponent<playerStats>().money - 200 >= 0)
        {
            player.GetComponent<playerStats>().money -= 200;
            player.GetComponent<playerStats>().updateMoney();

            robot.GetComponent<Stats>().maxhealth+=10;
            robot.GetComponent<Stats>().health += 10;
            
            canvas.transform.Find("Health").GetComponent<TMP_Text>().text = "Can: "+robot.GetComponent<Stats>().maxhealth.ToString();
        }
    }
    public void increaseDamage()
    {
        if (player.GetComponent<playerStats>().money - 200 >= 0)
        {
            player.GetComponent<playerStats>().money -= 200;
            player.GetComponent<playerStats>().updateMoney();
            robot.GetComponent<Stats>().damage += 1;
            canvas.transform.Find("Damage").GetComponent<TMP_Text>().text = "Hasar: "+robot.GetComponent<Stats>().damage.ToString();

        }
    }
    public void increaseAS()
    {
        if (player.GetComponent<playerStats>().money - 200>= 0)
        {
            player.GetComponent<playerStats>().money -= 200;
            player.GetComponent<playerStats>().updateMoney();
            robot.GetComponent<Stats>().attackspeed += 0.1f;
            canvas.transform.Find("Attack Speed").GetComponent<TMP_Text>().text = "Saldiri Hizi: "+robot.GetComponent<Stats>().attackspeed.ToString();
        }
    }

    public void enableCanvas()
    {
        robot = GameObject.Find("Player").GetComponent<playerStats>().followerList[0];
        canvas.SetActive(!canvas.activeSelf);
        canvas.transform.Find("Attack Speed").GetComponent<TMP_Text>().text = "Saldiri Hizi: " + robot.GetComponent<Stats>().attackspeed.ToString();
        canvas.transform.Find("Damage").GetComponent<TMP_Text>().text = "Hasar: " + robot.GetComponent<Stats>().damage.ToString();
        canvas.transform.Find("Health").GetComponent<TMP_Text>().text = "Can: " + robot.GetComponent<Stats>().maxhealth.ToString();
    }
}
