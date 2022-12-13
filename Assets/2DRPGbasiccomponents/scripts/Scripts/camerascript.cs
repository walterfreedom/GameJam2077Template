using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerascript : MonoBehaviour
{
    public GameObject target;
    void Update()
    {
        if(target!=null)
            transform.position = new Vector3(target.transform.position.x,target.transform.position.y,transform.position.z);
        
    }
}
