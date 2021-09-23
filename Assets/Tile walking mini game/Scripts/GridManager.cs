using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tileGrid;

    private void Start()
    {
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

    private GameObject GetTileAt(int Xposition, int Yposition)
    {
        return tileGrid[Yposition * 5 + Xposition];
    }

}
