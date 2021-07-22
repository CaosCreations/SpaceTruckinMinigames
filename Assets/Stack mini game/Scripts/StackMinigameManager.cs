using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackMinigameManager : MonoBehaviour
{
    [SerializeField] private int maxScore;

    [SerializeField] private CubeCornersPositionTracker bottomCubeCornerPositions;
    [SerializeField] private CubeCornersPositionTracker topCubeCornerPositions;

    [SerializeField] private GameObject topCubePrefab;

    [SerializeField] private Transform cubeSpawnStartPosition;

    [SerializeField] private CubeMover cubeMover;

    [SerializeField] private StackMinigameUI stackMinigameUI;



    private List<GameObject> stackedCubes { get; set; } = new List<GameObject>(); 

    private bool gameRunning = false;

    private void Update()
    {
        if (gameRunning == false)
            return;

        cubeMover.MoveCube();
    }

    private void OnEnable()
    {
        ResetGame();
    }


    public void StackCube()
    {
        // There is only one cube at the beginning. The player can place it wherever. There is no cutting involved yet.
        if(stackedCubes.Count == 1)
        {
            GameObject newTopCube = (GameObject)spawnCube(spawnPosition: bottomCubeCornerPositions.transform.position + new Vector3(0f, topCubePrefab.transform.localScale.y, 0f),
                                                          cubeWidth: bottomCubeCornerPositions.transform.localScale.x);
            topCubeCornerPositions = newTopCube.GetComponent<CubeCornersPositionTracker>();

            cubeMover.CurrentMovingCube = topCubeCornerPositions.transform;

            stackedCubes.Add(newTopCube);

            return;
        }

        bottomCubeCornerPositions.UpdateCornerPositions();
        topCubeCornerPositions.UpdateCornerPositions();

        // Cubes aren't stacked
        if (topCubeCornerPositions.LeftCornerXPosition > bottomCubeCornerPositions.RightCornerXPosition ||
            topCubeCornerPositions.RightCornerXPosition < bottomCubeCornerPositions.LeftCornerXPosition)
        {
            Debug.Log("Cubes are not stacked. Game over");
            gameRunning = false;
            stackMinigameUI.SetLoseUI();
            return;
        }

        // Cubes are stacked

        // If cubes are stacked, we cut part of the top cube so that its side that
        // was sticking out is now aligned with that of the bottom cube
        // If the cube are not stacked, then it's game over.
        else
        {
            float cubeOverlap;
            float XspawnPosition;

            // Top cube to the left of bottom cube
            if (topCubeCornerPositions.LeftCornerXPosition < bottomCubeCornerPositions.LeftCornerXPosition)
            {
                cubeOverlap = Mathf.Abs(bottomCubeCornerPositions.LeftCornerXPosition - topCubeCornerPositions.RightCornerXPosition);

                XspawnPosition = bottomCubeCornerPositions.LeftCornerXPosition + cubeOverlap/2;
            }

            // Top cube to the right of bottom cube
            else
            {
                cubeOverlap = Mathf.Abs(bottomCubeCornerPositions.RightCornerXPosition - topCubeCornerPositions.LeftCornerXPosition);

                XspawnPosition = bottomCubeCornerPositions.RightCornerXPosition - cubeOverlap / 2;
            }

            // Cutting off the current top cube (moving and resizing it)

            Vector3 newCutTopCubePosition = new Vector3(XspawnPosition, topCubeCornerPositions.transform.position.y, topCubeCornerPositions.transform.position.z);

            topCubeCornerPositions.transform.position = newCutTopCubePosition;

            topCubeCornerPositions.transform.localScale = new Vector3(cubeOverlap, topCubeCornerPositions.transform.localScale.y, topCubeCornerPositions.transform.localScale.z);

            // Only spawn next top cube if the stack hasn't reached to top rank yet
            if(stackedCubes.Count >= maxScore)
            {
                gameRunning = false;
                stackMinigameUI.SetWinUI();
            }

            else
            {
                GameObject newTopCube = (GameObject)spawnCube(spawnPosition: newCutTopCubePosition + new Vector3(0f, topCubePrefab.transform.localScale.y, 0f),
                                                              cubeWidth: cubeOverlap);

                stackedCubes.Add(newTopCube);

                bottomCubeCornerPositions = topCubeCornerPositions.GetComponent<CubeCornersPositionTracker>();
                topCubeCornerPositions = newTopCube.GetComponent<CubeCornersPositionTracker>();

                cubeMover.CurrentMovingCube = newTopCube.transform;
            }
        }
    }

    private GameObject spawnCube(Vector3 spawnPosition, float cubeWidth)
    {
        GameObject spawnedCube =(GameObject)Instantiate(topCubePrefab, spawnPosition, Quaternion.identity);

        spawnedCube.transform.localScale = new Vector3(cubeWidth, spawnedCube.transform.localScale.y, spawnedCube.transform.localScale.z);

        return spawnedCube;
    }

    public void ResetGame()
    {
        foreach(GameObject cube in stackedCubes)
        {
            Destroy(cube);
        }

        stackedCubes.Clear();


        GameObject newBottomCube = (GameObject)spawnCube(cubeSpawnStartPosition.position, topCubePrefab.transform.localScale.x);

        stackedCubes.Add(newBottomCube);

        cubeMover.CurrentMovingCube = newBottomCube.transform;
        
        topCubeCornerPositions = null;

        bottomCubeCornerPositions = newBottomCube.GetComponent<CubeCornersPositionTracker>();


        gameRunning = true;

        stackMinigameUI.SetNewGameUI();
    }



}
