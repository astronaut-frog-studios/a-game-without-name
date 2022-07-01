using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemyLmg : ShootEnemyBase
{
    [Header("LMG Gunner")]
    [SerializeField] private int numberOfBullets;

    private List<Bullet> projectiles = new List<Bullet>();

    protected override void Awake()
    {
        base.Awake();
        bulletSpeed = (Difficulty.Instance.enemyBulletSpeed - 0.8f);
        numberOfBullets = (Difficulty.Instance.enemyNumberOfBullets + 4);
    }

    protected override void ChangeEnemyDifficulty()
    {
        base.ChangeEnemyDifficulty();
        bulletSpeed += (Difficulty.Instance.enemyBulletSpeed - 0.8f);
        numberOfBullets += (Difficulty.Instance.enemyNumberOfBullets + 4);
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

        var angleStep = 360f / numberOfBullets;
        var angle = 0f;

        for (int i = 0; i < numberOfBullets; i++)
        {
            var projectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            projectiles.Add(projectile);

            if (projectile)
            {
                var directionX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f) * 1;
                var directionY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f) * 1;
                var direction = new Vector2(directionX, directionY);

                projectile.rigid.velocity = (direction - (Vector2)transform.position).normalized * bulletSpeed;
            }

            angle += angleStep;
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

            var bulletCollideWithPlayer = projectile &&
            Physics2D.OverlapCircle(projectile.transform.position, 0.3f,
                 LayerMask.NameToLayer("PlayerTrigger"));

            if (bulletCollideWithPlayer)
            {
                Destroy(projectile.gameObject);
                PlayerEvents.OnDamageReceived(enemy.damage);
            }
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
