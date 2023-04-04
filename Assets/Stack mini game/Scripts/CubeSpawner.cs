using System;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;

    [SerializeField] private Transform cubeSpawnStartPosition;

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


    /// <summary>
    /// The cubes are divided in several parts of equal width, and cuts include a set number of complete parts.
    /// Following that logic, the more the cube divisions, the harder the game, because it is harder to get the timing right to stack smaller parts.
    /// On the one hand, it becomes easy for the player to do perfect stacks, on the other hand it sets a minimum width for the cube,
    /// making it easy to stack it.
    /// </summary>

    public void CutCube(CubeCornersPositionTracker topCubeCornerPosition, CubeCornersPositionTracker bottomCubeCornerPosition, CubeOverlap cubeOverlap)
    {
        float topCornerXPosition = GetTopCornerPosition(topCubeCornerPosition, cubeOverlap);

        float bottomCornerXPosition = GetBottomCornerPosition(bottomCubeCornerPosition, cubeOverlap);

        float roundedCutWidth = GetRoundedCutWidth(topCornerXPosition, bottomCornerXPosition);

        ResizeCutCube(topCubeCornerPosition, cubeOverlap, bottomCornerXPosition, roundedCutWidth);
    }

    private float GetTopCornerPosition(CubeCornersPositionTracker topCubeCornerPosition, CubeOverlap cubeOverlap)
    {
        if (cubeOverlap == CubeOverlap.Right)
            return topCubeCornerPosition.GetLeftCornerPosition();

        else
            return topCubeCornerPosition.GetRightCornerPosition();
    }

    private float GetBottomCornerPosition(CubeCornersPositionTracker bottomCubeCornerPosition, CubeOverlap cubeOverlap)
    {
        if (cubeOverlap == CubeOverlap.Right)
            return bottomCubeCornerPosition.GetRightCornerPosition();

        else
            return bottomCubeCornerPosition.GetLeftCornerPosition();
    }

    private float GetRoundedCutWidth(float topCornerX, float bottomCornerX)
    {
        float cutWidth = 0f;

        float distanceBetweenCubes = Mathf.Abs(topCornerX - bottomCornerX);

        while(cutWidth < distanceBetweenCubes)
            cutWidth += cubeDivisionWidth;

        return cutWidth;
    }

    private void ResizeCutCube(CubeCornersPositionTracker topCubeCornerPosition, CubeOverlap cubeOverlap, float bottomCornerXPosition, float roundedCutWidth)
    {
        float newXCubePosition = 0f;

        if (cubeOverlap == CubeOverlap.Right)
            newXCubePosition = (bottomCornerXPosition + (bottomCornerXPosition - roundedCutWidth)) / 2;

        else if (cubeOverlap == CubeOverlap.Left)
            newXCubePosition = (bottomCornerXPosition + (bottomCornerXPosition + roundedCutWidth)) / 2;

        topCubeCornerPosition.transform.position = new Vector3(newXCubePosition, topCubeCornerPosition.transform.position.y, topCubeCornerPosition.transform.position.z);
        topCubeCornerPosition.transform.localScale = new Vector3(roundedCutWidth, topCubeCornerPosition.transform.localScale.y, topCubeCornerPosition.transform.localScale.z);
    }
}
