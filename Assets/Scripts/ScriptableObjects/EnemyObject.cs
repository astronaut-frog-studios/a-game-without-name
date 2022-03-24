using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_SO", menuName = "ScriptableObjects/Enemy/Enemy")]
public class EnemyObject : ScriptableObject
{
    public float health;
    public float maxHealth = 100f;
    public float speed;
    public float damage = 8f;
    public float detectRange = 5f;
    public float attackRange = 2.5f;
    public float attackCooldown = 10f;
    public RuntimeAnimatorController animController;

    private void OnEnable()
    {
        health = maxHealth;
    }
}
