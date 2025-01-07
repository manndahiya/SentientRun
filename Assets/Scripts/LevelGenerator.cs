using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private int startingChunksAmount = 12;
    [SerializeField] private Transform chunkParent;
    [SerializeField] private float chunkLength = 100f;
    [SerializeField] private float moveSpeed = 8f;
    List<GameObject> chunks = new List<GameObject>();


    void Start()
    {
        GenerateStartingChunks();
    }

    private void GenerateStartingChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        float spawnPositionZ = CalculateSpawnPositionZ();

        Vector3 chunkSpawnPosition = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
       
        GameObject chunk = ObjectPooler.SharedInstance.GetPooledObject("Chunk", new Vector3(transform.position.x, transform.position.y, spawnPositionZ), Quaternion.identity);
        
        chunk.SetActive(true);
        chunks.Add(chunk);
        chunk.transform.SetParent(chunkParent);
    }

    private float CalculateSpawnPositionZ()
    {
        float spawnPositionZ;

        if (chunks.Count == 0)
        {
            spawnPositionZ = transform.position.z;
        }
        else
        {
            spawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLength;
        }

        return spawnPositionZ;
    }

    private void MoveChunks()
    {
       
        for (int i = 0; i < chunks.Count; i++)
        {
            GameObject chunk = chunks[i];
            chunks[i].transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
              
            if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
            {
               chunks.Remove(chunk);
               ObjectPooler.SharedInstance.ReturnToPool("Chunk", chunk);
               SpawnChunk();
                    
            }
                  
        }

    }

    void Update()
    {
        MoveChunks();   
    }
}
