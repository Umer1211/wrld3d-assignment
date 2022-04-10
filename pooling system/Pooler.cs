using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> dictionaryPool;
    void Start()
    {
        dictionaryPool = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            dictionaryPool.Add(pool.tag, objectPool);
        }
    }

    public void SpawnFromPool(string tag,Vector3 pos,Quaternion rot)
    {
        GameObject objTospawn = dictionaryPool[tag].Dequeue();

        objTospawn.SetActive(true);
        objTospawn.transform.position = pos;
        objTospawn.transform.rotation = rot;

        dictionaryPool[tag].Enqueue(objTospawn); //enquese the apawn obj for many time use
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}