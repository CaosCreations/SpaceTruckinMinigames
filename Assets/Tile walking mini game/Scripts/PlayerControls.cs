using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    [SerializeField] private RectTransform playerRectTransform;

    private Vector3 playerStartPosition;

    [SerializeField] private TileWalkingUI tileWalkingUI;

    private int playerXGridPosition = 0;

    private int playerYGridPosition = 0;

    private bool canMove = true;


    private void Awake()
    {
        playerStartPosition = playerRectTransform.localPosition;
    }


    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        if (canMove == false)
            return;

        if (Input.GetKeyDown(KeyCode.Z))
            MovePlayer(Xmovement: 0, Ymovement: 1);

        else if (Input.GetKeyDown(KeyCode.Q))
            MovePlayer(Xmovement: -1, Ymovement: 0);

        else if (Input.GetKeyDown(KeyCode.S))
            MovePlayer(Xmovement: 0, Ymovement: -1);

        else if (Input.GetKeyDown(KeyCode.D))
            MovePlayer(Xmovement: 1, Ymovement: 0);
    }

    private void MovePlayer(int Xmovement, int Ymovement)
    {
        Tile desiredTile = gridManager.GetTileAt(playerXGridPosition + Xmovement, playerYGridPosition + Ymovement);

        if(desiredTile == null || desiredTile.TileStatus == TileStatus.obstacle)
        {
            return;
        }

        if(desiredTile.TileStatus == TileStatus.touched)
        {
            tileWalkingUI.ToggleGameOverUI();
            canMove = false;
        }

        playerRectTransform.position = desiredTile.RectTransform.position;

        playerXGridPosition += Xmovement;
        playerYGridPosition += Ymovement;


        /// What lies below could be moved to a different function

        gridManager.WalkOnTile(playerXGridPosition, playerYGridPosition);

        if(gridManager.UntouchedTileCount == 0)
        {
            Debug.Log("Toggle win UI");
            tileWalkingUI.ToggleWinUI();
            canMove = false;
        }
    }


    public void ResetPlayerMovement()
    {
        playerRectTransform.localPosition = playerStartPosition;
        playerXGridPosition = 0;
        playerYGridPosition = 0;
        canMove = true;
    }
}
