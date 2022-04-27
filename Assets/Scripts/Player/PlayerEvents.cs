using UnityEngine.Events;

public static class PlayerEvents
{
    public static event UnityAction<float> DamageReceived;
    public static void OnDamageReceived(float damage) => DamageReceived?.Invoke(damage);
}