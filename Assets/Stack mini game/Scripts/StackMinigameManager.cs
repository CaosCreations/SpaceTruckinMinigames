using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackMinigameManager : MonoBehaviour
{
    [SerializeField] private CubeCornersPositionTracker bottomCubeCornerPositions;
    [SerializeField] private CubeCornersPositionTracker topCubeCornerPositions;

    [SerializeField] private GameObject topCubePrefab;

    [SerializeField] private Transform cubeSpawnXStartPosition;


    [SerializeField] private CubeMover cubeMover;

    private int stackedCubesCount = 0;

    private void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            checkStackedCubesPositions();
        }
    }

    
    private void checkStackedCubesPositions()
    {
        // There is only one cube at beginning. The player can place it wherever. There is no stacking involved yet.

        if(stackedCubesCount == 0)
        {
            GameObject newTopCube = (GameObject)spawnCube(spawnPosition: bottomCubeCornerPositions.transform.position + new Vector3(0f, topCubePrefab.transform.localScale.y, 0f),
                                                          cubeWidth: bottomCubeCornerPositions.transform.localScale.x);
            topCubeCornerPositions = newTopCube.GetComponent<CubeCornersPositionTracker>();

            cubeMover.CurrentMovingCube = topCubeCornerPositions.transform;
            stackedCubesCount++;
            return;
        }

        bottomCubeCornerPositions.UpdateCornerPositions();
        topCubeCornerPositions.UpdateCornerPositions();

        // Cubes aren't stacked
        if (topCubeCornerPositions.LeftCornerXPosition > topCubeCornerPositions.RightCornerXPosition ||
            topCubeCornerPositions.RightCornerXPosition < topCubeCornerPositions.LeftCornerXPosition)
        {
            Debug.Log("Cubes are not stacked. Game over");
        }

        // Cubes are stacked

        // If cubes are stacked, it cuts part of the top cube so that its side that
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

            // Cutting off the current top cube

            Vector3 spawnPosition = new Vector3(XspawnPosition, topCubeCornerPositions.transform.position.y, topCubeCornerPositions.transform.position.z);

            GameObject cutTopCube = (GameObject)spawnCube(spawnPosition: spawnPosition, cubeWidth: cubeOverlap);

            Destroy(topCubeCornerPositions.gameObject);

            // Setting up next gameloop
            GameObject newTopCube = (GameObject)spawnCube(spawnPosition: spawnPosition + new Vector3(0f, topCubePrefab.transform.localScale.y, 0f),
                                                          cubeWidth: cutTopCube.transform.localScale.x);

            bottomCubeCornerPositions = cutTopCube.GetComponent<CubeCornersPositionTracker>();
            topCubeCornerPositions = newTopCube.GetComponent<CubeCornersPositionTracker>();

            stackedCubesCount++;

            cubeMover.CurrentMovingCube = newTopCube.transform;
        }
    }

    private GameObject spawnCube(Vector3 spawnPosition, float cubeWidth)
    {
        GameObject spawnedCube =(GameObject)Instantiate(topCubePrefab, spawnPosition, Quaternion.identity);

        spawnedCube.transform.localScale = new Vector3(cubeWidth, spawnedCube.transform.localScale.y, spawnedCube.transform.localScale.z);

        return spawnedCube;
    }


}
