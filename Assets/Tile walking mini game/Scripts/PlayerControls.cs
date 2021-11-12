using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    [SerializeField] private RectTransform playerRectTransform;

    [SerializeField] private TileWalkingUI tileWalkingUI;

    private Vector3 playerStartPosition;

    private int playerXGridPosition = 0;

    private int playerYGridPosition = 0;

    private bool canMove = true;

    private int[] defaultInput = new int[] {0, 0};

    private Dictionary<KeyCode, int[]> inputDictionary = new Dictionary<KeyCode, int[]>()
    {
        { KeyCode.Z, new int[]{ 0, 1} },
        { KeyCode.W, new int[]{ 0, 1} },
        { KeyCode.A, new int[]{ -1, 0} },
        { KeyCode.Q, new int[]{ -1, 0} },
        { KeyCode.S, new int[]{ 0, -1} },
        { KeyCode.D, new int[]{ 1, 0} },
    };

    private void Awake()
    {
        playerStartPosition = playerRectTransform.localPosition;
        gridManager.WinEvent += DisablePlayerMovement;
        gridManager.LoseEvent += DisablePlayerMovement;
    }

    private void OnGUI()
    {
        if (canMove == false)
            return;

        int[] playerInput = GetPlayerInput();

        if (playerInput[0] == 0 && playerInput[1] == 0)
            return;

        if (CheckIfTileIsWalkable(XInput: playerInput[0], YInput: playerInput[1]) == false)
            return;

        MovePlayerToTile(Xmovement: playerInput[0], Ymovement: playerInput[1]);

        gridManager.UpdateTileStatus(playerXGridPosition, playerYGridPosition);
    }

    private int[] GetPlayerInput()
    {
        Event currentEvent = Event.current;

        if (currentEvent != null && 
            currentEvent.isKey == true && 
            currentEvent.type == EventType.KeyDown == true &&
            inputDictionary.ContainsKey(currentEvent.keyCode) ==  true
            )
        {
            return inputDictionary[currentEvent.keyCode];
        }

        return defaultInput;
    }

    private bool CheckIfTileIsWalkable(int XInput, int YInput)
    {
        Tile desiredTile = gridManager.GetTileAt(playerXGridPosition + XInput, playerYGridPosition + YInput);

        if (desiredTile == null || desiredTile.TileStatus == TileStatus.obstacle)
        {
            return false;
        }

        return true;
    }

    // The player moves on the grid one tile at a time. Left, right, up or down.
    // We use the grid's X and Y axis to do so

    private void MovePlayerToTile(int Xmovement, int Ymovement)
    {
        playerXGridPosition += Xmovement;
        playerYGridPosition += Ymovement;

        playerRectTransform.position = gridManager.GetTileAt(playerXGridPosition, playerYGridPosition).RectTransform.position;
    }

    private void DisablePlayerMovement()
    {
        canMove = false;
    }

    public void ResetPlayerMovement()
    {
        playerRectTransform.localPosition = playerStartPosition;
        playerXGridPosition = 0;
        playerYGridPosition = 0;
        canMove = true;
    }
}
