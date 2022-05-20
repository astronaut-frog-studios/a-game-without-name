using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyOpacity_SO", menuName = "ScriptableObjects/Enemy/EnemyOpacity")]
public class EnemyOpacityObject : ScriptableObject
{
    public float intialOpacity = 1.0f;
    public float minOpacity = 0.5f;
    public float maxOpacity = 0.84f;

}
