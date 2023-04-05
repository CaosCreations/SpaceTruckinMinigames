using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICubeSpawner
{
    void SpawnBottomCube();
    void SpawnStackedCube(Vector3 spawnPosition, float setWidth);
    void CutCube(CubeCornersPositionTracker topCubeCornerPosition, CubeCornersPositionTracker bottomCubeCornerPosition, CubeOverlap cubeOverlap);
}

public class CubeSpawner : MonoBehaviour, ICubeSpawner
{
    [SerializeField] private GameObject cubePrefab;

    [SerializeField] private Transform cubeSpawnStartPosition;

    public Action<GameObject> CubeSpawnedEvent;

    [SerializeField]
    [Range(2, 20)]
    private int cubeDivisions = 3;

    private float cubeDivisionWidth;
    private readonly List<GameObject> cubePool = new();

    private void Awake()
    {
        cubeDivisionWidth = cubePrefab.transform.localScale.x / cubeDivisions;
    }

    public void SpawnBottomCube()
    {
        GameObject spawnedCube = GetCubeFromPool();

        spawnedCube.transform.SetPositionAndRotation(cubeSpawnStartPosition.position, Quaternion.identity);
        spawnedCube.transform.localScale = new Vector3(cubePrefab.transform.localScale.x, spawnedCube.transform.localScale.y, spawnedCube.transform.localScale.z);

        CubeSpawnedEvent.Invoke(spawnedCube);
    }

    public void SpawnStackedCube(Vector3 spawnPosition, float setWidth)
    {
        spawnPosition += new Vector3(0f, cubePrefab.transform.localScale.y, 0f);

        GameObject spawnedCube = GetCubeFromPool();

        spawnedCube.transform.position = spawnPosition;
        spawnedCube.transform.rotation = Quaternion.identity;
        spawnedCube.transform.localScale = new Vector3(setWidth, spawnedCube.transform.localScale.y, spawnedCube.transform.localScale.z);

        CubeSpawnedEvent.Invoke(spawnedCube);
    }

    public void CutCube(CubeCornersPositionTracker topCubeCornerPosition, CubeCornersPositionTracker bottomCubeCornerPosition, CubeOverlap cubeOverlap)
    {
        float topCornerXPosition = GetTopCornerPosition(topCubeCornerPosition, cubeOverlap);

        float bottomCornerXPosition = GetBottomCornerPosition(bottomCubeCornerPosition, cubeOverlap);

        float roundedCutWidth = GetRoundedCutWidth(topCornerXPosition, bottomCornerXPosition);

        ResizeCutCube(topCubeCornerPosition, cubeOverlap, bottomCornerXPosition, roundedCutWidth);
    }

    private float GetTopCornerPosition(CubeCornersPositionTracker topCubeCornerPosition, CubeOverlap cubeOverlap)
    {
        return cubeOverlap == CubeOverlap.Right ? topCubeCornerPosition.GetLeftCornerPosition() : topCubeCornerPosition.GetRightCornerPosition();
    }

    private float GetBottomCornerPosition(CubeCornersPositionTracker bottomCubeCornerPosition, CubeOverlap cubeOverlap)
    {
        return cubeOverlap == CubeOverlap.Right ? bottomCubeCornerPosition.GetRightCornerPosition() : bottomCubeCornerPosition.GetLeftCornerPosition();
    }

    private float GetRoundedCutWidth(float topCornerX, float bottomCornerX)
    {
        float cutWidth = 0f;

        float distanceBetweenCubes = Mathf.Abs(topCornerX - bottomCornerX);

        while (cutWidth < distanceBetweenCubes)
        {
            cutWidth += cubeDivisionWidth;
        }

        return cutWidth;
    }

    private void ResizeCutCube(CubeCornersPositionTracker topCubeCornerPosition, CubeOverlap cubeOverlap, float bottomCornerXPosition, float roundedCutWidth)
    {
        float newXCubePosition = cubeOverlap == CubeOverlap.Right ? (bottomCornerXPosition + (bottomCornerXPosition - roundedCutWidth)) / 2 : (bottomCornerXPosition + (bottomCornerXPosition + roundedCutWidth)) / 2;

        topCubeCornerPosition.transform.position = new Vector3(newXCubePosition, topCubeCornerPosition.transform.position.y, topCubeCornerPosition.transform.position.z);
        topCubeCornerPosition.transform.localScale = new Vector3(roundedCutWidth, topCubeCornerPosition.transform.localScale.y, topCubeCornerPosition.transform.localScale.z);
    }

    private GameObject GetCubeFromPool()
    {
        GameObject cube = cubePool.Find(c => !c.activeSelf);

        if (cube == null)
        {
            cube = Instantiate(cubePrefab, Vector3.zero, Quaternion.identity, transform);
            cubePool.Add(cube);
        }

        cube.SetActive(true);

        return cube;
    }
}
