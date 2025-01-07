using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private Queue<GameObject> objectPool;

    public static ObjectPooler SharedInstance;


    private void Awake()
    {
        SharedInstance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        objectPool = new Queue<GameObject>();
        InitializePool();
    }

    private void InitializePool()
    {
        GameObject temp;

        foreach (Pool pool in pools)
        {

            for (int i = 0; i < pool.size; i++)
            {
                temp = Instantiate(pool.prefab);
                temp.SetActive(false);
                objectPool.Enqueue(temp);
            }
            poolDictionary[pool.tag] = objectPool;
        }
    }

    void Start()
    {
       
     
    }

    void Update()
    {
        
    }

    public GameObject GetPooledObject(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist");
            return null;
        }

        if (objectPool.Count == 0)
        {
            Debug.LogWarning($"Pool with tag {tag} is empty. Consider increasing the pool size.");
            return null; 
        }

        GameObject objToSpawn = poolDictionary[tag].Dequeue();
            objToSpawn.SetActive(false);
            objToSpawn.transform.position = position;
            objToSpawn.transform.rotation = rotation;

            return objToSpawn;
  
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist");
            return;
        }

        objectToReturn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturn);
    }
}
