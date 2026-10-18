using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Transform Player { get; private set; }

    void Awake()
    {
        Assert.IsNull(Instance);
        Instance = this;

        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
