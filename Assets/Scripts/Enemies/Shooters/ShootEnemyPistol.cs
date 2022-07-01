using System.Collections;
using UnityEngine;

public class ShootEnemyPistol : ShootEnemyBase
{
    private Bullet projectile;

    protected override void Awake()
    {
        base.Awake();
        bulletSpeed = (Difficulty.Instance.enemyBulletSpeed + 1);
    }

    protected override void ChangeEnemyDifficulty()
    {
        base.ChangeEnemyDifficulty();
        bulletSpeed += (Difficulty.Instance.enemyBulletSpeed + 1);
    }

    protected override void FixedUpdate()
    {
        if (health <= 0)
        {
            if (!projectile) return;
            Destroy(projectile.gameObject);

            return;
        }

        if (canCheckBulletCollision)
        {
            CheckBulletCollision();
        }

        base.FixedUpdate();
    }

    protected override void OnPlayerClose()
    {
        base.OnPlayerClose();
        rigidbody.velocity = GetTargetDirection(target.transform.position) * -enemy.speed;
    }

    protected override void Shoot()
    {
        base.Shoot();
        projectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);


        var direction = GetTargetDirection(target.position);
        projectile.rigid.velocity = direction * bulletSpeed;
    }

    protected override void CheckBulletCollision()
    {
        base.CheckBulletCollision();

        if (!projectile)
        {
            return;
        }

        if (bulletCollideWithPlayer)
        {
            Destroy(projectile.gameObject);
            PlayerEvents.OnDamageReceived(enemy.damage);
        }
    }

    private bool canCheckBulletCollision => (state is ShootEnemyBaseState.SHOOTING || state is ShootEnemyBaseState.PREPARING_ATTACK) && projectile;
    private bool bulletCollideWithPlayer => projectile &&
                                     Physics2D.OverlapCircle(projectile.transform.position, 0.3f,
                                         LayerMask.NameToLayer("PlayerTrigger"));
}