using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState[] states;
    public EnemyAi enemy;
    public EnemyStateId currentState;

    public EnemyStateMachine(EnemyAi enemy)
    {
        this.enemy = enemy;
        int numStates = System.Enum.GetNames(typeof(EnemyStateId)).Length;
        states = new EnemyState[numStates];
    }

    public void RegisterState(EnemyState state)
    {
        int index = (int)state.GetId();
        states[index] = state;
    }

    public EnemyState GetState(EnemyStateId stateId)
    {
        int index = (int)stateId;
        return states[index];
    }

    public void Update()
    {
        GetState(currentState)?.Update(enemy);
    }

    public void ChangeState(EnemyStateId newState)
    {
        GetState(currentState)?.Exit(enemy);
        currentState = newState;
        enemy.current = newState;
        GetState(currentState)?.Start(enemy);
    }
}

