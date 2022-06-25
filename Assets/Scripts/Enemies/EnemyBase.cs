using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Common Variables")]
    [SerializeField] private EnemyOpacityObject enemyOpacity;
    [SerializeField] protected EnemyObject enemy;
    [ReadOnly, SerializeField] protected float health;
    [ReadOnly, SerializeField] protected float attackCooldown;
    [ReadOnly, SerializeField] protected float damage;
    [HideInInspector] public bool takenDamage;
    [HideInInspector] public float maxHealth;
    protected new Rigidbody2D rigidbody;
    protected Transform target;

    [Header("Detect Player")]
    [SerializeField] private LayerMask layersToDetect;
    [ReadOnly, SerializeField] private bool detectSneakyPlayer;
    [ReadOnly, SerializeField] private bool playerIsHidingEventCalled;

    private Animator enemyAnim;
    private readonly int Explode = Animator.StringToHash("Explode");

    public UnityAction<float> HealthChange;
    private void OnHealthChange(float currentHealth) => HealthChange?.Invoke(currentHealth);

    protected virtual void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;

        var flickEnemy = gameObject.AddComponent<FlickEnemy>();
        flickEnemy.enemyOpacity = enemyOpacity;

        enemyAnim = gameObject.GetComponent<Animator>();
        enemyAnim.runtimeAnimatorController = enemy.animController;


        health = enemy.health + Difficulty.Instance.enemyHealth;
        maxHealth = enemy.maxHealth;
        damage = enemy.damage + Difficulty.Instance.enemyDamage;
        attackCooldown = 0 - Difficulty.Instance.enemyCooldown;

        OnHealthChange(health);
        PlayerEvents.PlayerHid += PlayerIsHiding;
        Difficulty.EnemyDifficulty += ChangeEnemyDifficulty;
    }

    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        attackCooldown = 0;
    }

    void OnDestroy()
    {
        PlayerEvents.PlayerHid -= PlayerIsHiding;
    }

    private void ReceivedDamage(float amountToLose)
    {
        health -= amountToLose;
        takenDamage = health < maxHealth;

        OnHealthChange(health);

        if (health > 0) return;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        enemyAnim.SetTrigger(Explode);
        StartCoroutine(TimerToDestroy());
    }

    private void PlayerIsHiding(bool isHiding)
    {
        playerIsHidingEventCalled = isHiding;

        if (!playerIsHidingEventCalled) return;

        var rayHit2D = Physics2D.Raycast(transform.position, target.position - transform.position, enemy.detectRange, layersToDetect);

        if (rayHit2D)
        {
            if (!(rayHit2D.transform.CompareTag("Player")))
            {
                detectSneakyPlayer = false;
                Debug.DrawLine(transform.position, rayHit2D.point, Color.red);
                return;
            }

            detectSneakyPlayer = true;
            Debug.DrawLine(transform.position, rayHit2D.point, Color.green);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("PlayerBullet"))
        {
            Destroy(col.gameObject);
            ReceivedDamage(PlayerManager.Instance.Damage);
        }
    }

    protected void CheckAttackCooldown()
    {
        if (inCooldown)
            attackCooldown -= Time.deltaTime;
    }

    private void ChangeEnemyDifficulty()
    {
        health += Difficulty.Instance.enemyHealth;
        damage += Difficulty.Instance.enemyDamage;
        attackCooldown -= Difficulty.Instance.enemyCooldown;
    }

    private IEnumerator TimerToDestroy()
    {
        yield return new WaitForSeconds(0.12f);

        AudioSystem.Instance.PlaySfx("explode");

        yield return new WaitForSeconds(enemyAnim.GetCurrentAnimatorStateInfo(0).length +
                                        enemyAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);

        Destroy(gameObject);
    }

    #region getters
    protected bool canDetectPlayer => Vector2.Distance(transform.position, target.position) <= enemy.detectRange && !playerIsHidingEventCalled ||
    detectSneakyPlayer && playerIsHidingEventCalled;
    protected void StopEnemy() => rigidbody.velocity = Vector2.zero;
    protected bool closerToPlayer => target && Vector2.Distance(transform.position, target.position) <= enemy.attackRange;
    protected bool inCooldown => attackCooldown > 0;
    #endregion
}