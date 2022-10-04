using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickerLight : MonoBehaviour
{
    [SerializeField] private bool canFlick;
    [SerializeField] private bool isOff;
    [SerializeField] private float baseIntensity = 1.3f;

    private bool isFlickering;
    private float lightIntensity;
    private float flickerDelayTime;
    private Light2D light2DComponent;

    private void Start()
    {
        light2DComponent = gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        LightsManager.Instance.ChangeFlickerLights += ChangeLightProperties;
    }

    private void Update()
    {
        if (!isOff)
        {
            light2DComponent.intensity = baseIntensity;
        }
        else
        {
            light2DComponent.intensity = 0;
            return;
        }

        if (!canFlick) return;
        
        if (isFlickering) return;
        StartCoroutine(FlickeringLight());
    }

    private void ChangeLightProperties(bool off, bool flick)
    {
        isOff = off;
        canFlick = flick;
    }
    
    private IEnumerator FlickeringLight()
    {
        isFlickering = true;

        lightIntensity = Random.Range(0.2f, 1.2f);
        light2DComponent.intensity = lightIntensity;

        flickerDelayTime = Random.Range(0.05f, 0.2f);
        yield return new WaitForSeconds(flickerDelayTime);

        lightIntensity = Random.Range(1f, 2f);
        light2DComponent.intensity = lightIntensity;

        isFlickering = false;
    }
}