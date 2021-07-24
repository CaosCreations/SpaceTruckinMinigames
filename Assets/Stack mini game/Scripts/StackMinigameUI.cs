using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackMinigameUI : MonoBehaviour
{
    [SerializeField] private GameObject stackButton;
    [SerializeField] private GameObject replayButton;

    [SerializeField] private Text outcomeText;


    public void SetGameUI(GameState gameState)
    {
        if(gameState == GameState.NewGame)
        {
            outcomeText.text = "";
            stackButton.SetActive(true);
            replayButton.SetActive(false);
        }

        else if(gameState == GameState.Win)
        {
            outcomeText.text = "You won!";
            stackButton.SetActive(false);
            replayButton.SetActive(true);
        }

        else if(gameState == GameState.Lose)
        {
            outcomeText.text = "You lose!";
            stackButton.SetActive(false);
            replayButton.SetActive(true);
        }
    }
}
