using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float obstacleSpawnTime = 1f;

    private int obstaclesSpawned = 0;
   
    void Start()
    {
        StartCoroutine(SpawnObstacleRoutine());
    }


    IEnumerator SpawnObstacleRoutine()
    {
        while (obstaclesSpawned < 5)
        {
            yield return new WaitForSeconds(obstacleSpawnTime);
            Instantiate(obstaclePrefab, transform.position, Quaternion.identity);
            obstaclesSpawned++;
        }
    }
   
    void Update()
    {
        
    }
}
