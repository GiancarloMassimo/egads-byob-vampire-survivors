using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;

using Random = UnityEngine.Random;

[Serializable]
public struct AttackPoolItem
{
    public int weight;
    public GameObject prefab;
}

public class AttackSpawner : MonoBehaviour
{
    [SerializeField] AttackPoolItem[] pool;

    readonly List<GameObject> prefabPool = new();
    readonly Dictionary<GameObject, Queue<GameObject>> inactiveInstances = new();
    readonly Dictionary<GameObject, GameObject> prefabLookupTable = new();
    readonly HashSet<GameObject> activeInstances = new();

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

    public void SpawnAttackInstance(LayerMask damageLayers, Vector2 pos, Quaternion forward)
    {
        var attack = GetFromPool();
        attack.transform.SetPositionAndRotation(pos, forward);

        if (attack.TryGetComponent<IDamagesLayers>(out var layerDamage))
        {
            layerDamage.SetDamageLayers(damageLayers);
        }
        else
        {
            Assert.IsTrue(false, "Add damage layers!!!!!!!!");
        }

        attack.SetActive(true);
        activeInstances.Add(attack);
    }

    public void ClearCurrentAttack()
    {
        foreach (var attack in new List<GameObject>(activeInstances)) ReturnToPool(attack);
    }
    
    protected GameObject GetFromPool()
    {
        GameObject prefab = prefabPool[Random.Range(0, prefabPool.Count)];

        if (inactiveInstances.TryGetValue(prefab, out var queue) && queue.Count > 0)
            return queue.Dequeue();

        GameObject attack = Instantiate(prefab);
        attack.SetActive(false); 

        prefabLookupTable[attack] = prefab;
        if (!inactiveInstances.ContainsKey(prefab))
            inactiveInstances[prefab] = new();

        if (attack.TryGetComponent<IDespawnNotifier>(out var notifier))
        {
            notifier.OnDestroyRequested += ReturnToPool;
        }
        else
        {
            Assert.IsTrue(false, "Add a despawn notifier!!!!!!!!");
        }

        return attack;
    }

    void ReturnToPool(GameObject attack)
    {
        if (!activeInstances.Remove(attack)) return; 
        attack.SetActive(false);
        inactiveInstances[prefabLookupTable[attack]].Enqueue(attack);
    }
}
