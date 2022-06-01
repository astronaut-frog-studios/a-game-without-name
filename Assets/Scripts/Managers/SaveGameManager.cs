using UnityEngine;
using UnityEngine.UI;


public class SaveGameManager : PersistentSingleton<SaveGameManager>
{
    [SerializeField] private SaveGameObject saveGameObject;

    [ReadOnly] public int currentRound;
    [SerializeField] public Text displayRound;

    private void Start()
    {
        SetCurrentRound();
    }

    private void SetCurrentRound()
    {
        currentRound = saveGameObject.currentRound;
        displayRound.text = currentRound.ToString();
    }

    public void SaveAll()
    {
        saveGameObject.currentRound = currentRound;
    }
}
