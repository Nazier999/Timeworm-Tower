using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : EnemyBaseState
{
    public MeleeAttackState(groundEnemy enemy, string animationName) : base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entering " + animationName);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemy.ledgeDetection.position, enemy.stats.meleeDetectDistance, enemy.damageableLayer);

        foreach(Collider2D hitCollider in hitColliders)
        {
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();

            if(damageable != null)
            {
                hitCollider.GetComponent<Rigidbody2D>().velocity = new Vector2(enemy.stats.knockbackAngle.x * enemy.facingDirection, enemy.stats.knockbackAngle.y) * enemy.stats.knockbackForce;
                damageable.TakeDamage(enemy.stats.damageAmount);
            }
        }

        enemy.SwitchState(enemy.patrolState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
