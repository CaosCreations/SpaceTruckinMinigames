using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private CubeSpawner cubeSpawner;

    [SerializeField] private CubeStack cubeStack;

    [SerializeField] private int maxScore = 3;

    public Action GameResetEvent;

    public Action GameEndEvent;

    public Action GameWinEvent;

    public Action GameLoseEvent;

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
            GameEndEvent?.Invoke();
            GameLoseEvent?.Invoke();
            return;
        }

        // Cubes are stacked
        // we cut part of the top cube so that its side that
        // was sticking out is now aligned with that of the bottom cube
        CutStickingOutTopCubeSide(topCubeCornerPosition, bottomCubeCornerPosition);

        // Only spawn next top cube if the stack hasn't reached to top rank yet
        if (cubeStack.StackedCubes.Count >= maxScore)
        {
            GameEndEvent?.Invoke();
            GameWinEvent?.Invoke();
            return;
        }

        cubeSpawner.SpawnStackedCube(spawnPosition: bottomCubeCornerPosition.transform.position, setWidth: cubeOverlapDistance);
    }

    private void CutStickingOutTopCubeSide(CubeCornersPositionTracker topCubeCornerPosition, CubeCornersPositionTracker bottomCubeCornerPosition)
    {
        float XspawnPosition;

        float width = cubeStack.CubesOverlapDistance();

        // Top cube to the left of bottom cube
        if (bottomCubeCornerPosition.GetLeftCornerPosition() < topCubeCornerPosition.GetLeftCornerPosition())
        {
            XspawnPosition = topCubeCornerPosition.GetLeftCornerPosition() + width / 2;
        }

        // Top cube to the right of bottom cube
        else
        {
            XspawnPosition = bottomCubeCornerPosition.GetLeftCornerPosition() + width / 2;
        }

        // Cutting off the current top cube (moving and resizing it)

        bottomCubeCornerPosition.transform.position = new Vector3(XspawnPosition, bottomCubeCornerPosition.transform.position.y, bottomCubeCornerPosition.transform.position.z);

        bottomCubeCornerPosition.transform.localScale = new Vector3(width, bottomCubeCornerPosition.transform.localScale.y, bottomCubeCornerPosition.transform.localScale.z);
    }

    public void ResetGame()
    {
        GameResetEvent?.Invoke();

        cubeSpawner.SpawnBottomCube();
    }
}