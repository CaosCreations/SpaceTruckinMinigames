using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tile[][] tileGrid;

    [SerializeField] private TileColorManager tileColorManager;

    [SerializeField] private ObstaclesPosition[][] obstacleLayouts;

    public int UntouchedTileCount { get; private set; } = 20;

    private void Awake()
    {
        tileGrid = new Tile[5][];

        Tile[] tiles = FindObjectsOfType<Tile>();

        for(int x = 0 , i = 0; x < 5; x++)
        {
            tileGrid[x] = new Tile[5];

            for (int y = 0; y < 5; y++, i++)
            {
                tileGrid[x][y] = tiles[i];
            }
        }

        CreateObstaclesLayouts();
    }

    private void Start()
    {
        ResetGrid();
    }

    public void ResetGrid()
    {
        UntouchedTileCount = 20;

        foreach (Tile[] tileColumn in tileGrid)
        {
            foreach(Tile tile in tileColumn)
            {
                tile.TileStatus = TileStatus.untouched;
            }
        }

        RandomlyAddingObsctacles();

        foreach (Tile[] tileColumn in tileGrid)
        {
            foreach (Tile tile in tileColumn)
            {
                tileColorManager.ChangeTileColorBasedOnStatus(tile);
            }
        }

        ArrangingTilesInAGrid();

        WalkOnTile(0, 0);
    }

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
        {                           new ObstaclesPosition (0,4),
                                    new ObstaclesPosition (3,1),
                                    new ObstaclesPosition (3,2),
                                    new ObstaclesPosition (2,2),
                                    new ObstaclesPosition (2,3),
        };
    }

    private void ArrangingTilesInAGrid()
    {
        int currentXPosition = -225;
        int currentYPosition = -225;

        for(int x = 0; x < 5; x++)
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

    private void RandomlyAddingObsctacles()
    {
        ObstaclesPosition[] randomlyPickedLayout = obstacleLayouts[Random.Range(0, obstacleLayouts.Length)];

        foreach(ObstaclesPosition item in randomlyPickedLayout)
        {
            Tile tile = GetTileAt(item.Xposition, item.Yposition);

            tile.TileStatus = TileStatus.obstacle;
        }
    }

    public Tile GetTileAt(int Xposition, int Yposition)
    {
        if ((Xposition) > 4 || (Yposition) > 4 || (Xposition) < 0 || (Yposition) < 0)
            return null;
        else
            return tileGrid[Xposition][Yposition];
    }
    
    public void WalkOnTile(int Xposition, int Yposition)
    {
        Tile tile = GetTileAt(Xposition, Yposition);

        if(tile.TileStatus == TileStatus.untouched)
        {
            tile.TileStatus = TileStatus.touched;
            UntouchedTileCount--;
            Debug.Log("UntouchedTileCount = " + UntouchedTileCount);
        }

        else if(tile.TileStatus == TileStatus.touched)
        {
            tile.TileStatus = TileStatus.touchedTwice;
        }

        tileColorManager.ChangeTileColorBasedOnStatus(tile);
    }
}
