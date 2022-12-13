using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class weaponstats: MonoBehaviour
{
    public int damage = 10;
    public GameObject bullet;
    public bool isMelee = false;
    public int reach=1;
    GameObject objecttolarp;
    public bool shotgun;
  
    public void shoot(List<string> enemylist,GameObject shooter,Transform attackposition)
    {
        if (shotgun)
        {
            float x = -10;
            for (int i =0; i < 3; i++)
            {
                Quaternion newrot = attackposition.rotation * Quaternion.Euler(0, 0, -90+x); ;
                x += 10;
                var newbullet = Instantiate(bullet, attackposition.position, newrot);

                newbullet.GetComponent<Bullet>().Enemylist.AddRange(enemylist);
                newbullet.GetComponent<Bullet>().setShooter(gameObject);
                newbullet.GetComponent<Bullet>().damage = damage;
                newbullet.active = true;
            }
        }
        else
        {
            Quaternion newrot = attackposition.rotation * Quaternion.Euler(0, 0, -90); ;

            var newbullet = Instantiate(bullet, attackposition.position, newrot);

            newbullet.GetComponent<Bullet>().Enemylist.AddRange(enemylist);
            newbullet.GetComponent<Bullet>().setShooter(gameObject);
            newbullet.GetComponent<Bullet>().damage = damage;
            newbullet.active = true;
        }
    }
    public void swing(List<string> enemylist, GameObject shooter, Transform attackposition)
    {
        
        var player = GameObject.Find("Player");
        if (player.GetComponent<movement>().LARPMACHINE < 2)
        {
            if (Mathf.Abs(player.transform.Find("atk").rotation.eulerAngles.z) > 90&& Mathf.Abs(player.transform.Find("atk").rotation.eulerAngles.z) <270)
            {
                //print(player.transform.Find("atk").rotation.eulerAngles.z);
                player.GetComponent<movement>().LARPMACHINE = Mathf.Lerp(90, 0, Time.deltaTime);
            }

            else
            {
                //print(player.transform.Find("atk").rotation.eulerAngles.z);
                player.GetComponent<movement>().LARPMACHINE = Mathf.Lerp(-90, 0, Time.deltaTime);
            }

            var hit = Physics2D.OverlapCircleAll(attackposition.position, reach);
            foreach (var aga in hit)
            {
                if (enemylist.Contains(aga.tag))
                {
                    aga.gameObject.GetComponent<Stats>().DamageOrKill(damage, aga.gameObject, 5, shooter);
                }
                if (aga.TryGetComponent<Tilemap>(out Tilemap tilemap) && aga.tag == "Obstacle")
                {
                    print("hit");
                    int x = 0;
                    int y = 0;
                    if (player.transform.position.x > attackposition.position.x)
                        x -= reach;
                    if (player.transform.position.y > attackposition.position.y)
                        y -= reach;

                    for (int ax = x; ax <= x + reach; ax++)
                    {
                        for (int ay = y; ay <= reach; ay++)
                        {
                            tilemap.SetTile(new Vector3Int((int)attackposition.transform.position.x + ax, (int)attackposition.transform.position.y + ay, 0), null);

                        }
                    }
                    Bounds bounds = new Bounds(player.transform.position, new Vector3(reach * 2, reach * 2, 1));

                    AstarPath.active.UpdateGraphs(bounds);

                }
            }
        }
       
    }
}
