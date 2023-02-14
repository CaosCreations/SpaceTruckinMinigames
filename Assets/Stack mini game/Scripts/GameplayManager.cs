using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private CubeSpawner cubeSpawner;

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

    public void DoPlayButton()
    {
        CubeCornersPositionTracker topCubeCornerPosition = cubeStack.CubeCornersPositionPile.CubeCornersPositionList[0];

        // There is only one cube at the beginning. The player can place it wherever. There is no cutting involved yet.
        if (cubeStack.CubeCornersPositionPile.CubeCornersPositionList.Count == 1)
        {
            cubeSpawner.SpawnStackedCube(spawnPosition: cubeStack.CubeCornersPositionPile.CubeCornersPositionList[0].transform.position,
                                         setWidth: cubeStack.CubeCornersPositionPile.CubeCornersPositionList[0].transform.localScale.x);

            return;
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
            return;
        }

        // Cubes are stacked
        // we cut part of the top cube so that its side that
        // was sticking out is now aligned with that of the bottom cube
        cubeSpawner.CutCube(topCubeCornerPosition, bottomCubeCornerPosition);

        // Only spawn next top cube if the stack hasn't reached to top rank yet
        if (cubeStack.StackedCubes.Count >= fullWinScore)
        {
            gameStates.SetCurrentState("full win");
            GameEndEvent?.Invoke();
            return;
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