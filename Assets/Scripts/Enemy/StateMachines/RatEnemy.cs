using System.Collections;
using UnityEngine;

public class RatEnemy : EnemyStateMachine
{
    private static WaitForSeconds _waitForSeconds3 = new WaitForSeconds(3);

    [SerializeReference, SubclassSelector] 
    EnemyState chaseState;

    [SerializeReference, SubclassSelector] 
    EnemyState SpeedState;

    [SerializeReference, SubclassSelector] 
    EnemyState Jumpscare;


    bool shouldJumpscare;
    bool shouldGoSpeedy;

    float timer = 0;
    

    protected override EnemyState BuildTransitions()
    {
        AddTransition(Jumpscare, () => shouldJumpscare, chaseState, SpeedState);
        AddTransition(SpeedState, () => shouldGoSpeedy, chaseState);
        AddTransition(chaseState, () => !shouldGoSpeedy, SpeedState);

        return chaseState;
    }

    protected override void Update()
    {
        base.Update();
        
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = 0.5f;
            if (Random.Range(0f, 1f) < 0.1f)
            {
                shouldJumpscare = true;
            } 
            else if (Random.Range(0f, 1f) < 0.2f)
            {
                if (!shouldGoSpeedy) StartCoroutine(GoFast());
            }
        } 

    }

    IEnumerator GoFast()
    {
        shouldGoSpeedy = true;
        yield return _waitForSeconds3;
        shouldGoSpeedy = false;
    }
}
