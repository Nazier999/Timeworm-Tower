using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundEnemy : MonoBehaviour
{
    public float stateTime;

    public EnemyBaseState currentState;
    public PatrolState patrolState;
    public PlayerDetectedState playerDetectedState;
    public MeleeAttackState meleeAttackState;

    public Rigidbody2D rb;
    public Transform ledgeDetection;
    public LayerMask groundLayer, obstacleLayer, playerLayer, damageableLayer;
    public int facingDirection = 1;
    public GameObject alert;
    private bool playerDetected;
    public ChargeState chargeState;

    public StatsSO stats;


    private void Awake()
    {
        patrolState = new PatrolState(this, "patrol");
        playerDetectedState = new PlayerDetectedState(this, "playerDetected");
        chargeState = new ChargeState(this, "charge");
        meleeAttackState = new MeleeAttackState(this, "meleeAttack");

        currentState = patrolState;
        currentState.Enter();
    }



    private void Update()
    {
        currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }

    public bool CheckForObstacles()
    {
        RaycastHit2D hit = Physics2D.Raycast(ledgeDetection.position, Vector2.down, stats.raycastDistance, groundLayer);
        RaycastHit2D hitObstacle = Physics2D.Raycast(ledgeDetection.position, Vector2.right, stats.obstacleDistance, obstacleLayer);
        if (hit.collider == null || hitObstacle.collider == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckForPlayer()
    {
        RaycastHit2D hitPlayer = Physics2D.Raycast(ledgeDetection.position, facingDirection == 1 ? Vector2.right : Vector2.left, stats.playerDetectDistance, playerLayer);
        if (hitPlayer.collider == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckForMeleeTarget()
    {
        RaycastHit2D hitMeleeTarget = Physics2D.Raycast(ledgeDetection.position, facingDirection == 1 ? Vector2.right : Vector2.left, stats.meleeDetectDistance, playerLayer);
        if (hitMeleeTarget.collider == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SwitchState(EnemyBaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
        stateTime = Time.time;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(ledgeDetection.position, (facingDirection == 1 ? Vector2.right : Vector2.left) * 5);
    }

}
