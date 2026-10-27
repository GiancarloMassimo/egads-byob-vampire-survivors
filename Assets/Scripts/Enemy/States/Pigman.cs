using System;
using UnityEngine;

[Serializable]
public class Pigman : EnemyState
{
    [SerializeField] float targetScale;
    [SerializeField] float blowUpSpeed;
    [SerializeField] LayerMask damageLayers;
    [SerializeField] AudioClip sound;

    Transform self;    

    float attackTimer;
    bool hasPlayedSound = false;

    protected override void Init(GameObject enemy)
    {
        self = enemy.transform;
    }

    public override void Setup()
    {
        hasPlayedSound = false;
    }

    public override void Cleanup()
    {
        self.transform.localScale = Vector3.one;
    }

    public override void Execute() {
        attackTimer -= Time.deltaTime;

        if (attackTimer < 0)
        {
            if (!hasPlayedSound) AudioManager.instance.PlaySound(sound);
            hasPlayedSound = true;
            self.transform.localScale = Vector3.MoveTowards(self.transform.localScale, new(targetScale, 1, 1), blowUpSpeed * Time.deltaTime);
            if (self.localScale.x > targetScale) RaiseExit();
        }
    }

    public override void ExecutePhysics() {}

}
