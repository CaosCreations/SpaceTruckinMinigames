using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField] private Button startGameButton;

    [Range(0.5f, 2.0f)]
    [SerializeField] private float waitBetweenColorSequences;

    private MiniGamePhases currentMiniGamePhases = MiniGamePhases.WatchingPhase;

    private Sequence sequence;

    private UIManager uiManager;

    private ColorButtonEffectPlayer colorButtonEffectPlayer;

    private int score = 0;


    private void Awake()
    {
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
        sequence.CreateColorSequence(2);
        yield return StartCoroutine(PlayColorSequenceCoroutine());
    }

    private IEnumerator PlayColorSequenceCoroutine()
    {
        currentMiniGamePhases = MiniGamePhases.WatchingPhase;
        yield return StartCoroutine(colorButtonEffectPlayer.PlayButtonSequence(sequence.ColorSequence));
        currentMiniGamePhases = MiniGamePhases.PlayingPhase;
    }

    public IEnumerator SelectColor(Colors color)
    {
        if (currentMiniGamePhases == MiniGamePhases.WatchingPhase)
        {
            yield break;
        }

        currentMiniGamePhases = MiniGamePhases.WatchingPhase;
        yield return StartCoroutine(colorButtonEffectPlayer.PlayButtonAudioAndVisual(color));
        currentMiniGamePhases = MiniGamePhases.PlayingPhase;

        if (!sequence.ColorIsSameAsCurrentSequenceColor(color))
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
            currentMiniGamePhases = MiniGamePhases.WatchingPhase;
            score++;
            yield return new WaitForSeconds(waitBetweenColorSequences);
            StartCoroutine(SetNextRound());
        }
    }

    private IEnumerator SetNextRound()
    {
        sequence.ExtendColorSequence();
        sequence.ResetSequenceIndex();
        yield return StartCoroutine(PlayColorSequenceCoroutine());
    }

    private void GameOver()
    {
        currentMiniGamePhases = MiniGamePhases.WatchingPhase;

        sequence.ResetSequenceIndex();

        uiManager.ChangeScore(score);

        uiManager.ToggleGameOverUI(onOff: true);
    }
}
