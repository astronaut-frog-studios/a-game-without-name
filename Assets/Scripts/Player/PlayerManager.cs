using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerObject playerObject;

    public float Health { get; private set; }
    public float Damage { get; private set; }

    [SerializeField] private float health;
    [SerializeField] private float damage;

    private PlayerCollisions playerCollisions;

    private void Start()
    {
        PlayerEvents.DamageReceived += ReceivedDamage;
        PlayerEvents.PlayerDifficulty += PlayerDifficultyChange;

        playerCollisions = GetComponent<PlayerCollisions>();

        Health = playerObject.health + Difficulty.Instance.playerHealth;
        Damage = playerObject.damage + Difficulty.Instance.playerDamage;

        health = Health;
        damage = Damage;
    }

    private void ReceivedDamage(float amountToLose)
    {
        playerCollisions.DamageReceived();
        Health -= amountToLose;

        print("Health: " + Health);

        if (Health <= 0) print("dead"); //Destroy(gameObject);
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