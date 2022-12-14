using UnityEngine;

public class SaveScore: MonoBehaviour
{
    public int score;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
}
