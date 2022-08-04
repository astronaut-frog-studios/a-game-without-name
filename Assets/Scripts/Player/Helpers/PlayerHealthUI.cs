using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Color low;

    [SerializeField] private Color high;

    [SerializeField] private Slider healthSlider;

    [SerializeField] private PlayerObject player;

    public void OnHealthChange(float amount)
    {
        if (amount <= 0.2)
        {
            healthSlider.fillRect.gameObject.SetActive(false);
            return;
        }

        
        healthSlider.maxValue = PlayerManager.Instance.MaxHealth;
        healthSlider.value = amount;

        healthSlider.fillRect.GetComponentInChildren<Image>().color =
            Color.Lerp(low, high, healthSlider.normalizedValue);
    }
}