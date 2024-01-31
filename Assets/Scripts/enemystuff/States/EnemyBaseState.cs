using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState
{
    protected groundEnemy enemy;
    protected string animationName;

    public EnemyBaseState(groundEnemy enemy, string animationName)
    {
        this.enemy = enemy;
        this.animationName = animationName;
    }

    public virtual void Enter()
    {
       Debug.Log("Entering " + animationName + " state");
    }

    public virtual void Exit()
    {
        Debug.Log("Exiting " + animationName + " state");
    }
    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicsUpdate()
    {
        
    }

}
