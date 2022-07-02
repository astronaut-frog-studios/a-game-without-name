using UnityEngine.Events;

public static class PlayerEvents
{

    public static event UnityAction<float, bool> DamageReceived;
    public static void OnDamageReceived(float damage, bool isMelee = false) => DamageReceived?.Invoke(damage, isMelee);

    public static event UnityAction<bool> EnemyDetected;
    public static void OnEnemyDetected(bool enemyIsGhost) => EnemyDetected?.Invoke(enemyIsGhost);

    public static event UnityAction<bool> PlayerHid;
    public static void OnPlayerHid(bool isHiding) => PlayerHid?.Invoke(isHiding);

    public static event UnityAction PlayerDifficulty;
    public static void OnPlayerDifficultyChange() => PlayerDifficulty?.Invoke();
}