using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Transform Player { get; private set; }

    void Awake()
    {
        Instance = this;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
