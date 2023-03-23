using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackMiniGame_GameplayManager : MonoBehaviour
{
    [Header("Game feel")]

    [Range(0.0f, 2f)]
    [SerializeField] private float stackFreezeTime;

    [Header("Dependencies")]
    [SerializeField] private CubeSpawner cubeSpawner;

    [SerializeField] private CubeMover cubeMover;

    [SerializeField] private CubeStack cubeStack;

    [SerializeField] private int fullWinScore;

    [SerializeField] private int partialWinScore;

    [SerializeField] private GameState gameStates;

    public GameState GameStates => gameStates;

    public Action GameResetEvent;

    public Action GameEndEvent;


    private void Start()
    {
        ResetGame();
    }

    // When the player presses the play button, he or she attempts to stack the current moving cube on top of the cube below
    // If the cubes are stacked, the game goes on, and we spawn a new cube on top
    // If not it's game over

    public IEnumerator DoPlayButton()
    {
        CubeCornersPositionTracker topCubeCornerPosition = cubeStack.CubeCornersPositionPile.CubeCornersPositionList[0];

        // There is only one cube at the beginning. The player can place it wherever. There is no cutting involved yet.
        if (cubeStack.CubeCornersPositionPile.CubeCornersPositionList.Count == 1)
        {
            cubeSpawner.SpawnStackedCube(spawnPosition: cubeStack.CubeCornersPositionPile.CubeCornersPositionList[0].transform.position,
                                         setWidth: cubeStack.CubeCornersPositionPile.CubeCornersPositionList[0].transform.localScale.x);

            yield break;
        }

        float cubeOverlapDistance = cubeStack.CubesOverlapDistance();
        CubeCornersPositionTracker bottomCubeCornerPosition = cubeStack.CubeCornersPositionPile.CubeCornersPositionList[1];

        // Cubes aren't stacked. It's game over
        if (cubeOverlapDistance == 0f)
        {
            if(cubeStack.StackedCubes.Count >= partialWinScore+1)
                gameStates.SetCurrentState("partial win");
            else
                gameStates.SetCurrentState("lose");

            GameEndEvent?.Invoke();
            yield break;
        }

        // Cubes are stacked
        // we cut part of the top cube so that its side that
        // was sticking out is now aligned with that of the bottom cube
        cubeSpawner.CutCube(topCubeCornerPosition, bottomCubeCornerPosition);

        yield return StartCoroutine(cubeMover.FreezeCubeMovement(stackFreezeTime));

        // Only spawn next top cube if the stack hasn't reached to top rank yet
        if (cubeStack.StackedCubes.Count >= fullWinScore)
        {
            gameStates.SetCurrentState("full win");
            GameEndEvent?.Invoke();
            yield break;
        }

        cubeSpawner.SpawnStackedCube(spawnPosition: bottomCubeCornerPosition.transform.position, setWidth: cubeOverlapDistance);
    }

    public void ResetGame()
    {
        gameStates.SetCurrentState("new game");

        GameResetEvent?.Invoke();

        cubeSpawner.SpawnBottomCube();
    }
}