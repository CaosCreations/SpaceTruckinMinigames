using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Tile[] tileGrid;

    [SerializeField] private TileColorManager tileColorManager;

    private void Awake()
    {
        tileGrid = FindObjectsOfType<Tile>();
    }

    private void Start()
    {
        ResetGrid();
    }

    private void ResetGrid()
    {
        foreach (Tile tile in tileGrid)
        {
            tile.TileStatus = TileStatus.untouched;
        }

        RandomlyAddingObsctacles(obstaclesNumber: 5);

        foreach (Tile tile in tileGrid)
        {
            tileColorManager.ChangeTileColorBasedOnStatus(tile);
        }

        ArrangingTilesInAGrid();
    }


    private void ArrangingTilesInAGrid()
    {
        int currentXPosition = 0;
        int currentYPosition = 0;

        for(int y = 0, i = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++, i++)
            {
                tileGrid[i].GetComponent<RectTransform>().localPosition = new Vector2(currentXPosition, currentYPosition);

                currentXPosition += 100; 
                
            }
            currentXPosition = 0;
            currentYPosition += 100;
        }
    }

    private void RandomlyAddingObsctacles(int obstaclesNumber)
    {
        HashSet<int> randomTileIndices = TileWalkingUtils.GetRandomNonRepeatingNumbers(outputSize: obstaclesNumber,
                                                                                       minNumber: 0,
                                                                      maxNumber: tileGrid.Length - 1);

        foreach (int index in randomTileIndices)
        {
            tileGrid[index].TileStatus = TileStatus.obstacle;
        }
    }

    private Tile GetTileAt(int Xposition, int Yposition)
    {
        return tileGrid[Yposition * 5 + Xposition];
    }

}
