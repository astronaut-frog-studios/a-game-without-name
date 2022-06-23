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

        if (state is WalkEnemyState.WAITING) return;

        if (!canDetectPlayer || inCooldown || state is WalkEnemyState.SEARCHING_RANDOM_POS)
        {
            if (state is WalkEnemyState.WALKING_TO_PLAYER)
            {
                StartCoroutine(WaitToSearchRandomPos());
                return;
            }

            if (state is WalkEnemyState.WALKING)
            {
                if (pathPending) return;

                StartCoroutine(WaitToSearchRandomPos());
                return;
            }

            if (state != WalkEnemyState.SEARCHING_RANDOM_POS) return;

            WalkToRandomPos();
            return;
        }

        if (closerToPlayer && !inCooldown)
        {
            StopEnemy();

            // attack animation
            PlayerEvents.OnDamageReceived(enemy.damage);
            attackCooldown = enemy.attackCooldown;
            return;
        }

        state = WalkEnemyState.WALKING_TO_PLAYER;

        var targetDirection = target.transform.position - transform.position;
        var finalDirection = facingToPlayer ? targetDirection + directionOffset : targetDirection - directionOffset;

        rigidbody.velocity = finalDirection * enemy.speed / 2;
        CheckPlayerLookDirection();
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

    private Vector2 GetDirection(Vector2 targetPosition)
    {
        var direction = targetPosition - (Vector2)transform.position;
        direction.Normalize();

        return direction;
    }

    private void WalkToRandomPos()
    {
        randomPosition = new Vector2(LevelBounds.RandomPosX, LevelBounds.RandomPosY);
        rigidbody.velocity = GetDirection(randomPosition) * enemy.speed;
        state = WalkEnemyState.WALKING;
    }

    private IEnumerator WaitToSearchRandomPos()
    {
        state = WalkEnemyState.WAITING;
        StopEnemy();
        yield return new WaitForSeconds(.5f);
        state = WalkEnemyState.SEARCHING_RANDOM_POS;
    }

    private bool pathPending => Vector2.Distance(rigidbody.position, randomPosition) > stoppingDistance;
    private bool facingToPlayer => rigidbody.velocity.x > 0;
}
