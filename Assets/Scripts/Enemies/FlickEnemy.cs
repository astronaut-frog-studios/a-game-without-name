using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlickEnemy : MonoBehaviour
{
    public bool isGhost = true;
    private bool isFlickering;
    private float flickerDelay;
    private float opacity;
    private SpriteRenderer spriteComponent;
    private Color spriteColor;

    public EnemyOpacityObject enemyOpacity;
    private float baseOpacity, minOpacity, maxOpacity;

    private void Start()
    {
        PlayerEvents.EnemyDetected += SetIsGhost;

        baseOpacity = enemyOpacity.intialOpacity;
        minOpacity = enemyOpacity.minOpacity;
        maxOpacity = enemyOpacity.maxOpacity;

        spriteComponent = gameObject.GetComponent<SpriteRenderer>();
        spriteColor = spriteComponent.color;
    }

    private void Update()
    {
        if (!isGhost)
        {
            spriteColor.a = baseOpacity;
            spriteComponent.color = spriteColor;
            return;
        }

        if (isFlickering) return;
        StartCoroutine(FlickeringLight());
    }

    private void SetIsGhost(bool isGhost)
    {
        this.isGhost = isGhost;
    }

    private IEnumerator FlickeringLight()
    {
        isFlickering = true;

        opacity = Random.Range(minOpacity - 0.2f, maxOpacity - 0.12f);
        spriteColor.a = opacity;
        spriteComponent.color = spriteColor;

        flickerDelay = Random.Range(0.08f, 0.2f);
        yield return new WaitForSeconds(flickerDelay);

        opacity = Random.Range(minOpacity + 0.2f, maxOpacity + 0.2f);
        spriteColor.a = opacity;
        spriteComponent.color = spriteColor;

        isFlickering = false;
    }
}
