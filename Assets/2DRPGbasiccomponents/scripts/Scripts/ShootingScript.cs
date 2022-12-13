using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    //I am actually a function :3

    public void shoot(GameObject bullet, Transform direction, Transform spawn,GameObject shooter)
    {    
        var bullet1=Instantiate(bullet, spawn.position, spawn.rotation);
        bullet1.GetComponent<Bullet>().setEnemylist(shooter.GetComponent<AImovement>().Enemylist);
        bullet1.GetComponent<Bullet>().setShooter(shooter);
        bullet1.GetComponent<Bullet>().damage=shooter.GetComponent<AIstats>().damage;
    }
}
