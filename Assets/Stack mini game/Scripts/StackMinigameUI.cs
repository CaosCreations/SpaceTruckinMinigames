using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackMinigameUI : MonoBehaviour
{
    [SerializeField] private GameplayManager stackMinigameManager;

    [SerializeField] private Button stackButton;
    [SerializeField] private Button replayButton;

    [SerializeField] private Text outcomeText;

    private void Awake()
    {
        stackButton.onClick.RemoveAllListeners();
        stackButton.onClick.AddListener(stackMinigameManager.DoPlayButton);

        replayButton.onClick.RemoveAllListeners();
        replayButton.onClick.AddListener(stackMinigameManager.ResetGame);

        stackMinigameManager.GameResetEvent += () => SetGameUI(GameState.NewGame);
        stackMinigameManager.GameWinEvent += () => SetGameUI(GameState.Win);
        stackMinigameManager.GameLoseEvent += () => SetGameUI(GameState.Lose);
    }

    public void SetGameUI(GameState gameState)
    {
        outcomeText.text = GetGameOutcomeText(gameState);

        switch (gameState)
        {
            case GameState.NewGame:
                stackButton.gameObject.SetActive(true);
                replayButton.gameObject.SetActive(false);
                break;

            case GameState.Win:
            case GameState.Lose:
                stackButton.gameObject.SetActive(false);
                replayButton.gameObject.SetActive(true);
                break;

            default:
                break;
        }
    }

    private string GetGameOutcomeText(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.NewGame:
                return string.Empty;

            case GameState.Win:
                return "You won!";

            case GameState.Lose:
                return "You lose!";

            default:
                return string.Empty;
        }
    }
}

