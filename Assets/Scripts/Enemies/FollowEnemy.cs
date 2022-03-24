using UnityEngine;

public class FollowEnemy : EnemyBase
{
    private float attackCooldown;
    private new Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        attackCooldown = enemy.attackCooldown;
    }

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

            if (inCooldown) return;

            PlayerEvents.OnDamageReceived(enemy.damage);
            attackCooldown = enemy.attackCooldown;
            return;
        }

        var direction = target.transform.position - transform.position;
        direction.Normalize();
        rigidbody.velocity = direction * enemy.speed;

        CheckPlayerLookDirection();
    }

    private void CheckAttackCooldown()
    {
        if (inCooldown)
            attackCooldown -= Time.deltaTime;
    }

    private void CheckPlayerLookDirection()
    {
        if (isFacingRight)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            return;
        }

        transform.localScale = new Vector3(1, 1, 1);
    }

    private void StopEnemy() => rigidbody.velocity = Vector2.zero;
    private bool closerToPlayer => target && Vector2.Distance(transform.position, target.position) <= enemy.attackRange;
    private bool isFacingRight => rigidbody.velocity.x > 0;
    private bool inCooldown => attackCooldown > 0;
}