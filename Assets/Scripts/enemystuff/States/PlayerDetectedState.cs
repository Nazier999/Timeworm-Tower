using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : EnemyBaseState
{
    public PlayerDetectedState(groundEnemy enemy, string animationName) : base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();

       enemy.rb.velocity = Vector2.zero;
        enemy.alert.SetActive(true);
    }

    public override void Exit()
    {
        enemy.alert.SetActive(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(!enemy.CheckForPlayer())
        {
            enemy.SwitchState(enemy.patrolState);
        }
        else
        {
            if(Time.time >= enemy.stateTime + enemy.stats.playerDetectedWaitTime)
            {
                enemy.SwitchState(enemy.chargeState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
