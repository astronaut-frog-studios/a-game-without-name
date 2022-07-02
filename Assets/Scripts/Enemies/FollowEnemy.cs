using UnityEngine;

public class FollowEnemy : EnemyBase
{
    private void FixedUpdate()
    {
        CheckAttackCooldown();

        if (!canDetectPlayer)
        {
            StopEnemy();
            return;
        }

        if (closerToPlayer)
        {
            StopEnemy();

            if (inCooldown || !hasCollision) return;

            PlayerEvents.OnDamageReceived(enemy.damage, true);
            attackCooldown = enemy.attackCooldown;
            return;
        }

        WalkToPlayer();
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

    private void WalkToPlayer()
    {
        var direction = target.transform.position - transform.position;
        direction.Normalize();
        rigidbody.velocity = direction * enemy.speed;

        CheckPlayerLookDirection();
    }

    private bool facingToPlayer => rigidbody.velocity.x > 0;
}