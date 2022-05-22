using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerObject playerObject;

    public float Health { get; private set; }
    public float Damage { get; private set; }

    private void Start()
    {
        PlayerEvents.DamageReceived += ReceivedDamage;

        Health = playerObject.health;
        Damage = playerObject.damage;
    }

    private void ReceivedDamage(float amountToLose)
    {
        Health -= amountToLose;

        print("Health: " + Health);

        if (Health <= 0) print("dead"); //Destroy(gameObject);
    }
}