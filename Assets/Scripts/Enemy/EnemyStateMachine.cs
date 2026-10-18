using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

struct Transition
{
    public Func<bool> Predicate { get; private set; }
    public EnemyState To { get; private set; }

    public Transition(EnemyState to, Func<bool> predicate)
    {
        Predicate = predicate;
        To = to;
    }
}

public abstract class EnemyStateMachine : MonoBehaviour
{
    EnemyState currentState;

    readonly HashSet<EnemyState> states = new();
    readonly Dictionary<EnemyState, List<Transition>> transitionMap = new();
    readonly List<Transition> anyStatePredicates = new();
 
    void Register(EnemyState state)
    {
        Assert.IsNotNull(state); 
        states.Add(state);
    }

    protected virtual void Awake()
    {
        currentState = BuildTransitions();
        Register(currentState);
    }

    protected virtual void Start()
    {
        foreach (var state in states) state.Register(gameObject);
        currentState.Setup();
    }

    protected virtual void Update()
    {
        currentState.Execute();
        EnemyState nextState = GetNextState();
        
        if (nextState != null)
        {
            TransitionTo(nextState);
        }
    }

    protected virtual void FixedUpdate()
    {
        currentState.ExecutePhysics();
    }

    EnemyState GetNextState()
    {
        foreach (var transition in anyStatePredicates)
        {
            if (transition.To != currentState && transition.Predicate()) return transition.To;
        }

        if (transitionMap.TryGetValue(currentState, out var transitions)) 
        {
            foreach (var transition in transitions)
            {
                if (transition.Predicate()) return transition.To;
            }
        }

        return null;
    }

    void TransitionTo(EnemyState nextState)
    {
        currentState.Cleanup();
        nextState.Setup();
        currentState = nextState;
    }

    protected void AddTransition(EnemyState to, Func<bool> predicate, params EnemyState[] from)
    {
        Register(to);

        foreach (var state in from)
        {
            Assert.AreNotEqual(to, state, "circular transition detected");
            Register(state);

            if (!transitionMap.TryGetValue(state, out var transitions))
            {
                transitions = new();
                transitionMap[state] = transitions;
            }

            transitions.Add(new(to, predicate));
        }
    }
    
    protected void AddTransition(EnemyState to, Func<bool> predicate)
    {
        Register(to);
        anyStatePredicates.Add(new(to, predicate));
    }

    protected void AddTransitionOnExit(EnemyState to, params EnemyState[] from)
    {
        Register(to);

        foreach (var state in from)
        {
            Assert.AreNotEqual(to, state, "circular transition detected");
            Register(state);

            state.Exit += () => 
            {
                Assert.AreEqual(currentState, state);
                TransitionTo(to);
            };
        }
    }

    protected abstract EnemyState BuildTransitions();
}
