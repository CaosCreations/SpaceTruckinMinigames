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


    [Range(2, 20)]
    [SerializeField] private int cubeDivisions = 3;

    private float cubeDivisionWidth;

    private void Awake()
    {
        cubeDivisionWidth = cubePrefab.transform.localScale.x / cubeDivisions;
    }

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

    public void CutCube(CubeCornersPositionTracker topCubeCornerPosition, CubeCornersPositionTracker bottomCubeCornerPosition, CubeOverlap cubeOverlap)
    {
        float topCornerX = 0f;

        float bottomCornerX = 0f;

        if(cubeOverlap == CubeOverlap.Right)
        {
            topCornerX = topCubeCornerPosition.GetLeftCornerPosition();
            bottomCornerX = bottomCubeCornerPosition.GetRightCornerPosition();
        }

        else if(cubeOverlap == CubeOverlap.Left) 
        {
            topCornerX = topCubeCornerPosition.GetRightCornerPosition();
            bottomCornerX = bottomCubeCornerPosition.GetLeftCornerPosition();
        }

        // Cutting off the current top cube (moving and resizing it)

        float roundedCutWidth = GetRoundedCutWidth(topCornerX, bottomCornerX);

        float newXCubePosition = 0f;

        if (cubeOverlap == CubeOverlap.Right)
        {
            newXCubePosition = (bottomCornerX + (bottomCornerX - roundedCutWidth)) / 2;
        }

        else if(cubeOverlap == CubeOverlap.Left)
        {
            newXCubePosition = (bottomCornerX + (bottomCornerX + roundedCutWidth)) / 2;
        }

        topCubeCornerPosition.transform.position = new Vector3(newXCubePosition, topCubeCornerPosition.transform.position.y, topCubeCornerPosition.transform.position.z);
        topCubeCornerPosition.transform.localScale = new Vector3(roundedCutWidth, topCubeCornerPosition.transform.localScale.y, topCubeCornerPosition.transform.localScale.z);

        // Cutting off the current top cube (moving and resizing it)

        //bottomCubeCornerPosition.transform.position = new Vector3(XspawnPosition, bottomCubeCornerPosition.transform.position.y, bottomCubeCornerPosition.transform.position.z);

        //bottomCubeCornerPosition.transform.localScale = new Vector3(width, bottomCubeCornerPosition.transform.localScale.y, bottomCubeCornerPosition.transform.localScale.z);
    }

    // Start from the top corner and go to the bottom corner (where the cut occurs) in increments
    private float GetRoundedCutWidth(float topCornerX, float bottomCornerX)
    {
        float result = 0f;

        float totalDistance = Mathf.Abs(topCornerX - bottomCornerX);

        while(result < totalDistance)
        {
            result += cubeDivisionWidth;
        }

        return result;
    }


}
