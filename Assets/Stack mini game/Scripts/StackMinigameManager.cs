using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackMinigameManager : MonoBehaviour
{
    [SerializeField] private int maxScore;

    private CubeCornersPositionPile cubeCornersPositionPile = new CubeCornersPositionPile();

    [SerializeField] private GameObject cubePrefab;

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
        if(cubeCornersPositionPile.CubeCornersPositionList.Count == 1)
        {

            SpawnTopCube(spawnPosition: cubeCornersPositionPile.CubeCornersPositionList[0].transform.position + new Vector3(0f, cubePrefab.transform.localScale.y, 0f),
                                                          cubeWidth: cubeCornersPositionPile.CubeCornersPositionList[0].transform.localScale.x);

            return;
        }

        cubeCornersPositionPile.CubeCornersPositionList[0].UpdateCornerPositions();
        cubeCornersPositionPile.CubeCornersPositionList[1].UpdateCornerPositions();

        // Cubes aren't stacked
        if (cubeCornersPositionPile.CubeCornersPositionList[1].LeftCornerXPosition > cubeCornersPositionPile.CubeCornersPositionList[0].RightCornerXPosition ||
            cubeCornersPositionPile.CubeCornersPositionList[1].RightCornerXPosition < cubeCornersPositionPile.CubeCornersPositionList[0].LeftCornerXPosition)
        {
            Debug.Log("Cubes are not stacked. Game over");
            gameRunning = false;
            stackMinigameUI.SetGameUI(GameState.Lose);
            return;
        }

        // Cubes are stacked

        // If cubes are stacked, we cut part of the top cube so that its side that
        // was sticking out is now aligned with that of the bottom cube
        // If the cube are not stacked, then it's game over.
        else
        {
            float cubeOverlap = cubeCornersPositionPile.CubeCornersPositionList[0].transform.localScale.x
                                - Mathf.Abs(cubeCornersPositionPile.CubeCornersPositionList[0].LeftCornerXPosition - cubeCornersPositionPile.CubeCornersPositionList[1].LeftCornerXPosition);


            float XspawnPosition;

            // Top cube to the left of bottom cube
            if (cubeCornersPositionPile.CubeCornersPositionList[1].LeftCornerXPosition < cubeCornersPositionPile.CubeCornersPositionList[0].LeftCornerXPosition)
            {
                XspawnPosition = cubeCornersPositionPile.CubeCornersPositionList[0].LeftCornerXPosition + cubeOverlap/2;
            }

            // Top cube to the right of bottom cube
            else
            {
                XspawnPosition = cubeCornersPositionPile.CubeCornersPositionList[1].LeftCornerXPosition + cubeOverlap/2;
            }

            // Cutting off the current top cube (moving and resizing it)

            Vector3 newCutTopCubePosition = new Vector3(XspawnPosition, cubeCornersPositionPile.CubeCornersPositionList[1].transform.position.y, cubeCornersPositionPile.CubeCornersPositionList[1].transform.position.z);

            cubeCornersPositionPile.CubeCornersPositionList[1].transform.position = newCutTopCubePosition;

            cubeCornersPositionPile.CubeCornersPositionList[1].transform.localScale = new Vector3(cubeOverlap, cubeCornersPositionPile.CubeCornersPositionList[1].transform.localScale.y, cubeCornersPositionPile.CubeCornersPositionList[1].transform.localScale.z);

            // Only spawn next top cube if the stack hasn't reached to top rank yet
            if(stackedCubes.Count >= maxScore)
            {
                gameRunning = false;
                stackMinigameUI.SetGameUI(GameState.Win);
                return;
            }

            SpawnTopCube(spawnPosition: newCutTopCubePosition + new Vector3(0f, cubePrefab.transform.localScale.y, 0f),
                                                              cubeWidth: cubeOverlap);
        }
    }


    private void SpawnTopCube(Vector3 spawnPosition, float cubeWidth)
    {
        GameObject topcube = (GameObject)Instantiate(cubePrefab, spawnPosition, Quaternion.identity);

        topcube.transform.localScale = new Vector3(cubeWidth, topcube.transform.localScale.y, topcube.transform.localScale.z);

        cubeMover.CurrentMovingCube = topcube.transform;
        stackedCubes.Add(topcube);
        cubeCornersPositionPile.Add(topcube.GetComponent<CubeCornersPositionTracker>());
    }

    public void ResetGame()
    {
        foreach(GameObject cube in stackedCubes)
        {
            Destroy(cube);
        }

        stackedCubes.Clear();

        cubeCornersPositionPile.ResetPile();


        SpawnTopCube(cubeSpawnStartPosition.position, cubePrefab.transform.localScale.x);


        gameRunning = true;

        stackMinigameUI.SetGameUI(GameState.NewGame);
    }



}
