using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public string id="";
    public string name;
    [HideInInspector]
    public string faction;
    [HideInInspector]
    public string race;
    public float baseThreat=10;


    public float health;
   
    public int maxhealth;
    [HideInInspector]
    public int damage;
   
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float basespeed;
    [HideInInspector]
    public List<Status> statuslist = new List<Status>();
    public List<string> enemylist = new List<string>();
    public bool canAttack = true;
    public float attackspeed=1.0f;
    public float hpregen=10;


    [HideInInspector]
    public int oxygen = 120;
    [HideInInspector]
    public int maxox = 120;
    [HideInInspector]
    public bool breathable = true;
    [HideInInspector]               
    public float maxtemp = 32.0f;
    [HideInInspector]
    public float mintemp = 5.0f;
    [HideInInspector]
    public float currenttemp = 18.0f;
    [HideInInspector]
    public float ambtemp = 0.0f;
    [HideInInspector]
    public List<float> templist;

    [HideInInspector]
    public float energy = 120;
    [HideInInspector]
    public int maxenergy = 120;
    [HideInInspector]
    public bool charging = false;

    [HideInInspector]
    public bool needsair = true;
    [HideInInspector]
    public bool userenergy = false;
    [HideInInspector]
    public bool airprotected = false;
    [HideInInspector]
    public bool tempsensitive = true;

    [HideInInspector]
    public GameObject helmet;
    [HideInInspector]
    public GameObject body;
    [HideInInspector]
    public GameObject feet;

    public GameObject whattodrop;

    bool usedefault = false;
    int goldvalue = 50;

    [HideInInspector]
    public bool shieldmode=false;

    GameObject healthbar;
    public AudioClip playWhenDamaged;
    public AudioSource audio;
 
    
    private void Awake()
    {
        //if it breaks down put the id == "" && back
        if (gameObject.tag != "Player")
            id = System.Guid.NewGuid().ToString();
        else
        {
            id = "playerGaming";
        }

        GameObject.Find("Astarpath").GetComponent<savesystem>().saveables2.Add(gameObject);
        GameObject.Find("Astarpath").GetComponent<savesystem>().ids.Add(id);
        GameObject.Find("Astarpath").GetComponent<savesystem>().saveables.Add(id,gameObject);

        try {
            healthbar = transform.Find("Canvas").Find("healthbar").gameObject;
        }
        catch { }
       
        if(audio==null)
        {
            audio = gameObject.AddComponent<AudioSource>();
        }

        audio.spatialBlend = 1.0f;
        audio.maxDistance = 15f;
        audio.rolloffMode = AudioRolloffMode.Linear;
       
        if (whattodrop == null)
        {
            whattodrop = GameObject.Find("coin");
            usedefault = true;
        }

        if(gameObject.TryGetComponent<AImovement>(out AImovement aImovement))
        {
            aImovement.Enemylist.AddRange(enemylist);
        }

        templist.Add(0);
        if (needsair)
            statuslist.Add(new Status("breath", -1, 1));
        if (tempsensitive)
            statuslist.Add(new Status("temp", -1, 1));
        if (userenergy)
            statuslist.Add(new Status("energy", -1, 1));
        statuslist.Add(new Status("regen", -1, 1));
        InvokeRepeating("statuscheck", 0, 1.0f);
    }

    IEnumerator awakerout()
    {
        yield return new WaitForEndOfFrame();
        if (id == "" && gameObject.tag != "Player")
            id = System.Guid.NewGuid().ToString();
        else
        {
            id = "playerGaming";
        }

        GameObject.Find("Astarpath").GetComponent<savesystem>().saveables2.Add(gameObject);
        GameObject.Find("Astarpath").GetComponent<savesystem>().ids.Add(id);
        healthbar = transform.Find("Canvas").Find("healthbar").gameObject;
        if (whattodrop == null)
        {
            whattodrop = GameObject.Find("coin");
            usedefault = true;
        }

        templist.Add(0);
        if (needsair)
            statuslist.Add(new Status("breath", -1, 1));
        if (tempsensitive)
            statuslist.Add(new Status("temp", -1, 1));
        if (userenergy)
            statuslist.Add(new Status("energy", -1, 1));
        statuslist.Add(new Status("regen", -1, 1));
        InvokeRepeating("statuscheck", 0, 1.0f);
    }
    private void OnDestroy()
    {
        GameObject.Find("Astarpath").GetComponent<savesystem>().ids.Remove(id);
        GameObject.Find("Astarpath").GetComponent<savesystem>().saveables2.Remove(gameObject);
    }
    [HideInInspector]
    public List<Status> removelist = new List<Status>();
    void statuscheck()
    {
        StartCoroutine(statusnumerator());
    }

    IEnumerator statusnumerator()
    {
        foreach (Status status in statuslist)
        {
            if (!status.applyEffect(status, gameObject.GetComponent<Stats>()))
                removelist.Add(status);
        }

        yield return new WaitForEndOfFrame();
        foreach (Status status in removelist)
        {
            statuslist.Remove(status);
        }
    }

    public void changespeed(float amount)
    {
        if (gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<movement>().speed2 *= amount;
        }
        else
        {
            gameObject.GetComponent<AImovement>().AIPath.maxSpeed*= amount;
        }
    }

   public void attackset(){
        canAttack = !canAttack;
        Invoke("attackset", attackspeed);
    }

    public void equipHelmet(GameObject helmettoequip)
    {
        helmet = helmettoequip;
    }

    public void unEquipHelmet()

    {
        helmet = null;
    }

    public void DamageOrKill(int damage, GameObject ItemToDealDamage, float knockback, GameObject attacker)
    {
        //if health wont drop to or below zero after attack
        if (health - damage > 0)
        {
            if (!shieldmode)
            {
                //decrease health by damage
                health -= damage;
                Vector2 knockbackDirection = new Vector2(ItemToDealDamage.transform.position.x - attacker.transform.position.x, ItemToDealDamage.transform.position.y - attacker.transform.position.y).normalized;
                //ItemToDealDamage.GetComponent<Rigidbody2D>().AddForce(knockback*knockbackDirection*100);


                Status a = new Status("stun", 1, 1);
                statuslist.Add(a);
               if(healthbar!= null)
                {
                    var bar = healthbar.transform.Find("bar");
                    healthbar.gameObject.active = true;
                    bar.GetComponent<Slider>().value = ((float)health / (float)maxhealth) * 100.0f;
                }
                  
               if(audio!=null)
                if (!audio.isPlaying)
                {
                    audio.clip = playWhenDamaged;
                    audio.Play();
                }
                
            }
            else
            {
                energy -= damage;
                
            }

        }
        else
        {
            if (gameObject.CompareTag("Player"))
            {
                transform.position = new Vector3(0, 0, 0);
                health = maxhealth;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                
            }
            else
            {
                droploot(gameObject.transform.position);
                Destroy(ItemToDealDamage.gameObject);
            }
            
        }
    }
    
    void droploot(Vector2 position)
    {
        var drop = Instantiate(whattodrop, position, gameObject.transform.rotation);
        if (usedefault)
        {
            drop.GetComponent<coinScript>().value = goldvalue;
        }
    }

    public void heal(float str)
    {
        if (health + str < maxhealth)
        {
            health += str;
            if (healthbar != null)
            {
                var bar = healthbar.transform.Find("bar");
                bar.GetComponent<Slider>().value = ((float)health / (float)maxhealth) * 100.0f;
            }
          
        }
        else
        {
            health = maxhealth;
            if(gameObject.tag!="Player" && healthbar != null)
            healthbar.active = false;
        }
    }

    public SaveData createSaveData()
    {
        SaveData data = new SaveData(this);
        return data;
    }

    public void loadData(SaveData data)
    {
      
        health = data.health;
        transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        GameObject.Find("Astarpath").GetComponent<savesystem>().saveables.Remove(id);
        id = data.id;
        GameObject.Find("Astarpath").GetComponent<savesystem>().saveables.Add(id,gameObject);
    }
}

