using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tile[][] tileGrid;

    [SerializeField] private TileColorManager tileColorManager;

    [SerializeField] private ObstaclesPosition[][] obstacleLayouts;

    public Action WinEvent;

    public Action LoseEvent;
    public int UntouchedTileCount { get; private set; }

    private void Awake()
    {
        PopulateTileGrid();

        CreateObstaclesLayouts();
    }

    private void Start()
    {
        ResetGrid();
    }

    public void ResetGrid()
    {
        foreach (Tile[] tileColumn in tileGrid)
        {
            foreach(Tile tile in tileColumn)
            {
                tile.TileStatus = TileStatus.untouched;
            }
        }

        ObstaclesPosition[] randomlyPickedLayout = obstacleLayouts[UnityEngine.Random.Range(0, obstacleLayouts.Length)];

        UntouchedTileCount = 25 - randomlyPickedLayout.Length;

        AddingObsctacles(randomlyPickedLayout);

        foreach (Tile[] tileColumn in tileGrid)
        {
            foreach (Tile tile in tileColumn)
            {
                tileColorManager.ChangeTileColorBasedOnStatus(tile);
            }
        }

        ArrangingTilesInAGrid();

        UpdateTileStatus(0, 0);
    }

    private void PopulateTileGrid()
    {
        tileGrid = new Tile[5][];

        Tile[] tiles = FindObjectsOfType<Tile>();

        for (int x = 0, i = 0; x < 5; x++)
        {
            tileGrid[x] = new Tile[5];

            for (int y = 0; y < 5; y++, i++)
            {
                tileGrid[x][y] = tiles[i];
            }
        }
    }

    private void AddingObsctacles(ObstaclesPosition[] obstaclePositionLayout)
    {
        foreach (ObstaclesPosition item in obstaclePositionLayout)
        {
            Tile tile = GetTileAt(item.Xposition, item.Yposition);

            tile.TileStatus = TileStatus.obstacle;
        }
    }

    private void ArrangingTilesInAGrid()
    {
        int currentXPosition = -225;
        int currentYPosition = -225;

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                tileGrid[x][y].GetComponent<RectTransform>().localPosition = new Vector2(currentXPosition, currentYPosition);

                currentYPosition += 100;
            }

            currentYPosition = -225;
            currentXPosition += 100;
        }
    }


    // As a new game starts, we randomly pick an obstacle layout for the map
    // We can create more layout below, by adding them to the obstacleLayouts array
    private void CreateObstaclesLayouts()
    {
        obstacleLayouts = new ObstaclesPosition[3][];

        obstacleLayouts[0] = new ObstaclesPosition[5]
        {                           new ObstaclesPosition (0,2),
                                    new ObstaclesPosition (0,3),
                                    new ObstaclesPosition (0,4),
                                    new ObstaclesPosition (2,3),
                                    new ObstaclesPosition (2,4),
        };

        obstacleLayouts[1] = new ObstaclesPosition[5]
        {                           new ObstaclesPosition (2,2),
                                    new ObstaclesPosition (3,2),
                                    new ObstaclesPosition (0,4),
                                    new ObstaclesPosition (1,4),
                                    new ObstaclesPosition (2,4),
        };

        obstacleLayouts[2] = new ObstaclesPosition[3]
        {                           new ObstaclesPosition (1,2),
                                    new ObstaclesPosition (2,2),
                                    new ObstaclesPosition (3,3),
        };
    }

    // If the player walks on all tiles only once, he or she wins
    // If the player walks on the same tile twice, it's game over
    public void UpdateTileStatus(int Xposition, int Yposition)
    {
        Tile tile = GetTileAt(Xposition, Yposition);

        if (tile.TileStatus == TileStatus.untouched)
        {
            tile.TileStatus = TileStatus.touched;
            UntouchedTileCount--;
        }

        else if (tile.TileStatus == TileStatus.touched)
        {
            tile.TileStatus = TileStatus.touchedTwice;
            LoseEvent();
        }

        tileColorManager.ChangeTileColorBasedOnStatus(tile);

        if (UntouchedTileCount == 0)
            WinEvent();
    }

    public Tile GetTileAt(int Xposition, int Yposition)
    {
        if ((Xposition) > 4 || (Yposition) > 4 || (Xposition) < 0 || (Yposition) < 0)
            return null;
        else
            return tileGrid[Xposition][Yposition];
    }
}
