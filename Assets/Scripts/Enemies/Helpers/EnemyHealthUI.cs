using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Color low;
    [SerializeField] private Color high;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private EnemyBase enemyPros;

    private void Start()
    {
        enemyPros = gameObject.GetComponent<EnemyBase>();
        enemyPros.HealthChange += ChangeHealthSlider;
    }
    
    private void ChangeHealthSlider(float amount)
    {
        if (amount <= 0)
        {
            healthSlider.gameObject.SetActive(false);
            return;
        }
    
        healthSlider.gameObject.SetActive(enemyPros.takenDamage);
        healthSlider.maxValue = enemyPros.maxHealth;
        healthSlider.value = amount;
    
        healthSlider.fillRect.GetComponentInChildren<Image>().color =
            Color.Lerp(low, high, healthSlider.normalizedValue);
    }
}