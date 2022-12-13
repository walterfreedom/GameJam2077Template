using UnityEngine;

public class destroyafteranimation : MonoBehaviour
{
    void Start()
    {

        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}