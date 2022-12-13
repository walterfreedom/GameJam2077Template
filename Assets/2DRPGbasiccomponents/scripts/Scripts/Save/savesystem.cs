using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class savesystem : MonoBehaviour
{
    public Dictionary<string, GameObject> saveables = new Dictionary<string, GameObject>();
    public List<GameObject> saveables2 = new List<GameObject>();
    public List<string> ids = new List<string>();
    private List<SaveData> savedata = new List<SaveData>();
    private List<picklesave> itemdata = new List<picklesave>();
    public List<GameObject> addprefabs;
    public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    string path;
    private void Start()
    {
        addprefabs.AddRange(Resources.LoadAll("Prefabs", typeof(GameObject)).Cast<GameObject>());
        foreach (var prefab in addprefabs)
        {

            prefabs.Add(prefab.name, prefab);
        }
    }
    [ContextMenu("Save")]
    public void SaveGameObjects()
    {

        string content = "";
        string content2 = "";
        savedata.Clear();
        itemdata.Clear();



        foreach (var item in saveables)
        {
            if (item.Value != null)
            {
                if (item.Value.TryGetComponent<Stats>(out Stats stat))
                {
                    savedata.Add(stat.createSaveData());
                }

                if (item.Value.TryGetComponent<pickle>(out pickle pickle))
                {
                    itemdata.Add(pickle.createSaveData());
                }
            }
        }
        content = content + JsonHelper.ToJson<SaveData>(savedata.ToArray());
        content2 = content2 + JsonHelper.ToJson<picklesave>(itemdata.ToArray());
        writeFile(content, Application.persistentDataPath + "/save.json");
        writeFile(content2, Application.persistentDataPath + "/saveitems.json");
    }


    [ContextMenu("Load")]
    public void loadGameObjects()
    {
        IEnumerator loadroutine = LoadGameData(true);
        StartCoroutine(loadroutine);


        //string objects = readFile(Application.persistentDataPath + "/save.json");

        //List<SaveData> savedatas = JsonHelper.FromJson<SaveData>(objects).ToList<SaveData>();

        //string saveitems= readFile(Application.persistentDataPath + "/saveitems.json");

        //List<picklesave> itemdatas = JsonHelper.FromJson<picklesave>(saveitems).ToList<picklesave>();


        //var player = GameObject.Find("Player");
        //player.GetComponent<playerStats>().clearInventory();

        //foreach (var saveable in saveables2)
        //{
        //    if (saveable.tag != "Player")
        //    {
        //        Destroy(saveable);
        //    }  
        //}
        //saveables2.Clear();
        //ids.Clear();
        //player.GetComponent<playerStats>().templistClear();
        //saveables2.Add(player);

        //foreach (var data in savedatas)
        //{
        //    if (data.prefabname != "Player")
        //    {
        //        Vector3 spawnloc = new Vector3(data.position[0], data.position[1], data.position[2]);
        //        var a = Instantiate(prefabs[data.prefabname], spawnloc, Quaternion.identity);
        //        a.GetComponent<Stats>().loadData(data);

        //    }
        //    else
        //    {
        //        player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        //    }
        //}

        //foreach (var item in saveables2)
        //{
        //    if (item.TryGetComponent<pickle>(out pickle pickle))
        //    {
        //        saveables.Add(pickle.id, item);
        //    }
        //}

        //foreach (var data in itemdatas)
        //{

        //    Vector3 spawnloc = new Vector3(data.position[0], data.position[1], data.position[2]);
        //    var a = Instantiate(prefabs[data.itemname], spawnloc, Quaternion.identity);
        //    a.name = data.itemname;
        //    a.GetComponent<pickle>().loadData(data);

        //}

        //player.GetComponent<playerStats>().followerList.Clear();
        //player.GetComponent<playerStats>().followerList.Add(GameObject.Find("Robot(Clone)"));
    }

    private IEnumerator LoadGameData(bool willrepeat)
    {
        string objects = readFile(Application.persistentDataPath + "/save.json");

        List<SaveData> savedatas = JsonHelper.FromJson<SaveData>(objects).ToList<SaveData>();

        string saveitems = readFile(Application.persistentDataPath + "/saveitems.json");

        List<picklesave> itemdatas = JsonHelper.FromJson<picklesave>(saveitems).ToList<picklesave>();


        var player = GameObject.Find("Player");
        player.GetComponent<playerStats>().clearInventory();

        foreach (var saveable in saveables2)
        {
            if (saveable.tag != "Player")
            {
                Destroy(saveable);
            }
        }
        saveables2.Clear();
        ids.Clear();
        player.GetComponent<playerStats>().templistClear();

        saveables.Clear();
        saveables2.Add(player);
        saveables.Add(player.GetComponent<Stats>().id, player);
        yield return new WaitForEndOfFrame();
        foreach (var data in savedatas)
        {
            if (data.prefabname != "Player")
            {
                Vector3 spawnloc = new Vector3(data.position[0], data.position[1], data.position[2]);
                var a = Instantiate(prefabs[data.prefabname], spawnloc, Quaternion.identity);
                a.GetComponent<Stats>().loadData(data);

            }
            else
            {
                player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            }
        }

        //foreach (var item in saveables2)
        //{
        //    if (item.TryGetComponent<Stats>(out Stats stats))
        //    {
        //        saveables.Add(stats.id, item);
        //    }
        //}
        yield return new WaitForEndOfFrame();
        foreach (var data in itemdatas)
        {

            Vector3 spawnloc = new Vector3(data.position[0], data.position[1], data.position[2]);


            try
            {
                var a = Instantiate(prefabs[data.itemname], spawnloc, Quaternion.identity);
                a.name = data.itemname;
                a.GetComponent<pickle>().loadData(data);
            }
            catch
            {
                print(data.itemname);
            }
        }

        player.GetComponent<playerStats>().followerList.Clear();
        player.GetComponent<playerStats>().followerList.Add(GameObject.Find("Robot(Clone)"));
        yield return new WaitForEndOfFrame();
        if (willrepeat)
        {
            StartCoroutine(LoadGameData(false));
        }
    }

    private void writeFile(string content, string path)
    {
        FileStream file = new FileStream(path, FileMode.Create);
        if (File.Exists(path))
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.Write(content);
            }
        }

    }

    private string readFile(string path)
    {
        FileStream file = new FileStream(path, FileMode.Open);

        using (StreamReader reader = new StreamReader(file))
        {
            string savefile = reader.ReadToEnd();
            return savefile;
        }
    }

    public void loadPlayerDataOnly()
    {
        StartCoroutine(pdataonly(true));
    }

    private IEnumerator pdataonly(bool willrepeat)
    {
        string objects = readFile(Application.persistentDataPath + "/save.json");

        List<SaveData> savedatas = JsonHelper.FromJson<SaveData>(objects).ToList<SaveData>();

        string saveitems = readFile(Application.persistentDataPath + "/saveitems.json");

        List<picklesave> itemdatas = JsonHelper.FromJson<picklesave>(saveitems).ToList<picklesave>();


        var player = GameObject.Find("Player");
        player.GetComponent<playerStats>().clearInventory();

        foreach (var saveable in saveables2)
        {
            if (saveable.tag != "Player"&&!player.GetComponent<playerStats>().followerList.Contains(saveable))
            {
                Destroy(saveable);
            }
        }
        saveables2.Clear();
        ids.Clear();
        player.GetComponent<playerStats>().templistClear();

        saveables.Clear();
        saveables2.Add(player);
        saveables.Add(player.GetComponent<Stats>().id, player);
        yield return new WaitForEndOfFrame();

        foreach (var data in itemdatas)
        {

            Vector3 spawnloc = new Vector3(data.position[0], data.position[1], data.position[2]);

            if (data.ownerID == player.GetComponent<Stats>().id)
            {
                try
                {

                    var a = Instantiate(prefabs[data.itemname], spawnloc, Quaternion.identity);
                    a.name = data.itemname;
                    a.GetComponent<pickle>().loadData(data);
                }
                catch
                {
                    print(data.itemname);
                }
            }
        }

    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

