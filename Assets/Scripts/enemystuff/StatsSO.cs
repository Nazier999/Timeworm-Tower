using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StatsSO")]
public class StatsSO : ScriptableObject
{
    [Header("Patrol state")]
    public float speed;
    public float raycastDistance;
    public float obstacleDistance;

    [Header("Player detection")]
    public float playerDetectDistance;
    public float detectionPauseTime;
    public float playerDetectedWaitTime;

    [Header("Charge State")]
    public float chargeTime;
    public float chargeSpeed;
    public float meleeDetectDistance;

    [Header("Melee Attack State")]
    public float damageAmount;
    public float knockbackForce;
    public Vector2 knockbackAngle;
}
