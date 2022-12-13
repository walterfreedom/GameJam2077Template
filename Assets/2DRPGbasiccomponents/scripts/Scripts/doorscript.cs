using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorscript : MonoBehaviour
{
   public bool open = false;
   public void togglestate()
    {
        if (open)
        {
            gameObject.tag = null;
        }
        else
        {
            gameObject.tag = "Obstacle";
        }
    }
}
