using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLights : MonoBehaviour
{
    [SerializeField] private Light2D highlight;
    [SerializeField] private Light2D flashlight;

    private void Start()
    {
        LightsManager.Instance.TurnOnPlayerLights += TurnLightsOn;
    }

    private void TurnLightsOn(bool isOn)
    {
        highlight.enabled = isOn;
        flashlight.enabled = isOn;
    }
}