[System.Serializable]
public class SaveData
{
    
    public float health;
    public string id;
    public float[] position = new float[3];
    public bool useoxy;
    public bool useen;
    public bool usetemp;
    public List<string> enemylist = new List<string>();
    public string prefabname;
    public SaveData(Stats stats)
    {
        health = stats.health;
        id = stats.id;
        position[0] = stats.transform.position.x;
        position[1] = stats.transform.position.y;
        position[2] = stats.transform.position.z;
        useoxy = stats.needsair;
        useen = stats.userenergy;
        usetemp = stats.tempsensitive;
        enemylist.Clear();
        enemylist.AddRange(stats.enemylist);
        prefabname = stats.name;
    }
}
public class Status
{
    public string stname;
    float duration;
    int strength;
    string identity = "";
    public Status(string n, float dur, int str)
    {
        stname = n;
        strength = str;
        duration = dur;
    }

    public Status(string n, float dur, int str , string id)
    {
        stname = n;
        strength = str;
        duration = dur;
        identity = id;
    }



    public bool applyEffect(Status status, Stats stats)
    {
      
        if (status.duration - Time.deltaTime > 0)
        {

            if (status.stname == "stun")
            {
                status.duration -= Time.deltaTime;
                stats.speed = 0;
            }

            if (status.stname == "damage")
            {
                stats.DamageOrKill(status.strength, stats.gameObject, 0f, stats.gameObject);
            }

            if (status.stname == "heal")
            {
                stats.heal(status.strength);
            }

            duration -= 1;
            return true;
        }
        else if (duration == -1)
        {
            if (status.stname == "damage")
            {
                stats.DamageOrKill(status.strength, stats.gameObject, 0f, stats.gameObject);
            }

            if (status.stname == "heal")
            {
                stats.heal(status.strength);
            }

            if (status.stname == "breath")
            {
                
                if (stats.breathable)
                {
                    if (stats.oxygen + 10 < stats.maxox)
                    {
                        stats.oxygen += 10;
                    }
                    else
                    {
                        stats.oxygen = stats.maxox;
                    }
                }

                else if (stats.helmet != null)
                {

                    if (stats.helmet.TryGetComponent<pickle>(out pickle pickle))
                    {
                        
                        if (pickle.itemperks != null)
                        {
                            if (stats.oxygen + 1 < stats.maxox)
                            {
                                stats.oxygen += 1;

                            }
                            else
                            {
                                stats.oxygen = stats.maxox;

                            }
                            stats.helmet.GetComponent<pickle>().damageItem(1);
                        }

                    }


                    else
                    {
                        stats.oxygen--;
                    }

                }
                else
                {

                    stats.oxygen--;
                }
                if (stats.gameObject.tag == "Player")
                {
                    var pstats = stats.gameObject.GetComponent<playerStats>();
                    pstats.updateoxygen(((float)stats.oxygen/(float)stats.maxox)*100.0f);
                }
            

            }

            if (status.stname == "energy")
            {
                if (stats.charging)
                {

                    if (stats.energy + 10 < stats.maxenergy)
                    {
                        stats.energy += 10;
                    }
                    else
                    {
                        stats.energy = stats.maxenergy;
                    }
                }
                else
                {
                    stats.energy -= 1;
                }

                if (stats.gameObject.tag == "Player")
                {
                    var pstats = stats.gameObject.GetComponent<playerStats>();
                    pstats.updateoxygen(((float)stats.energy / (float)stats.maxenergy) * 100.0f);
                }
                else
                {
                    if (stats.energy == 0)
                    {
                        if(stats.gameObject.TryGetComponent<AImovement>(out AImovement aImovement))
                        {
                            aImovement.currentbehaviour = "regainenergy";
                        }
                    }
                }
            }
            if (status.stname== "temp")
            {
                var newtemp=0.0f;
                foreach(float tempsource in stats.templist)
                {
                    newtemp += (stats.ambtemp + tempsource) * stats.templist.Count;
                }
            }

            if (status.stname == "regen")
            {
                stats.heal(stats.hpregen);
            }

            return true;
        }
        else
        {
            if (status.stname == "stun")
            {
                stats.speed = stats.basespeed;
              
            }
          

            return false;
        }

    }
  
}

