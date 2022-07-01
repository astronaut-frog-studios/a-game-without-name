using System.Collections;
using UnityEngine;

public enum ShootEnemyBaseState
{
    WAITING,
    WALKING,
    RUNNING,
    SHOOTING,
    PREPARING_ATTACK,
    SEARCHING_RANDOM_POS
};

public abstract class ShootEnemyBase : EnemyBase
{
    [Header("Shoot Enemy")]
    [SerializeField] protected ShootEnemyBaseState state = ShootEnemyBaseState.SEARCHING_RANDOM_POS;
    [Space(12.0f)]
    [SerializeField] private float stoppingDistance = 0.25f;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected Bullet bulletPrefab;

    private Vector2 randomPosition;

    protected virtual void FixedUpdate()
    {
        CheckAttackCooldown();

        if (state is ShootEnemyBaseState.WAITING) return;

        if (!canDetectPlayer && state != ShootEnemyBaseState.SHOOTING)
        {
            if (state != ShootEnemyBaseState.PREPARING_ATTACK)
            {
                CheckWalkingState();
                return;
            }

            state = ShootEnemyBaseState.WAITING;
            StartCoroutine(WaitToSearchRandomPos());
            return;
        }

        if (closerToPlayer)
        {
            state = ShootEnemyBaseState.RUNNING;
            OnPlayerClose();
            return;
        }

        FacePlayer();
        if (preparingAttack) return;
        state = ShootEnemyBaseState.PREPARING_ATTACK;
        StopEnemy();

        if (inCooldown) return;
        Shoot();
    }

    protected virtual void OnPlayerClose()
    {
        FacePlayer();
    }

    protected virtual void Shoot()
    {
        state = ShootEnemyBaseState.SHOOTING;
        attackCooldown = enemy.attackCooldown;
    }

    protected virtual void CheckBulletCollision()
    {
        state = ShootEnemyBaseState.PREPARING_ATTACK;
    }

    protected Vector2 GetTargetDirection(Vector2 targetPosition)
    {
        var direction = targetPosition - (Vector2)transform.position;
        direction.Normalize();

        return direction;
    }

    private void CheckWalkingState()
    {
        if (state is ShootEnemyBaseState.WALKING)
        {
            if (pathPending) return;

            state = ShootEnemyBaseState.WAITING;
            StartCoroutine(WaitToSearchRandomPos());
            return;
        }

        if (state != ShootEnemyBaseState.SEARCHING_RANDOM_POS) return;
        WalkToRandomPos();
    }

    private void WalkToRandomPos()
    {
        randomPosition = new Vector2(LevelBounds.RandomPosX, LevelBounds.RandomPosY);

        rigidbody.velocity = GetTargetDirection(randomPosition) * enemy.speed;
        state = ShootEnemyBaseState.WALKING;
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
        state = ShootEnemyBaseState.SEARCHING_RANDOM_POS;
    }

    protected bool pathPending => Vector2.Distance(rigidbody.position, randomPosition) > stoppingDistance;
    protected bool preparingAttack => state is ShootEnemyBaseState.PREPARING_ATTACK && inCooldown;
}