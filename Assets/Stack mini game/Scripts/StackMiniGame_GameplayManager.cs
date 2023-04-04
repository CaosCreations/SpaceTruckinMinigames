using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackMiniGame_GameplayManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CubeSpawner cubeSpawner;

    [SerializeField] private CubeMover cubeMover;

    [SerializeField] private ScreenShaker screenShaker;

    [SerializeField] private CubeStack cubeStack;

    [SerializeField] private GameState gameStates;

    [Header("Gameplay")]

    [Range(0.0f, 2f)]
    [SerializeField] private float stackFreezeSeconds;

    [Range(0.0f, 2f)]
    [SerializeField] private float screenShakeSeconds;

    [SerializeField] private int fullWinScore;

    [SerializeField] private int partialWinScore;

    public GameState GameStates => gameStates;

    public Action GameResetEvent;

    public Action GameEndEvent;

    private IEnumerator stackCoroutine;


    private void Start()
    {
        ResetGame();
    }

    // When the player presses the play button, he or she attempts to stack the current moving cube on top of the cube below
    // If the cubes are stacked, the game goes on, and we spawn a new cube on top
    // If not it's game over

    public void StackCube()
    {
        if (stackCoroutine != null)
            return;

        stackCoroutine = StackCubeCoroutine();
        StartCoroutine(stackCoroutine);
    }

    private IEnumerator StackCubeCoroutine()
    {
        CubeCornersPositionTracker bottomCube = cubeStack.CubeCornersPositionPile.BottomCube;

        // There is only one cube at the beginning. The player can place it wherever. There is no cutting involved yet.
        if (cubeStack.CubeCornersPositionPile.TopCube == null)
        {
            screenShaker.ShakeScreen(seconds: screenShakeSeconds);
            yield return StartCoroutine(cubeMover.FreezeCubeMovement(stackFreezeSeconds));

            cubeSpawner.SpawnStackedCube(spawnPosition: bottomCube.transform.position,
                                         setWidth: bottomCube.transform.localScale.x);
            stackCoroutine = null;
            yield break;
        }

        CubeCornersPositionTracker topCube = cubeStack.CubeCornersPositionPile.TopCube;

        CubeOverlap cubeOverlap = cubeStack.GetCubeOverlap();

        // Cubes aren't stacked. It's game over
        if (cubeOverlap == CubeOverlap.None)
        {
            if(cubeStack.StackedCubes.Count >= partialWinScore+1)
                gameStates.SetCurrentState("partial win");
            else
                gameStates.SetCurrentState("lose");

            GameEndEvent?.Invoke();
            stackCoroutine = null;
            yield break;
        }

        // Cubes are stacked
        // we cut part of the top cube so that its side that
        // was sticking out is now aligned with that of the bottom cube
        cubeSpawner.CutCube(topCube, bottomCube, cubeOverlap);

        screenShaker.ShakeScreen(seconds: screenShakeSeconds);
        yield return StartCoroutine(cubeMover.FreezeCubeMovement(stackFreezeSeconds));

        // Only spawn next top cube if the stack hasn't reached to top rank yet
        if (cubeStack.StackedCubes.Count >= fullWinScore)
        {
            gameStates.SetCurrentState("full win");
            GameEndEvent?.Invoke();
            stackCoroutine = null;
            yield break;
        }

        cubeSpawner.SpawnStackedCube(spawnPosition: topCube.transform.position, setWidth: topCube.transform.localScale.x);
        stackCoroutine = null;
    }

    public void ResetGame()
    {
        gameStates.SetCurrentState("new game");

        GameResetEvent?.Invoke();

        cubeSpawner.SpawnBottomCube();
    }
}