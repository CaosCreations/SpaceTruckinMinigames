using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField] private GameState gameState;

    [SerializeField] private Button startGameButton;

    [Range(0.5f, 2.0f)]
    [SerializeField] private float waitBetweenColorSequences;

    private Sequence sequence;

    private UIManager uiManager;

    private ColorButtonEffectPlayer colorButtonEffectPlayer;

    private int score = 0;

    private void Awake()
    {
        gameState.SetCurrentState("watching phase");

        startGameButton.onClick.RemoveAllListeners();
        startGameButton.onClick.AddListener(StartGameLoop);

        sequence = FindObjectOfType<Sequence>();
        uiManager = FindObjectOfType<UIManager>();
        colorButtonEffectPlayer = FindObjectOfType<ColorButtonEffectPlayer>();
    }

    private void StartGameLoop()
    {
        StartCoroutine(StartGameLoopCoroutine());
    }

    private IEnumerator StartGameLoopCoroutine()
    {
        score = 0;
        uiManager.ToggleGameOverUI(onOff: false);
        sequence.CreateColorButtonSequence(2);
        yield return StartCoroutine(PlayColorSequenceCoroutine());
    }

    private IEnumerator PlayColorSequenceCoroutine()
    {
        gameState.SetCurrentState("watching phase");
        yield return StartCoroutine(colorButtonEffectPlayer.PlayButtonSequence(sequence.ColorButtonSequence));
        gameState.SetCurrentState("playing phase");
    }

    public IEnumerator SelectColor(ColorButton colorButton)
    {
        if (gameState.CheckCurrentState("watching phase"))
        {
            yield break;
        }

        gameState.SetCurrentState("watching phase");
        yield return StartCoroutine(colorButtonEffectPlayer.PlayButtonAudioAndVisual(colorButton));
        gameState.SetCurrentState("playing phase");

        if (!sequence.ColorButtonIsSameAsCurrentSequenceColor(colorButton))
        {
            GameOver();
            yield break;
        }

        else
        {
            sequence.IterateSequence();
        }

        if (sequence.SequenceReachedLastItem)
        {
            gameState.SetCurrentState("watching phase");
            score++;
            yield return new WaitForSeconds(waitBetweenColorSequences);
            StartCoroutine(SetNextRound());
        }
    }

    private IEnumerator SetNextRound()
    {
        sequence.ExtendColorButtonSequence();
        sequence.ResetSequenceIndex();
        yield return StartCoroutine(PlayColorSequenceCoroutine());
    }

    private void GameOver()
    {
        gameState.SetCurrentState("watching phase");

        sequence.ResetSequenceIndex();

        uiManager.ChangeScore(score);

        uiManager.ToggleGameOverUI(onOff: true);
    }
}
