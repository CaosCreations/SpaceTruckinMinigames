using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField] private Button startGameButton;

    private MiniGamePhases currentMiniGamePhases = MiniGamePhases.WatchingPhase;

    private Sequence sequence;

    private UIManager UImanager;

    private ColorButtonEffectPlayer colorButtonEffectPlayer;

    private int score = 0;


    private void Awake()
    {
        startGameButton.onClick.RemoveAllListeners();
        startGameButton.onClick.AddListener(StartGameLoop);

        sequence = GameObject.FindObjectOfType<Sequence>();
        UImanager = GameObject.FindObjectOfType<UIManager>();
        colorButtonEffectPlayer = GameObject.FindObjectOfType<ColorButtonEffectPlayer>();
    }

    private void StartGameLoop()
    {
        StartCoroutine(StartGameLoopCoroutine());
    }

    private IEnumerator StartGameLoopCoroutine()
    {
        score = 0;
        UImanager.ToggleGameOverUI(onOff: false);
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
        yield return StartCoroutine(colorButtonEffectPlayer.PlayButton(color));
        currentMiniGamePhases = MiniGamePhases.PlayingPhase;

        if (sequence.CompareColorWithCurrentSequenceColor(color) == false && currentMiniGamePhases != MiniGamePhases.WatchingPhase)
        {
            GameOver();
            yield break;
        }

        else
        {
            sequence.IterateSequence();
        }

        if (sequence.SequenceReachedLastItem() == true)
        {
            currentMiniGamePhases = MiniGamePhases.WatchingPhase;
            score++;
            yield return new WaitForSeconds(1f);
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

        UImanager.ChangeScore(score);

        UImanager.ToggleGameOverUI(onOff: true);
    }
}
