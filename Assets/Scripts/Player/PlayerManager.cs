using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerObject playerObject;

    public float Health { get; private set; }
    public float MaxHealth { get; private set; }
    public float Damage { get; private set; }

    [SerializeField] private PlayerHealthUI healthBar;
    [SerializeField] private PlayerAmmoUI ammoBar;
    [SerializeField] private float health;
    [SerializeField] private float damage;

    private PlayerMeleeCollision playerCollision;

    private void Start()
    {
        PlayerEvents.DamageReceived += ReceivedDamage;
        PlayerEvents.PlayerDifficulty += PlayerDifficultyChange;

        healthBar = GetComponent<PlayerHealthUI>();
        ammoBar = GetComponent<PlayerAmmoUI>();
        playerCollision = GetComponent<PlayerMeleeCollision>();

        Health = playerObject.health + Difficulty.Instance.playerHealth;
        MaxHealth = playerObject.maxHealth + Difficulty.Instance.playerMaxHealth;
        Damage = playerObject.damage + Difficulty.Instance.playerDamage;

        healthBar.OnHealthChange(Health);
        health = Health;
        damage = Damage;
    }

    private void ReceivedDamage(float amountToLose, bool isMelee)
    {
        if (isMelee)
        {
            playerCollision.DamageReceived();
        }

        Health -= amountToLose;
        health = Health;
        healthBar.OnHealthChange(Health);
        
        if (Health <= 0.2f)
        {
            GameManager.Instance.OnGameEnded(false);
        }
    }

    private void PlayerDifficultyChange()
    {
        Difficulty.Instance.UpdatePlayerValues();

        Health += Difficulty.Instance.playerHealth;
        Damage += Difficulty.Instance.playerDamage;

        health = Health;
        damage = Damage;
    }
}