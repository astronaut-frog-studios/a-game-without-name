using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

internal enum ShootEnemyState
{
    WAITING,
    SEARCHING,
    RUNNING,
    WALKING,
    PREPARING,
    SHOOTING
};

public class ShootEnemy : EnemyBase
{
    [SerializeField] private ShootEnemyState state = ShootEnemyState.SEARCHING;
    [SerializeField] private float stoppingDistance = 0.25f;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Bullet bulletPrefab;

    private Bullet bullet;
    private float attackCooldown;
    private Vector2 randomPosition;
    private new Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        attackCooldown = enemy.attackCooldown;
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            if (!bullet) return;
            Destroy(bullet.gameObject);

            return;
        }

        CheckAttackCooldown();

        if (state is ShootEnemyState.WAITING) return;

        if (!canDetectPlayer && state != ShootEnemyState.SHOOTING)
        {
            if (state != ShootEnemyState.PREPARING)
            {
                CheckWalkingState();
                return;
            }

            state = ShootEnemyState.WAITING;
            StartCoroutine(WaitToSearchRandomPos());
            return;
        }

        if (closerToPlayer)
        {
            state = ShootEnemyState.RUNNING;
            rigidbody.velocity = GetDirection(target.transform.position) * -enemy.speed;
            return;
        }

        FacePlayer();

        if (canCheckBulletCollision)
        {
            CheckBulletCollision();
            return;
        }

        if (preparingAttack) return;
        state = ShootEnemyState.PREPARING;
        StopEnemy();

        if (bullet || inCooldown) return;
        state = ShootEnemyState.SHOOTING;
        bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }

    private void CheckAttackCooldown()
    {
        if (inCooldown)
            attackCooldown -= Time.deltaTime;
    }

    private void CheckWalkingState()
    {
        if (state is ShootEnemyState.WALKING)
        {
            if (pathPending) return;

            state = ShootEnemyState.WAITING;
            StartCoroutine(WaitToSearchRandomPos());
            return;
        }

        if (state != ShootEnemyState.SEARCHING) return;
        WalkToRandomPos();
    }

    private void WalkToRandomPos()
    {
        randomPosition = (Vector2) transform.position + Random.insideUnitCircle * (enemy.detectRange + 2);
        rigidbody.velocity = GetDirection(randomPosition) * enemy.speed;
        // Debug.DrawLine(transform.position, randomPosition, Color.green, 5f);
        state = ShootEnemyState.WALKING;
    }

    private void CheckBulletCollision()
    {
        var direction = GetDirection(target.position);
        bullet.rigid.AddForce(direction * (bulletSpeed * Time.fixedDeltaTime), ForceMode2D.Impulse);

        attackCooldown = enemy.attackCooldown;

        if (!bulletHasCollide)
        {
            if (bullet.transform.position == target.position)
                Destroy(bullet.gameObject);
            return;
        }

        Destroy(bullet.gameObject);
        PlayerEvents.OnDamageReceived(enemy.damage);
        state = ShootEnemyState.PREPARING;
    }

    private Vector2 GetDirection(Vector2 targetPosition)
    {
        var direction = targetPosition - (Vector2) transform.position;
        direction.Normalize();

        return direction;
    }

    private void FacePlayer()
    {
        var direction = target.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rigidbody.rotation = angle;
    }

    private IEnumerator WaitToSearchRandomPos()
    {
        StopEnemy();
        yield return new WaitForSeconds(2.5f);
        rigidbody.rotation = 0;
        state = ShootEnemyState.SEARCHING;
    }

    private void StopEnemy() => rigidbody.velocity = Vector2.zero;
    private bool pathPending => Vector2.Distance(rigidbody.position, randomPosition) > stoppingDistance;
    private bool closerToPlayer => target && Vector2.Distance(transform.position, target.position) <= enemy.attackRange;
    private bool inCooldown => attackCooldown > 0;
    private bool preparingAttack => state is ShootEnemyState.PREPARING && inCooldown;
    private bool canCheckBulletCollision => state is ShootEnemyState.SHOOTING && bullet;
    private bool bulletHasCollide => bullet &&
                                     Physics2D.OverlapCircle(bullet.transform.position, 0.4f,
                                         LayerMask.NameToLayer("PlayerTrigger"));
}