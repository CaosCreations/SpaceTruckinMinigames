using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackMinigameUI : MonoBehaviour
{
    [SerializeField] private GameObject stackButton;
    [SerializeField] private GameObject replayButton;

    [SerializeField] private Text outcomeText;


    public void SetLoseUI()
    {
        outcomeText.text = "Game over!";
        stackButton.SetActive(false);
        replayButton.SetActive(true);
    }

    public void SetWinUI()
    {
        outcomeText.text = "You won!";
        stackButton.SetActive(false);
        replayButton.SetActive(true);
    }

    public void SetNewGameUI()
    {
        outcomeText.text = "";
        stackButton.SetActive(true);
        replayButton.SetActive(false);
    }

}
