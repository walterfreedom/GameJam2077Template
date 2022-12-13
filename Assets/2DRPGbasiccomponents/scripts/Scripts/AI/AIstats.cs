using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIstats : MonoBehaviour
{

    [HideInInspector]
    public int health;
    [HideInInspector]
    public int armor;
    public double attackSpeed=1;
    public int damage=5;
    public bool isranged = false;
    public double attackCooldown=0;
    public float baseThreat=10;
    public List<GameObject> projectiles;
    private GameObject NPC;
    public GameObject gun;
    public GameObject owner;
    public GameObject whattodrop;
    bool usedefault = false;
    int goldvalue = 50;
    public bool hasenemyinrange = false;
    public GameObject targetedobject;
    public GameObject healthbar;
    Transform anchor;
    public bool BLACKEAGLESPECIAL=false;

    private void Start()
    {
        anchor = gameObject.transform.Find("anchor");
        owner = gameObject;
        NPC =gameObject;
        if (whattodrop == null)
        {
            whattodrop = GameObject.Find("coin");
            usedefault = true;
        }
        healthbar = transform.Find("Canvas").Find("healthbar").gameObject;
    }


    private void Update()
    {
        if (attackCooldown > 0)
        {
            attackCooldown-= Time.deltaTime;
        }
        else if (hasenemyinrange)
        {
            if (isranged)
            {
                
                AIshootat(targetedobject);
            }
            else
            {
                MeleeAttack();
            }
        }
        
        healthbar.transform.position = Camera.main.WorldToScreenPoint(anchor.position);
    }
    public void AIshootat(GameObject enemy)
    {

        //NOTE TO SELF
        //if bullets doesn't spawn, go check the transform of the prefabs. positions should be  0 0 0 for gun and all else. 
        //this is only for the prefab of the mob
        if (attackCooldown <= 0)
        {
            Vector3 source = this.transform.position;
            Vector3 shootdirection = (enemy.transform.position - gameObject.transform.position).normalized;
            float angle = Mathf.Atan2(shootdirection.x, shootdirection.y) * Mathf.Rad2Deg;
            gun.transform.eulerAngles = new Vector3(0, 180, angle);
            if (BLACKEAGLESPECIAL)
            {
                angle -= 20;
                for(int j =0; j < 3; j++)
                {
                    angle += 10;
                    gun.transform.eulerAngles = new Vector3(0, 180, angle);
                    ShootingScript shooting = new ShootingScript();
                    shooting.shoot(projectiles[0], enemy.transform, gun.transform, gameObject);
                }
            }
            else
            {
                gun.transform.eulerAngles = new Vector3(0, 180, angle);
                ShootingScript shooting = new ShootingScript();
                shooting.shoot(projectiles[0], enemy.transform, gun.transform, gameObject);
            }
         

            attackCooldown = 1 / attackSpeed;
        }        
    }



     public void MeleeAttack()
    {
        
        if (attackCooldown <= 0) {
            attackCooldown = 1 / attackSpeed;
            
            var a = Physics2D.OverlapCircleAll(gameObject.transform.position, 1);
            foreach (var hit in a)
            {
                if (NPC.GetComponent<AImovement>().Enemylist.Contains(hit.transform.tag))
                {
                    dealdamage(hit.transform.gameObject);
                    break;
                }
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

    private void dealdamage(GameObject item)
    {
        item.TryGetComponent<Stats>(out Stats health);
        health.DamageOrKill(damage, item, 3, NPC);
    }

    //public void DamageOrKill(int damage, GameObject ItemToDealDamage, float knockback, GameObject attacker)
    //{
    //    //if health wont drop to or below zero after attack
    //    if (health - damage > 0)
    //    {
    //        //decrease health by damage
    //        ItemToDealDamage.GetComponent<AIstats>().health -= damage;
    //        Vector2 knockbackDirection = new Vector2(ItemToDealDamage.transform.position.x-attacker.transform.position.x, ItemToDealDamage.transform.position.y - attacker.transform.position.y).normalized;
    //        //ItemToDealDamage.GetComponent<Rigidbody2D>().AddForce(knockback*knockbackDirection*100);
    //        ItemToDealDamage.TryGetComponent<Stats>(out Stats stats);

    //        StatusEffect a = new StatusEffect(3,1,"stun");
    //        ;


    //    }
    //    else
    //    {
    //        droploot(gameObject.transform.position);
    //        Destroy(ItemToDealDamage.gameObject);
    //    }
    //} 
    //hello :3
}
