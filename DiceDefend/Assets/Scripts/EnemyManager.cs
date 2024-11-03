using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;


    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeField]
    private float enemiesPerWave, timeBetweenSpawns, currentWave, randX, randY;

    void Awake() 
    {
        Instance = this;
    }
    public void Init()
    {
        // enemiesPerWave = 5; timeBetweenSpawns = 1f; currentWave = 0;
    }
    [Button("Spawn Wave")]
    public void SpawnWave()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];
        spawnPoint.position += new Vector3(Random.Range(-randX, randX), Random.Range(-randY, randY));

        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}
