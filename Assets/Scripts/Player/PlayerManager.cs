using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerObject playerObject;

    public float health { get; private set; }
    public float damage { get; private set; }

    private void Start()
    {
        PlayerEvents.DamageReceived += ReceivedDamage;

        health = playerObject.health;
        damage = playerObject.damage;
    }

    private void ReceivedDamage(float amountToLose)
    {
        health -= amountToLose;
        
        print("Health: " + health);

        if (health <= 0) print("dead"); //Destroy(gameObject);
    }
}