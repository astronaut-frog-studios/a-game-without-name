using System.Collections;
using UnityEngine;

internal enum ShootEnemyState
{
    WAITING,
    WALKING,
    RUNNING,
    SHOOTING,
    PREPARING_ATTACK,
    SEARCHING_RANDOM_POS
};

public class ShootEnemy : EnemyBase
{
    [Header("Shoot Enemy")]
    [SerializeField] private ShootEnemyState state = ShootEnemyState.SEARCHING_RANDOM_POS;
    [Space(12.0f)]
    [SerializeField] private float stoppingDistance = 0.25f;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Bullet bulletPrefab;

    private Bullet bullet;
    private Vector2 randomPosition;

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
            if (state != ShootEnemyState.PREPARING_ATTACK)
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
        state = ShootEnemyState.PREPARING_ATTACK;
        StopEnemy();

        if (bullet || inCooldown) return;
        state = ShootEnemyState.SHOOTING;
        bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
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

        if (state != ShootEnemyState.SEARCHING_RANDOM_POS) return;
        WalkToRandomPos();
    }

    private void WalkToRandomPos()
    {
        randomPosition = new Vector2(LevelBounds.RandomPosX, LevelBounds.RandomPosY);

        rigidbody.velocity = GetDirection(randomPosition) * enemy.speed;
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
        state = ShootEnemyState.PREPARING_ATTACK;
    }

    private Vector2 GetDirection(Vector2 targetPosition)
    {
        var direction = targetPosition - (Vector2)transform.position;
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
        state = ShootEnemyState.SEARCHING_RANDOM_POS;
    }

    private bool pathPending => Vector2.Distance(rigidbody.position, randomPosition) > stoppingDistance;
    private bool preparingAttack => state is ShootEnemyState.PREPARING_ATTACK && inCooldown;
    private bool canCheckBulletCollision => state is ShootEnemyState.SHOOTING && bullet;
    private bool bulletHasCollide => bullet &&
                                     Physics2D.OverlapCircle(bullet.transform.position, 0.4f,
                                         LayerMask.NameToLayer("PlayerTrigger"));
}