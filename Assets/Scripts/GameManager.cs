using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Transform Player { get; private set; }

    readonly Dictionary<string, AttackSpawner> attacks = new();

    void Awake()
    {
        Instance = this;
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        var attackSpawners = FindObjectsByType<AttackSpawner>(FindObjectsSortMode.None);
        foreach (var attack in attackSpawners)
        {
            attacks[attack.key] = attack;
        }
    }

    public AttackSpawner GetAttack(string key)
    {
        return attacks[key];
    }
}
