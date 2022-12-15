using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class basicsign : MonoBehaviour , Iinteractable
{
    public string text;

    public void interactionTrigger(GameObject gameobject)
    {
        gameobject.transform.Find("Canvas").Find("Chat").Find("chattext").GetComponent<TMP_Text>().text = text;
        gameobject.transform.Find("Canvas").Find("Chat").gameObject.SetActive(true);
    }
}
