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

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] EnemyPoolItem[] pool;

    // TODO: change to animation curves, spawn in waves?
    [SerializeField] float spawnInterval;
    float spawnTimer = 0;

    readonly List<GameObject> prefabPool = new();
    readonly Dictionary<GameObject, Queue<GameObject>> inactiveEnemies = new();
    readonly Dictionary<GameObject, GameObject> prefabLookupTable = new();
    readonly HashSet<GameObject> activeEnemies = new();

    void Awake()
    {
        Assert.IsTrue(spawnInterval > 0);

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
        if (spawnTimer < 0)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }        
    }

    void SpawnEnemy()
    {
        var enemy = GetFromPool();
        enemy.transform.position = Vector2.zero;
        enemy.SetActive(true);
        activeEnemies.Add(enemy);    
    }

    GameObject GetFromPool()
    {
        GameObject prefab = prefabPool[Random.Range(0, prefabPool.Count)];

        if (inactiveEnemies.TryGetValue(prefab, out var queue) && queue.Count > 0)
            return queue.Dequeue();

        GameObject enemy = Instantiate(prefab);
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
        activeEnemies.Remove(enemy);
        enemy.SetActive(false);
        inactiveEnemies[prefabLookupTable[enemy]].Enqueue(enemy);
    }
}
