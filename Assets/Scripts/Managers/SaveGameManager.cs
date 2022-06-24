using UnityEngine;
using UnityEngine.UI;


public class SaveGameManager : PersistentSingleton<SaveGameManager>
{
    [SerializeField] private SaveGameObject saveGameObject;
    public int getSavedRound => saveGameObject.currentRound;

    public void SaveAll()
    {
        saveGameObject.currentRound = GameManager.Instance.getCurrentRound;
        Difficulty.Instance.OnSave();
    }
}
