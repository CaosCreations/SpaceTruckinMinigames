using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

public class StackMinigameUI : MonoBehaviour
{
    [SerializeField] private GameplayManager gameplayManager;

    [SerializeField] private Button stackButton;
    [SerializeField] private Button replayButton;

    [SerializeField] private Text outcomeText;

    [SerializeField] private GameResultMessage[] gameResultMessages;

    private Dictionary<string, GameResultMessage> gameResultMessageDictionary = new Dictionary<string, GameResultMessage>();

    private void Awake()
    {
        foreach(GameResultMessage item in gameResultMessages)
        {
            gameResultMessageDictionary.Add(item.State, item);
        }

        stackButton.onClick.RemoveAllListeners();
        stackButton.onClick.AddListener(gameplayManager.DoPlayButton);

        replayButton.onClick.RemoveAllListeners();
        replayButton.onClick.AddListener(gameplayManager.ResetGame);

        gameplayManager.GameResetEvent += () => SetGameUI(gameplayManager.GameStates);
        gameplayManager.GameEndEvent += () => SetGameUI(gameplayManager.GameStates);
    }

    public void SetGameUI(GameState gameState)
    {
        string result;

        foreach (GameResultMessage item in gameResultMessages) 
        { 
            if(gameState.TryGetState(item.State, out result) == true && result == gameState.CurrentState)
            {
                outcomeText.text = item.Message;

                stackButton.gameObject.SetActive(item.StackButtonActive);
                replayButton.gameObject.SetActive(item.ReplayButtonActive);

                return;
            }
        }
    }
}

