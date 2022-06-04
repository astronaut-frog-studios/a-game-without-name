using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum WalkEnemyState
{
    WAITING,
    WALKING,
    SEARCHING_RANDOM_POS,
    WALKING_TO_PLAYER,
}

public class WalkEnemy : EnemyBase
{
    [Header("Walk Enemy")]
    [SerializeField] private WalkEnemyState state = WalkEnemyState.SEARCHING_RANDOM_POS;
    [Space(12.0f)]
    [SerializeField] private float stoppingDistance = 0.12f;
    [SerializeField] private Vector3 directionOffset;
    private Vector2 randomPosition;

    private void FixedUpdate()
    {
        CheckAttackCooldown();

        if (!canDetectPlayer)
        {
            // search randomPositions closer to it
            return;
        }

        if (closerToPlayer)
        {
            StopEnemy();
            // stopp walking, and attack player if isn't in cooldown. SearchRandomPos
            return;
        }

        // far from player and canDetectPlayer
        state = WalkEnemyState.WALKING_TO_PLAYER;

        var targetDirection = target.transform.position - transform.position; // add offset
        targetDirection.Normalize();

        var finalDirection = facingToPlayer ? targetDirection - directionOffset : targetDirection + directionOffset;
        rigidbody.velocity = finalDirection * enemy.speed;

        // search random position closer to player and walks to it.
    }

    private void CheckPlayerLookDirection()
    {
        if (facingToPlayer)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            return;
        }

        transform.localScale = new Vector3(1, 1, 1);
    }

    private bool pathPending => Vector2.Distance(rigidbody.position, randomPosition) > stoppingDistance;
    private bool facingToPlayer => rigidbody.velocity.x > 0;
}
