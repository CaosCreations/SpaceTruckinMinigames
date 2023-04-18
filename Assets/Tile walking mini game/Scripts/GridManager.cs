using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Tile[] tileGrid;

    [SerializeField] private Transform tileContainer;

    [SerializeField] private TileColorManager tileColorManager;

    private ObstaclesPosition[][] obstacleLayouts;

    [SerializeField] private GameObject TilePrefab;

    public Action WinEvent;

    public Action LoseEvent;

    public Action<Tile> TileStatusChangedEvent;

    public int touchedTiles_Percent { get; private set; } = 0;

    public int GridWidth { get; private set; } = 5;
    public int GridHeight { get; private set; } = 5;

    [SerializeField] private GameState currentGameState;

    [Range(20, 95)]
    [SerializeField] private int partialWinThreshold_Percent = 75;

    private void Awake()
    {
        CreateObstaclesLayouts();
        SpawnTiles();
    }

    private void Start()
    {
        ResetGrid();
    }

    public void ResetGrid()
    {
        currentGameState.SetCurrentState("new game");
        touchedTiles_Percent = 0;

        foreach (Tile tile in tileGrid)
        {
            tile.TileStatus = TileStatus.Untouched;
        }

        AddingObsctacles();

        foreach (Tile tile in tileGrid)
        {
            tileColorManager.ChangeTileColorBasedOnStatus(tile);
        }

        UpdateTileStatus(0, 0);
    }

    private void AddingObsctacles()
    {
        ObstaclesPosition[] randomlyPickedLayout = obstacleLayouts[UnityEngine.Random.Range(0, obstacleLayouts.Length)];

        foreach (ObstaclesPosition item in randomlyPickedLayout)
        {
            Tile tile = GetTileAt(item.Xposition, item.Yposition);

            tile.TileStatus = TileStatus.Obstacle;
        }
    }

    // As a new game starts, we randomly pick an obstacle layout for the map
    // We can create more layout below, by adding them to the obstacleLayouts array
    private void CreateObstaclesLayouts()
    {
        obstacleLayouts = new ObstaclesPosition[2][];

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
    }

    private void SpawnTiles()
    {
        int currentXPosition = -225;
        int currentYPosition = -225;

        tileGrid = new Tile[GridWidth * GridHeight];

        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                tileGrid[x + y * GridWidth] = Instantiate(TilePrefab, tileContainer).GetComponent<Tile>();
                tileGrid[x + y * GridWidth].gameObject.GetComponent<RectTransform>().localPosition = new Vector2(currentXPosition, currentYPosition);

                currentYPosition += 100;
            }

            currentYPosition = -225;
            currentXPosition += 100;
        }
    }

    // If the player walks on all tiles only once, he or she wins
    // If the player walks on the same tile twice, it's game over
    public void UpdateTileStatus(int Xposition, int Yposition)
    {
        Tile tile = GetTileAt(Xposition, Yposition);

        if (tile.TileStatus == TileStatus.Untouched)
        {
            tile.TileStatus = TileStatus.Touched;
            touchedTiles_Percent += 5;
        }

        else if (tile.TileStatus == TileStatus.Touched)
        {
            tile.TileStatus = TileStatus.TouchedTwice;
            currentGameState.SetCurrentState("lose");
            LoseEvent();
        }

        tileColorManager.ChangeTileColorBasedOnStatus(tile);

        if (touchedTiles_Percent == 100)
        {
            currentGameState.SetCurrentState("full win");
            WinEvent();
        }

        else if(touchedTiles_Percent >= partialWinThreshold_Percent)
        {
            currentGameState.SetCurrentState("partial win");
        } 

        TileStatusChangedEvent?.Invoke(tile);
    }

    public Tile GetTileAt(int Xposition, int Yposition)
    {
        if (Xposition > GridWidth - 1 || Yposition > GridHeight - 1 || Xposition < 0 || Yposition < 0)
            return null;

        return tileGrid[Xposition + Yposition * GridWidth];
    }
}
