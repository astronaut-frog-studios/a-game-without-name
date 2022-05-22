using UnityEngine.Events;

public static class PlayerEvents
{

    public static event UnityAction<float> DamageReceived;
    public static void OnDamageReceived(float damage) => DamageReceived?.Invoke(damage);

    public static event UnityAction<bool> EnemyDetected;
    public static void OnEnemyDetected(bool enemyIsGhost) => EnemyDetected?.Invoke(enemyIsGhost);

    public static event UnityAction<bool> PlayerHided;
    public static void OnPlayerHided(bool isHiding) => PlayerHided?.Invoke(isHiding);

}