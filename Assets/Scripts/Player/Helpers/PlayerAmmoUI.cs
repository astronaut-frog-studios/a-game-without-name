using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmoUI : MonoBehaviour
{
    [SerializeField] private Color low;

    [SerializeField] private Color high;

    [SerializeField] private Slider ammoSlider;

    [SerializeField] private PlayerObject player;

    public void OnAmmoChange(float amount)
    {
        ammoSlider.maxValue = player.maxHealth;
        ammoSlider.value = amount;

        ammoSlider.fillRect.GetComponentInChildren<Image>().color =
            Color.Lerp(low, high, ammoSlider.normalizedValue);
    }
}