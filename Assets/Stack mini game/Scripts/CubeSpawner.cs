using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;

    [SerializeField] private Transform cubeSpawnStartPosition;

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
}
