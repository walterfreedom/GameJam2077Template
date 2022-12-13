using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class AImovement : MonoBehaviour
{
    private GameObject NPCobject; //AI object 
    public int range = 5; //Range of the AI
    public int baseSpeed = 5;//Speed of AI, used for pathfinding. set to 0 when got hit for few seconds.
    [HideInInspector] 
    public float speed;
    public int followdistance = 1;
    public int combatDistance = 1;

    [HideInInspector]
    public AIPath AIPath;
    [HideInInspector]
    public AIBase AIbase;
    [HideInInspector]
    public Seeker seeker;
    [HideInInspector]
    public Path path;

    [HideInInspector]
    private List<StatusEffect> statuslist;
    public bool canSeeThroughWalls = false;
    [HideInInspector]
    public float threat;
    [HideInInspector]
    public List<string> Enemylist;
    public List<string> Allylist;
   
    public GameObject owner;
    [HideInInspector]
    public AIDestinationSetter aIDestination;
    [HideInInspector]
    public bool agressiveMode = false;
    [HideInInspector]
    public bool isFollowingPlayer = false;

    public bool wandering= false;
   
    SpriteRenderer renderer;
    AIqueue AIqueue;
    [HideInInspector]
    public IAstarAI astarAI;
    bool simpleanimation = true;

    private List<Vector2> AIdailyPlan;
    private float randomwander=5;
    private float lastmovetime;
    [HideInInspector]
    public bool followerCommand=false;

    [HideInInspector]
    public string currentbehaviour="default";
    Vector2 lastpos; //The position (x,y) where the Player was seen last time by the AI 
    Vector2 lastPosition;
    Animator animator;

    private void Start()
    {
        aIDestination = gameObject.GetComponent<AIDestinationSetter>();
        AIqueue = GameObject.Find("Astarpath").GetComponent<AIqueue>();
        NPCobject = gameObject;
        renderer = gameObject.GetComponent<SpriteRenderer>();
        astarAI = gameObject.GetComponent<IAstarAI>();
        AIbase = gameObject.GetComponent<AIBase>();
        seeker = gameObject.GetComponent<Seeker>();
        AIPath = gameObject.GetComponent<AIPath>();
        aIDestination.target = gameObject.transform;

        AIPath.orientation = OrientationMode.YAxisForward;
        AIPath.enableRotation = false;
        AIPath.maxSpeed = baseSpeed;

        //without this AI starts travelling towards (0,0,0) when he spawns, do not change this
        owner = NPCobject;
        if(TryGetComponent(out Animator anr)){
            animator = anr;
            simpleanimation = false;
        }
        aIDestination = gameObject.GetComponent<AIDestinationSetter>();
        lastpos = NPCobject.transform.position;
        lastPosition = NPCobject.transform.position;
        animator = GetComponent<Animator>();
        speed = baseSpeed;
        InvokeRepeating("AIcheckeveryXseconds", 0.1f, 0.1f);
        InvokeRepeating("checkifattack", 1, 0.1f);

    }

    //update is only executed on server (30 ticks per second)

    void Update()
    {
        checkOwner();
        #region statuseffect

        //Legacy code.moved to Stats.cs
        //if (statuslist.Count != 0)
        //{
        //    foreach (StatusEffect status in statuslist)
        //    {

        //        bool willLoop = status.applyEffect(status, this);
        //        if (!willLoop)
        //        {
        //            statuslist.Remove(status);
        //        }

        //    }
        //}
        #endregion
        #region animator
        if (!simpleanimation)
        {
            animator.SetFloat("Speed", Mathf.Abs(NPCobject.transform.position.y - lastPosition.y) + Mathf.Abs(NPCobject.transform.position.x - lastPosition.x));
            if (Mathf.Abs(NPCobject.transform.position.y - lastPosition.y) + Mathf.Abs(NPCobject.transform.position.x - lastPosition.x) > 0.01f)
            {
                animator.SetFloat("Horizontal", NPCobject.transform.position.x - lastPosition.x);
                animator.SetFloat("Vertical", NPCobject.transform.position.y - lastPosition.y);
            }
            lastPosition = NPCobject.transform.position;
        }
        
        else
        {
            var a = astarAI.destination.x - lastpos.x;
            if (a < 0f)
            {
                renderer.flipX = true;
                lastpos.x = gameObject.transform.position.x;
            }
            else
            {
                renderer.flipX = false;
                lastpos.x = gameObject.transform.position.x;
            }
        }
        #endregion

       
    }

    void AIcheckeveryXseconds()
    {
        AIqueue.addqueue(gameObject);
    }

    public void AImove()
    {
        if (wandering)
        {

            if (Time.time - lastmovetime >= randomwander)
            {
                astarAI.destination = new Vector2(gameObject.transform.position.x + Random.Range(-5, 5),
                    gameObject.transform.position.y + Random.Range(-5, 5));
                randomwander = Random.Range(10, 50);
                lastmovetime = Time.time;
            }

        }
        else if (followerCommand)
        {


            if (astarAI.reachedEndOfPath)
                followerCommand = false;
        }
        else if (isFollowingPlayer)
        {
            aIDestination.target = owner.transform;
        }
        else if (currentbehaviour == "recharge")
        {
            aIDestination.target = gameObject.transform;
        }
        else
        {

            //this is used for detecting gameobjects in a circle with r=range
            if (!isFollowingPlayer)
            {
                AIPath.endReachedDistance = combatDistance;
                List<string> sumlist = new List<string>();
                sumlist.AddRange(Enemylist);
                sumlist.AddRange(Allylist);
                var GameObjectsInSight = Physics2D.OverlapCircleAll(NPCobject.transform.position, range);
                findTarget(GameObjectsInSight, sumlist, canSeeThroughWalls, lastpos);
            }

            //else if (Vector2.Distance(NPCobject.transform.position, owner.transform.position) < followdistance / 2)
            //{
            //    AIPath.endReachedDistance = combatDistance / 2;
            //    isFollowingPlayer = false;
            //}

        }
    }
    

    //attack script that gets called every 0.1 seconds
    void checkifattack()
    {
        
        var AIstats = gameObject.GetComponent<AIstats>();
        if (aIDestination.target != null)
        {
            if (Enemylist.Contains(aIDestination.target.gameObject.tag))
            {
                if (AIstats.isranged && aIDestination.target != null)
                {
                    AIstats.AIshootat(aIDestination.target.gameObject);
                }
                else
                {
                    AIstats.MeleeAttack();
                }
            }
        }
        
        
    }

    //this function is used by NPC, on update. it returns the target gameobject so AIpath can use it.

    public void findTarget(Collider2D[] gameObjects, List<string> tagstocopy, bool canseethroughwalls, Vector2 lastPosition)
    {

        //GameObject targetObject = new GameObject();
        //targetObject.transform.position = lastPosition;
        List<string> tags = new List<string>();
        if (tagstocopy.Count != 0)
        {
            tags.AddRange(tagstocopy);
        }

        else
        {
            return;
        }
        
        bool a = true;
        while (a && gameObjects.Length != 0)
        {

            List<GameObject> DetectedObjects = new List<GameObject>();

            foreach (var item in gameObjects)
            {
                if (item.tag == tags[0])
                {
                    //adds the gameobject to list
                    DetectedObjects.Add(item.gameObject);
                }
            }
            //if list isn't empty
            if (DetectedObjects.Count != 0)
            {
                if (canseethroughwalls)
                {
                    //targetObject = getClosest(visibleObjectList);
                    lastpos = getClosest(DetectedObjects).transform.position;
                    aIDestination.target = getClosest(DetectedObjects).transform;
                }
                else
                {
                    //this foreach is used for finding the closest visible gameobject, and targeting it
                    List<GameObject> visibleObjectList = new List<GameObject>();

                    
                    foreach (GameObject detectedObject in DetectedObjects)
                    {
                        //you changed this to vector 2, if breaks turn back to v3
                        Vector2 source = NPCobject.transform.position;
                        Vector2 dobject = detectedObject.transform.position;
                        Vector2 target = dobject - source;
                        RaycastHit2D[] hits = Physics2D.RaycastAll(source, target, range);
                        foreach (var hit in hits)
                        {
                            //checks if the gameobject is part of targets list. If it is, they will be added to visibleObjectList
                            //and then it will find the closest one, and return it 
                            if (hit.transform.gameObject.tag == tags[0])
                            {
                                //tags.Contains(hit.transform.gameObject.tag)
                                visibleObjectList.Add(hit.transform.gameObject);
                                break;
                            }

                            else if (hit.transform.gameObject.tag == "solid")
                            {
                                break;
                            }
                        }
                    }
                    //finds the closest one
                    if (visibleObjectList.Count != 0)
                    {
                        //this exist so AI wont attack when it doesnt have any enemy in range
                        aIDestination.target = getClosest(DetectedObjects).transform;
                       
                        
                        lastpos = aIDestination.target.transform.position;
                        
                        a = false;
                    }
                    else
                    {
                        //This wasted my 4 hours, I WILL NEVER FORGET THIS. UNACCEPTABLE!!!!!!!!
                        tags.RemoveAt(0);
                    }

                }
            }
            else if (tags.Count > 1)
            {
                tags.RemoveAt(0);
            }
            else
            {
                a = false;
                aIDestination.target = null;
            }
        }
    }

  
    public void checkOwner()
    {
        if (owner != null)
        {
            if (Vector2.Distance(NPCobject.transform.position, gameObject.GetComponent<AIstats>().owner.transform.position) > followdistance && !agressiveMode)
            {
                isFollowingPlayer = true;
                aIDestination.target = gameObject.GetComponent<AIstats>().owner.transform;
            }
        }
       
    }
    public GameObject getClosest(List<GameObject> gameObjects)
    {

        //SHOULD RETURN VECTOR 2 FOR TRANSFORM, IF YOU CREATE GAMEOBJECTS IT WILL BE A MEMORY LEAK AND YOUR PC WILL BURN PLEASE DONT AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        //is enemy
        //&& gameObjects[0].tag!="Player"
        if (Enemylist.Contains(gameObjects[0].tag))
        {

            int indextoreturn = 0;
            foreach (GameObject visibleObject in gameObjects)
            {
                
                float threat1 = gameObjects[indextoreturn].GetComponent<Stats>().baseThreat;
                float threat2 = visibleObject.GetComponent<Stats>().baseThreat;
                if (threat1 / Vector2.Distance(NPCobject.transform.position, gameObjects[indextoreturn].transform.position) <
                        threat2 / Vector2.Distance(NPCobject.transform.position, visibleObject.transform.position))
                {

                    indextoreturn = gameObjects.IndexOf(visibleObject);
                }
            }
            var a = NPCobject.GetComponent<AIstats>();
            AIPath.endReachedDistance = combatDistance;
            return gameObjects[indextoreturn];
        }
        //is ally
        else
        {
            int indextoreturn = 0;
            foreach (GameObject visibleObject in gameObjects)
            {

                if (Vector2.Distance(NPCobject.transform.position, gameObjects[indextoreturn].transform.position) >
                      Vector2.Distance(NPCobject.transform.position, visibleObject.transform.position))
                {

                    indextoreturn = gameObjects.IndexOf(visibleObject);
                }
            }
            AIPath.endReachedDistance = followdistance;
            return gameObjects[indextoreturn];
        }

    }
}
    public class StatusEffect : AImovement
    {
        float dur;
        int str;
        string type;

        public StatusEffect(float duration, int strength, string Statustype)
        {
            dur = duration;
            str = strength;
            type = Statustype;
        }

        public bool applyEffect(StatusEffect statusEffect, AImovement a)
        {

            if (statusEffect.dur - Time.deltaTime > 0)
            {

                if (statusEffect.type == "stun")
                {
                    statusEffect.dur -= Time.deltaTime;
                    a.speed = 0;
                }

                return true;
            }
            else
            {
                statusEffect.dur = 0;
                a.speed = baseSpeed;

                return false;
            }



        }
        public void applyEffect(int strength, string Statustype, GameObject target)
        {

        }
    }


