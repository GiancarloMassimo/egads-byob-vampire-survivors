using System;
using UnityEngine;

public interface IDespawnNotifier
{
    public event Action<GameObject> OnDestroyRequested;
}