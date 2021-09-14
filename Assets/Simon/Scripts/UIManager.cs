using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button startGameButton;

    [SerializeField] private Text gameOverText;

    [SerializeField] private Text finalScoreText;

    public void ToggleGameOverUI(bool onOff)
    {
        startGameButton.gameObject.SetActive(onOff);
        gameOverText.gameObject.SetActive(onOff);
        finalScoreText.gameObject.SetActive(onOff);
    }

    public void ChangeScore(int score)
    {
        finalScoreText.text = score.ToString();
    }
}
