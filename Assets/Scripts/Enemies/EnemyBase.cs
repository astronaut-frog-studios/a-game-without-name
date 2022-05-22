using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] private EnemyOpacityObject enemyOpacity;
    [SerializeField] protected EnemyObject enemy;
    [ReadOnly, SerializeField] protected float health;
    [HideInInspector] public bool takenDamage;
    public float maxHealth;
    protected Transform target;

    [SerializeField] private LayerMask layersToDetect;
    [ReadOnly, SerializeField] private bool detectSneakyPlayer;
    [ReadOnly, SerializeField] private bool playerIsHidingEventCalled;

    private Animator enemyAnim;
    private readonly int Explode = Animator.StringToHash("Explode");

    public UnityAction<float> HealthChange;
    private void OnHealthChange(float currentHealth) => HealthChange?.Invoke(currentHealth);

    protected virtual void Awake()
    {
        var flickEnemy = gameObject.AddComponent<FlickEnemy>();
        flickEnemy.enemyOpacity = enemyOpacity;

        enemyAnim = gameObject.GetComponent<Animator>();
        enemyAnim.runtimeAnimatorController = enemy.animController;
        target = GameObject.FindWithTag("Player").transform;

        health = enemy.health;
        maxHealth = enemy.maxHealth;

        OnHealthChange(health);
        PlayerEvents.PlayerHided += PlayerIsHiding;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("PlayerBullet"))
        {
            ReceivedDamage(PlayerManager.Instance.Damage);
        }
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
        if (rayHit2D.transform)
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

    private IEnumerator TimerToDestroy()
    {
        yield return new WaitForSeconds(0.12f);

        AudioSystem.Instance.PlaySfx("explode");

        yield return new WaitForSeconds(enemyAnim.GetCurrentAnimatorStateInfo(0).length +
                                        enemyAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);

        Destroy(gameObject);
    }

    protected bool canDetectPlayer => Vector2.Distance(transform.position, target.position) <= enemy.detectRange && !playerIsHidingEventCalled ||
    detectSneakyPlayer && playerIsHidingEventCalled;

}