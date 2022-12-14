using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{
    shopQueue shopQueue;
    public GameObject creature;
    bool canspawn=true;
 
    //   public int day;
    public int waveamount=1;
    GameObject daycounter;
    public float interval=5.0f;
    //public float waittime = 10.0f;
    bool isday;
   // public int customersToServe=0;
    private void Start()
    {
        daycounter = GameObject.Find("day");

        InvokeRepeating("spawncustomer", interval, interval);
    }
  
    //public void nextday()
    //{

    //    waveamount = (int)(4 + 2* day) ;
    //    customersToServe = waveamount;
    //    isday = true;
    //    interval -= 0.5f;
    //    InvokeRepeating("spawncustomer", interval,interval);

    //    daycounter.GetComponent<TMP_Text>().text = "Day: "+day.ToString();
    //    waittime += 1.1f;
    //    GameObject.Find("skipday").gameObject.active = false;
    //}
    void spawncustomer()
    {
        
        Instantiate(creature,transform.position,transform.rotation);
    }
    //public void servedCustomerCheck()
    //{
    //    customersToServe--;
    //    if (customersToServe == 0)
    //    {
    //        day++;
    //        isday = false;
    //        GameObject.Find("Player").transform.Find("Canvas").Find("skipday").gameObject.active = true;
    //    } 
    //}
}
