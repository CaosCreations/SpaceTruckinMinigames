using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;

    [SerializeField] private Transform cubeSpawnStartPosition;

    [SerializeField] private CubeStack cubeStack;

    public Action<GameObject> CubeSpawnedEvent;

    public void SpawnBottomCube()
    {
        GameObject spawnedCube = Instantiate(cubePrefab, cubeSpawnStartPosition.position, Quaternion.identity);

        spawnedCube.transform.localScale = new Vector3(cubePrefab.transform.localScale.x, spawnedCube.transform.localScale.y, spawnedCube.transform.localScale.z);

        CubeSpawnedEvent.Invoke(spawnedCube);
    }

    public void SpawnStackedCube(Vector3 spawnPosition, float setWidth)
    {
        spawnPosition += new Vector3(0f, cubePrefab.transform.localScale.y, 0f);

        GameObject spawnedCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);

        spawnedCube.transform.localScale = new Vector3(setWidth, spawnedCube.transform.localScale.y, spawnedCube.transform.localScale.z);

        CubeSpawnedEvent.Invoke(spawnedCube);
    }

    public void CutCube(CubeCornersPositionTracker topCubeCornerPosition, CubeCornersPositionTracker bottomCubeCornerPosition)
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
}
