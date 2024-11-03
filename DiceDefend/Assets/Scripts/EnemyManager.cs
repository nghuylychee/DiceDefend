using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public event Action<float> OnWaveSpawn, OnWaveClear;
    public event Action<float> OnEnemyKilled;

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeField]
    private float enemyPerWave, currentEnemy, timeBetweenSpawns, currentWave, randX, randY;

    void Awake() 
    {
        Instance = this;
    }
    public void Init()
    {
        currentWave = 0;
    }
    
    [Button("Spawn Wave")]
    public void SpawnWave()
    {
        currentWave++;
        currentEnemy = enemyPerWave;
        StartCoroutine(SpawnEnemies());
    }
    private IEnumerator SpawnEnemies()
    {
        OnWaveSpawn?.Invoke(currentWave);
        for (int i = 0; i < enemyPerWave; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];
            var pos = spawnPoint.position + new Vector3(UnityEngine.Random.Range(-randX, randX), UnityEngine.Random.Range(-randY, randY));
            Instantiate(enemyPrefab, pos, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
    public void OnEnemyDie(float enemyReward)
    {
        currentEnemy--;
        OnEnemyKilled?.Invoke(enemyReward);
        if (currentEnemy <= 0)
        {
            OnWaveClear?.Invoke(currentWave);
        }
    }

}
