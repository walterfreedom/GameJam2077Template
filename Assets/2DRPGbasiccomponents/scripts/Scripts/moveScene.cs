using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class moveScene : MonoBehaviour
{
    public string scene;
    public void movescene()
    {
        SceneManager.LoadScene(scene);

        var player = GameObject.Find("Player");
        var manager = GameObject.Find("Astarpath");
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(manager);
        DontDestroyOnLoad(GameObject.Find("WalterFreedom"));
        DontDestroyOnLoad(GameObject.Find("EventSystem"));
        foreach (var follower in player.GetComponent<playerStats>().followerList)
        {
            DontDestroyOnLoad(follower);
        }
    
        foreach (var go in GameObject.FindObjectsOfType<dontDestroy>())
        {
            DontDestroyOnLoad(go);
        }
    }
}
