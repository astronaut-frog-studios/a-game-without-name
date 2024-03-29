using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightDetect : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            PlayerEvents.OnEnemyDetected(false);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            PlayerEvents.OnEnemyDetected(true);
        }
    }
}
