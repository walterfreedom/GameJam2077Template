using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walterfreedom : MonoBehaviour
{

    public List<quest> quests = new List<quest>();
    public void DesertedIsland()
    {
        if (GameObject.Find("Player").GetComponent<playerStats>().money >= 500)
        {


            GameObject.Find("Player").GetComponent<playerStats>().money -= 500;
            gameObject.GetComponent<moveScene>().movescene();
            transform.Find("Canvas").Find("Image").Find("base").gameObject.SetActive(false);
            transform.Find("Canvas").Find("Image").Find("quest").gameObject.SetActive(true);
            quests.Add(new quest("kill", 1, 0, "skeleton"));
            print("QUEST COUNT = "+quests.Count.ToString());
        }
        
    }

    public void returnHome()
    {
        if (quests.Count==0)
        {
            GameObject.Find("Player").GetComponent<playerStats>().money += 1000;
            transform.Find("Canvas").Find("Image").Find("base").gameObject.SetActive(true);
            transform.Find("Canvas").Find("Image").Find("quest").gameObject.SetActive(false);
            gameObject.GetComponent<moveScene>().scene="2Drpg";
            gameObject.GetComponent<moveScene>().movescene();
        }
       
    }
}


public class quest
{

    //WIP 
    public string name;
    int data1;
    int data2;
    string data3;

    public quest(string n,int d1,int d2, string d3)
    {
        name = n;
        data1 = d1;
        data2 = d2;
        data3 = d3;
    }

    public bool checkQuest(string variable)
    {
        if (name == "kill")
        {
            Debug.Log("SKELETOR");
            if (variable == data3)
                data1--;
            if (data1 == 0)
            {
               
                Debug.Log("WORKEDDDDDDDDDDDDDD");
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }
}


