using UnityEngine;

[CreateAssetMenu(fileName = "SaveGames_SO", menuName = "ScriptableObjects/SaveGame")]
public class SaveGameObject : ScriptableObject
{
    [Header("Rounds")]
    public int currentRound = 0;

    [Header("Player")]
    public float playerHealth = 0;
    public PlayerObject playerSO;

    [Header("Cutscenes")]
    public string currentCutsceneName = "none";
    public int currentCutsceneIndex = 0;

    private void OnEnable()
    {
        playerHealth = playerSO.health;
    }
}
