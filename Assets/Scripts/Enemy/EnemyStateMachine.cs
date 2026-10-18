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
    EnemyState initialState;
    EnemyState currentState;

    readonly HashSet<EnemyState> states = new();
    readonly Dictionary<EnemyState, List<Transition>> transitionMap = new();
    readonly List<Transition> anyStatePredicates = new();
 
    EnemyState pendingExit = null;
    bool firstInit = true; 

    void Register(EnemyState state)
    {
        Assert.IsNotNull(state); 
        states.Add(state);
    }

    protected virtual void Awake()
    {
        initialState = BuildTransitions();
        Register(currentState);
    }

    void Start()
    {
        foreach (var state in states) state.Build(gameObject);
        currentState = initialState;
        currentState.Setup();

        firstInit = false;
    }

    void OnEnable()
    {
        if (firstInit) return;

        currentState = initialState;
        currentState.Setup();
    }

    void OnDisable()
    {
        currentState?.Cleanup();
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
        if (pendingExit != null)
        {
            var to = pendingExit;
            pendingExit = null;
            return to;
        }

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
        Assert.IsTrue(from.Length > 0);
        Register(to);

        foreach (var state in from)
        {
            Assert.AreNotEqual(to, state, "self transition detected");
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
            Assert.AreNotEqual(to, state, "self transition detected");
            Register(state);

            state.Exit += () => 
            {
                Assert.AreEqual(currentState, state);
                pendingExit = to;
            };
        }
    }

    protected abstract EnemyState BuildTransitions();
}
