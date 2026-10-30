using System;
using UnityEngine;

[Serializable]
public class Grow : EnemyState
{
    [SerializeField] float growSpeed;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite growSprite;

    Transform self;

    protected override void Init(GameObject enemy)
    {
        self = enemy.transform;
    }   

    public override void Setup()
    {
        if (growSprite != null) self.GetComponent<SpriteRenderer>().sprite = growSprite;
    }

    public override void Cleanup()
    {
        self.transform.localScale = Vector3.one;
        if (normalSprite != null) self.GetComponent<SpriteRenderer>().sprite = normalSprite;
    }
    
    public override void Execute()
    {
        self.transform.localScale += Vector3.one * Time.deltaTime * growSpeed; 
    }

    public override void ExecutePhysics()
    {
    }
}
