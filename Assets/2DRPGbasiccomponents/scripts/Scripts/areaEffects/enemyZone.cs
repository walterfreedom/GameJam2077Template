using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class enemyZone : MonoBehaviour
{
    public bool waituntilwave = false;
    public List<GameObject> spawnedcreatures;
    public GameObject spawn;
    public float interval=5;
    public int maxspawn;
    public GameObject destination;
    private IEnumerator coroutine;
    
    void Start()
    {
        if (destination == null)
        {
            destination = gameObject;
        }
        coroutine = spawnenemy(interval);
        StartCoroutine(coroutine);
    }
    IEnumerator spawnenemy(float intv)
    {
        while (true)
        {
            spawnedcreatures.RemoveAll(creature=>creature==null);
            if (spawnedcreatures.Count + 1 < maxspawn)
            {
                var newspawn =Instantiate(spawn, transform.position+new Vector3(Random.Range(-5,5), Random.Range(-5, 5),0), transform.rotation);
                newspawn.GetComponent<AIDestinationSetter>().target = destination.transform;
                spawnedcreatures.Add(newspawn);
                yield return new WaitForSeconds(interval/10);
                
            }
            else
            {
                yield return new WaitForSeconds(interval);
            }
        }
    }
}
