using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStateId
{
    wander,
    attack,
    follow,
    dead
}

public interface EnemyState {
    EnemyStateId GetId();
    public void Update(EnemyAi enemy);
    public void Start(EnemyAi enemy);
    public void Exit(EnemyAi enemy);
}
