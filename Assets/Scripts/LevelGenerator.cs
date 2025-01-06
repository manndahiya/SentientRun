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
        GenerateChunks();
    }

    private void GenerateChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++)
        {
            float spawnPositionZ = CalculateSpawnPositionZ(i);

            Vector3 chunkSpawnPosition = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
            GameObject chunk = Instantiate(chunkPrefab, chunkSpawnPosition, Quaternion.identity, chunkParent);
            chunks.Add(chunk);
        }
    }

    private float CalculateSpawnPositionZ(int i)
    {
        float spawnPositionZ;

        if (i == 0)
        {
            spawnPositionZ = transform.position.z;
        }
        else
        {
            spawnPositionZ = transform.position.z + (i * chunkLength);
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
                    Destroy(chunk);
            }
            
           

           
        }

    }

    void Update()
    {
        MoveChunks();   
    }
}
