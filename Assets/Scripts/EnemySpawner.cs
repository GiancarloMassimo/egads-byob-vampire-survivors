using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;

using Random = UnityEngine.Random;

[Serializable]
public struct EnemyPoolItem
{
    public int weight;
    public GameObject prefab;
}

[Serializable]
public struct SpawnConfiner
{
    public float minDistFromPlayer;
    public Vector2 min;
    public Vector2 max;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] EnemyPoolItem[] pool;

    int numSpawns = 0;

    [SerializeField] int maxEnemies;
    [SerializeField] AnimationCurve enemiesPerWave;
    [SerializeField] AnimationCurve spawnInterval;

    [SerializeField] SpawnConfiner spawnConfiner;

    float spawnTimer = 0;

    readonly List<GameObject> prefabPool = new();
    readonly Dictionary<GameObject, Queue<GameObject>> inactiveEnemies = new();
    readonly Dictionary<GameObject, GameObject> prefabLookupTable = new();
    readonly HashSet<GameObject> activeEnemies = new();

    void Awake()
    {
        foreach (var item in pool)
        {
            for (int i = 0; i < item.weight; i++)
            {
                prefabPool.Add(item.prefab);
            }
        }
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0 && activeEnemies.Count < maxEnemies)
        {
            int enemiesInWave = (int)enemiesPerWave.Evaluate(numSpawns++);
            spawnTimer = spawnInterval.Evaluate(numSpawns);

            for (int i = 0; i < enemiesInWave; i++) SpawnEnemy();
        }        
    }

    void SpawnEnemy()
    {
        var enemy = GetFromPool();
        enemy.transform.position = ChooseSpawnPosition();
        enemy.SetActive(true);
        activeEnemies.Add(enemy);    
    }

    Vector2 ChooseSpawnPosition()
    {
        const int maxTries = 10;
        int i = 0;

        Vector2 spawnPos = default;

        do {
            if (++i == maxTries) break;

            spawnPos = new(
                Random.Range(spawnConfiner.min.x, spawnConfiner.max.x),
                Random.Range(spawnConfiner.min.y, spawnConfiner.max.y)
            );
        } while (Vector2.Distance(GameManager.Instance.Player.position, spawnPos) < spawnConfiner.minDistFromPlayer);

        return spawnPos;
    }

    GameObject GetFromPool()
    {
        GameObject prefab = prefabPool[Random.Range(0, prefabPool.Count)];

        if (inactiveEnemies.TryGetValue(prefab, out var queue) && queue.Count > 0)
            return queue.Dequeue();

        GameObject enemy = Instantiate(prefab);
        enemy.SetActive(false);

        prefabLookupTable[enemy] = prefab;
        if (!inactiveEnemies.ContainsKey(prefab))
            inactiveEnemies[prefab] = new();

        if (enemy.TryGetComponent<IDespawnNotifier>(out var notifier))
        {
            notifier.OnDestroyRequested += ReturnToPool;
        }
        else
        {
            Assert.IsTrue(false, "Add a despawn notifier!!!!!!!!");
        }

        return enemy;
    }

    void ReturnToPool(GameObject enemy)
    {
        if (!activeEnemies.Remove(enemy)) return; 
        enemy.SetActive(false);
        inactiveEnemies[prefabLookupTable[enemy]].Enqueue(enemy);
    }
}
