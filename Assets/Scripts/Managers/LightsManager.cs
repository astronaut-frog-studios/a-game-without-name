using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightsManager : Singleton<LightsManager>
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private float globalLightOn = 2f;
    [SerializeField] private float globalLightOff = 0.7f;

    [SerializeField] private Color globalLightColorInitial;
    [SerializeField] private Color globalLightColorWaves;

    public delegate void ChangeFlickerLightsHandler(bool isOff, bool canFlick);
    public event ChangeFlickerLightsHandler ChangeFlickerLights;

    public delegate void TurnOnPlayerLightsHandler(bool isOn);
    public event TurnOnPlayerLightsHandler TurnOnPlayerLights;

    private void Start()
    {
        globalLight.intensity = globalLightOn;
        RoundsManager.Instance.TurnLightsOff += OnWaveStarted;
        RoundsManager.Instance.TurnLightsOn += OnRoundStarted;
    }

    private void OnRoundStarted()
    {
        ChangeFlickerLights?.Invoke(false, false);
        TurnOnPlayerLights?.Invoke(false);

        globalLight.intensity = globalLightOn;
        globalLight.color = globalLightColorInitial;
    }

    private void OnWaveStarted()
    {
        var isOff = Helpers.GenerateOneOrZeroBool();
        var canFlick = !isOff || Helpers.GenerateOneOrZeroBool();
        ChangeFlickerLights?.Invoke(isOff, canFlick);
        TurnOnPlayerLights?.Invoke(true);

        globalLight.intensity = globalLightOff;
        globalLight.color = globalLightColorWaves;

    }
}