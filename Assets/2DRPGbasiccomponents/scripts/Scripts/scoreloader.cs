using UnityEngine;
using TMPro;

public class scoreloader : MonoBehaviour
{
    GameObject manager;
    private void Start()
    {
        manager = GameObject.Find("manager");
        gameObject.GetComponent<TMP_Text>().text = "Your score was: "+manager.GetComponent<SaveScore>().score.ToString();
    }
}
