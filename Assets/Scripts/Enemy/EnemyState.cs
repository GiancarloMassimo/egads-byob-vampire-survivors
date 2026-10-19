using System;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public abstract class EnemyState
{
    public event Action Exit;
    protected void RaiseExit() => Exit?.Invoke();

    bool isInitialized = false;
    public void Build(GameObject enemy)
    {
        Assert.IsFalse(isInitialized, "double initialization");
        isInitialized = true;
        Init(enemy);
    }

    protected abstract void Init(GameObject enemy);
    
    public abstract void Setup();
    public abstract void Cleanup();
    public abstract void Execute();
    public abstract void ExecutePhysics();
}
