using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TileWalkingUI : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    [SerializeField] private PlayerControls playerControls;

    [SerializeField] private Text endGameText;

    [SerializeField] private Button gameOverButton;

    [SerializeField] private Text currentScoreText;

    private void Awake()
    {
        gameOverButton.onClick.RemoveAllListeners();
        gameOverButton.onClick.AddListener(gridManager.ResetGrid);
        gameOverButton.onClick.AddListener(playerControls.ResetPlayerMovement);
        gameOverButton.onClick.AddListener(DisableAllUIElements);

        gridManager.GameEventUpdatedEvent += UpdateUI;
        gridManager.TileStatusChangedEvent += UpdateCurrentScore;
    }

    private void UpdateUI(GameState gameState)
    {
        if (gameState.CurrentState == "full win")
            ToggleEndGameUI(message: "You won!");

        else if (gameState.CurrentState == "partial win")
            ToggleEndGameUI(message: "You (kinda) won.");

        else if (gameState.CurrentState == "lose")
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
        if(tile.TileStatus == TileStatus.Touched)
            currentScoreText.text = gridManager.touchedTiles_Percent.ToString() + "%";
    }

    private void DisableAllUIElements()
    {
        endGameText.gameObject.SetActive(false);
        gameOverButton.gameObject.SetActive(false);
    }
}
