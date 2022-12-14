using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class worldgen : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap Water;
    public Tilemap Floor;
    public Tilemap Solid;
    public int width = 124;
    public int height = 124;


    [Header("Tiles")]
    public Tile grass;
    public Tile water;
    public Tile rock;
    public Tile sand;


    [Header("Tiles")]
    public float waterlevel = 0.3f;


    float offsetx;
    float offsety;
    public float scale = 2f;

    public List<GameObject> TerrainPrefabs;
    private Texture2D tex;

    private void Start()
    {
        offsetx = Random.Range(0f, 10000f);
        offsety = Random.Range(0f, 10000f);
        tex = new Texture2D(216, 216);

        createTerrain();


    }
    void createTerrain()
    {
        float[,] falloffmap = new float[width, height];
        for(int y =0; y < width; y++)
        {
            for (int x= 0; x < width; x++)
            {
                float xv = x / (float)width * 2 - 1;
                float yv = y / (float)width * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffmap[x, y] = Mathf.Pow(v,3f)/(Mathf.Pow(v,3f)+Mathf.Pow(2.2f-2.2f*v,3f));
            }
        }

        float[,] worldmap = new float[width, height];
        //create mainland
        for (int i = 0; i < width; i++)
        {

            for (int j = 0; j < height; j++)
            {
                //if (Water.GetTile(new Vector3Int(i, j, 20)) == null && Floor.GetTile(new Vector3Int(i, j, 3)) == null)
                //{
                    var a = Mathf.PerlinNoise((float)i / width * scale + offsetx, ((float)j / height * scale + offsety));
                     a -=falloffmap[j,i];
          

                worldmap[i,j] = a;
                    if (a >= 1.2f * waterlevel)
                    {
                        Floor.SetTile(new Vector3Int(i, j, 2), grass);
                        if (Random.Range(0f, 1f) < 0.1f)
                        {
                            //Instantiate(TerrainPrefabs[0], new Vector2(i, j), new Quaternion(0, 0, 0, 0));
                        }
                    }
                    else if (a >= waterlevel)
                    {
                        Floor.SetTile(new Vector3Int(i, j, 3), sand);
                    }
                    else if (a < waterlevel)
                    {
                        Water.SetTile(new Vector3Int(i, j, 3), water);
                        //if((Random.Range(0f, 1f) < 0.3f)){
                        //    for(int x = 0; x < 5; x++)
                        //    {
                        //        for (int y = 0; y < 3; y++)
                        //        {
                        //            Floor.SetTile(new Vector3Int(i + x, j + y, 3), sand);
                        //        }
                        //    }
                        //}
                    }
                    else
                    {
                        Solid.SetTile(new Vector3Int(i, j, 0), rock);
                    }
                //}
            }
        }

        //setspawn
        GameObject spawn = new GameObject();
        GameObject spawn2 = new GameObject();
        bool spawned = false;
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < width; x++)
            {
                try
                {
                    if (worldmap[y, x] <= waterlevel && worldmap[x, y + 1] >= waterlevel&& Solid.GetTile(new Vector3Int(x,y,0))==null && Solid.GetTile(new Vector3Int(x+1, y+1, 0))==null)
                    {
                        
                        print(worldmap[x, y + 1]);
                        spawn.transform.position = new Vector3(x, y + 1);
                        spawn2.transform.position = new Vector3(x, y );
                        spawned = true;
                        break;
                    }
                }
                catch { }       
            }
            if (spawned)
                break;
        }

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < width; x++)
            {
                try
                {
                    if (Mathf.PerlinNoise(x * scale / width + offsety,y * scale / height + offsetx) <= waterlevel)
                    {
                        Solid.SetTile(new Vector3Int(x, y, 0), rock);
                        
                    }
                }
                catch { }
            }
          
        }


        //player items
        var manager= GameObject.Find("Astarpath");
        manager.GetComponent<savesystem>().loadPlayerDataOnly();
        var player = GameObject.Find("Player");
        player.transform.position = spawn2.transform.position;
        int yax = 2;
        foreach(var follower in player.GetComponent<playerStats>().followerList)
        {
            follower.transform.position = new Vector3(player.transform.position.x,player.transform.position.y+yax,0);
            yax += 2;
        }
        GameObject.Find("WalterFreedom").transform.position= new Vector3(player.transform.position.x, player.transform.position.y + yax, 0);
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < width; x++)
            {
                try
                {
                    if (worldmap[x, y] >= waterlevel)
                    {
                        //check surroundings;
                        bool willspawn = true;
                        for(int nx =-10; nx <= 10; nx++)
                        {
                            for (int ny = -3; ny <= 3; ny++)
                            {
                                if (worldmap[x+nx, y+ny] < waterlevel|| Solid.GetTile(new Vector3Int(x, y, 0))!=null)
                                    willspawn = false;
                            }
                        }
                        
                        if (willspawn)
                        {
                            Instantiate(TerrainPrefabs[0],new Vector3(x,y,0),Quaternion.identity);
                            x += 20;
                            y += 20;
                        }
                            
                    }

                   
                }
                catch { }
            }
            
        }
        StartCoroutine(scangraph());
    }

    IEnumerator scangraph()
    {
        yield return new WaitForEndOfFrame();
        AstarPath.active.Scan();
    }

    void createBiomes()
    {
        int success = 0;
        while (success < 5)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
        }
    }
}