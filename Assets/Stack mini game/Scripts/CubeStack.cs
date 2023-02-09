using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeStack : MonoBehaviour
{
    [SerializeField] private CubeSpawner cubeSpawner;

    [SerializeField] private GameplayManager stackMinigameManager;

    public CubeCornersPositionPile CubeCornersPositionPile { get; private set; } = new CubeCornersPositionPile();

    public List<GameObject> StackedCubes { get; private set; } = new List<GameObject>();

    private void Awake()
    {
        cubeSpawner.CubeSpawnedEvent += AddTopCubeDataToPile;

        stackMinigameManager.GameResetEvent += Reset;
    }

    public float CubesOverlapDistance()
    {
        float topCubeLeftCornerXposition = CubeCornersPositionPile.CubeCornersPositionList[0].GetLeftCornerPosition();
        float topCubeRightCornerXposition = CubeCornersPositionPile.CubeCornersPositionList[0].GetRightCornerPosition();
        float bottomCubeLeftCornerXposition = CubeCornersPositionPile.CubeCornersPositionList[1].GetLeftCornerPosition();
        float bottomCubeRightCornerXposition = CubeCornersPositionPile.CubeCornersPositionList[1].GetRightCornerPosition();

        // If the corners are this far apart, it can only mean that top and bottom cubes are not stacked on top of each other,
        // so there is no overlap
        if (topCubeLeftCornerXposition > bottomCubeRightCornerXposition ||
            topCubeRightCornerXposition < bottomCubeLeftCornerXposition)
        {
            return 0;
        }

        // The cubes are stacked, so there is some overlap
        else
        {
            float cubeOverlap = CubeCornersPositionPile.CubeCornersPositionList[1].transform.localScale.x
                                - Mathf.Abs(bottomCubeLeftCornerXposition - topCubeLeftCornerXposition);
            return cubeOverlap;
        }
    }

    private void AddTopCubeDataToPile(GameObject cube)
    {
        StackedCubes.Add(cube);
        CubeCornersPositionPile.Add(cube.GetComponent<CubeCornersPositionTracker>());
    }

    private void Reset()
    {
        foreach (GameObject cube in StackedCubes)
        {
            Destroy(cube);
        }

        StackedCubes.Clear();

        CubeCornersPositionPile.ResetPile();
    }
}
