using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public List<EnemySpawner> spawners = new List<EnemySpawner>();
    private List<Enemy> enemies = new List<Enemy>();

    [Header("Spawn")]
    public Enemy[] enemyPrefabs;
    public Transform player;
    public float spawnInterval = 3f;
    public int maxEnemyCount = 20;

    private float spawnTimer;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        TickEnemies();
        UpdateSpawn();
    }

    void TickEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Tick();
        }
    }

    void UpdateSpawn()
    {
        if (spawners.Count == 0) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            

            if (enemies.Count < maxEnemyCount)
            {
                Debug.Log("Enemy Count: " + enemies.Count);
                SpawnRandomEnemy();
            }
        }
    }

    public void RegisterSpawner(EnemySpawner spawner)
    {
        if (!spawners.Contains(spawner))
            spawners.Add(spawner);
    }

    public void UnregisterSpawner(EnemySpawner spawner)
    {
        spawners.Remove(spawner);
    }

    public void RegisterEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
        enemy.OnDeath += OnEnemyDeath;
    }

    void OnEnemyDeath(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public void SpawnRandomEnemy()
    {
        if (spawners.Count == 0) return;

        EnemySpawner spawner =
            spawners[Random.Range(0, spawners.Count)];

        Enemy prefab =
            enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Enemy enemy = spawner.Spawn(prefab, player);
    }
}