using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerObject playerObject;

    public float Health { get; private set; }
    public float Damage { get; private set; }

    [SerializeField] private float health;
    [SerializeField] private float damage;

    private PlayerMeleeCollision playerCollision;

    private void Start()
    {
        PlayerEvents.DamageReceived += ReceivedDamage;
        PlayerEvents.PlayerDifficulty += PlayerDifficultyChange;

        playerCollision = GetComponent<PlayerMeleeCollision>();

        Health = playerObject.health + Difficulty.Instance.playerHealth;
        Damage = playerObject.damage + Difficulty.Instance.playerDamage;

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

        print("Health: " + Health);

        if (Health <= 0)
        {
            GameManager.Instance.OnGameEnded(false);
            //Destroy(gameObject);
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