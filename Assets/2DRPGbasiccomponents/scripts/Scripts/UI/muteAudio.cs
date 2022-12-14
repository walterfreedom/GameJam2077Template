using UnityEngine;

public class muteAudio : MonoBehaviour
{
    GameObject music;
    private void Start()
    {
        music = GameObject.Find("music");
        
    }

    public void mutemusicbutton()
    {
        if (music.GetComponent<AudioSource>().enabled)
        {
            music.GetComponent<AudioSource>().enabled = false;
        }
        else
        {
            music.GetComponent<AudioSource>().enabled = true;
        }
    }
}
