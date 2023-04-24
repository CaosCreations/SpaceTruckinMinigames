using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TileWalkingUI : MonoBehaviour
{
    [Header("Dependencies")]

    [SerializeField] private GridManager gridManager;

    [SerializeField] private Timer timer;

    [SerializeField] private PlayerControls playerControls;

    [Header("UI")]

    [SerializeField] private Text endGameText;

    [SerializeField] private Button gameOverButton;

    [SerializeField] private Text currentScoreText;

    [SerializeField] private Text timerText;


    private void Awake()
    {
        gameOverButton.onClick.RemoveAllListeners();
        gameOverButton.onClick.AddListener(gridManager.ResetGrid);
        gameOverButton.onClick.AddListener(playerControls.ResetPlayerMovement);
        gameOverButton.onClick.AddListener(DisableAllUIElements);

        gridManager.GameEventUpdatedEvent += UpdateUI;
        gridManager.TileStatusChangedEvent += UpdateCurrentScore;

        timer.TimerUpdatedEvent += UpdateTimer;
    }

    private void UpdateUI(GameState gameState)
    {
        if (gameState.CheckCurrentState("full win"))
            ToggleEndGameUI(message: "You won!");

        else if (gameState.CheckCurrentState("partial win"))
            ToggleEndGameUI(message: "You (kinda) won.");

        else if (gameState.CheckCurrentState("lose"))
            ToggleEndGameUI(message: "You lost.");
    }

    private void ToggleEndGameUI(string message)
    {
        endGameText.text = message;
        endGameText.gameObject.SetActive(true);
        gameOverButton.gameObject.SetActive(true);
    }

    private void UpdateCurrentScore(Tile tile)
    {
        if(tile.TileStatus != TileStatus.Touched)
        {
            return;
        }

        int number = 5 + gridManager.touchedTiles_Percent;

        if(number > 100)
            number = 100;

        currentScoreText.text = number.ToString() + "%";
    }

    private void UpdateTimer(int timeLeft)
    {
        timerText.text= timeLeft.ToString();
    }

    private void DisableAllUIElements()
    {
        endGameText.gameObject.SetActive(false);
        gameOverButton.gameObject.SetActive(false);
    }
}
