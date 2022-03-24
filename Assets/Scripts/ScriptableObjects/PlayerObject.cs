using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player_SO", menuName = "ScriptableObjects/Player")]
public class PlayerObject : ScriptableObject
{
   public float health;
   public float maxHealth;
   public float damage;

   private void OnEnable()
   {
      health = maxHealth;
   }
}
