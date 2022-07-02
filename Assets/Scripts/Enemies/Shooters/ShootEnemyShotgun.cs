using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemyShotgun : ShootEnemyBase
{
    [Header("Shotgunner")]
    [SerializeField] private int numberOfBullets;
    [SerializeField] private float projectileSpread;

    private List<Bullet> projectiles = new List<Bullet>();

    protected override void Awake()
    {
        base.Awake();
        bulletSpeed = Difficulty.Instance.enemyBulletSpeed;
        numberOfBullets = Difficulty.Instance.enemyNumberOfBullets;
    }

    protected override void ChangeEnemyDifficulty()
    {
        base.ChangeEnemyDifficulty();
        bulletSpeed += Difficulty.Instance.enemyBulletSpeed;
        numberOfBullets += Difficulty.Instance.enemyNumberOfBullets;
    }

    protected override void FixedUpdate()
    {
        if (health <= 0)
        {
            if (projectiles.Count <= 0) return;

            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i])
                    Destroy(projectiles[i].gameObject);
            }

            return;
        }

        if (canCheckBulletCollision)
        {
            CheckBulletCollision();
        }

        base.FixedUpdate();
    }

    protected override void Shoot()
    {
        base.Shoot();

        projectiles.Clear();

        var targetDirection = GetTargetDirection(target.position);
        var facingRotation = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        var startRotation = facingRotation + projectileSpread / 2f;
        var angleIncrease = projectileSpread / ((float)numberOfBullets - 1f);

        for (int i = 0; i < numberOfBullets; i++)
        {
            var tempRotation = startRotation - angleIncrease * i;
            var projectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            projectiles.Add(projectile);

            if (projectile)
            {
                var directionX = Mathf.Cos(tempRotation * Mathf.Deg2Rad);
                var directionY = Mathf.Sin(tempRotation * Mathf.Deg2Rad);
                var direction = new Vector2(directionX, directionY);

                projectile.rigid.velocity = direction.normalized * bulletSpeed;
            }
        }
    }

    protected override void CheckBulletCollision()
    {
        base.CheckBulletCollision();

        for (int i = 0; i < projectiles.Count; i++)
        {
            var projectile = projectiles[i];

            if (!projectile)
            {
                return;
            }

            projectile.OnBulletCollide(() => PlayerEvents.OnDamageReceived(enemy.damage), "Player");
        }
    }

    protected override void OnPlayerClose()
    {
        base.OnPlayerClose();

        if (inCooldown) return;
        Shoot();
    }

    private bool canCheckBulletCollision => (state is ShootEnemyBaseState.SHOOTING || state is ShootEnemyBaseState.PREPARING_ATTACK)
                                                 && projectiles.Count > 0;
}
