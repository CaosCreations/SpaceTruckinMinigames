using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    [SerializeField] private RectTransform playerRectTransform;

    [SerializeField] private AnimationCurve playerMovementAnimationCurve;

    [Range(0.1f, 1f)]
    [SerializeField] private float playerMovementLerpDuration;

    private Vector3 playerStartPosition;

    private int playerXGridPosition = 0;

    private int playerYGridPosition = 0;

    private bool canMove = true;

    private int[] defaultInput = new int[] {0, 0};

    private int[] upInput = new int[] { 0, 1 };
    private int[] downInput = new int[] { 0, -1 };
    private int[] leftInput = new int[] { -1, 0 };
    private int[] rightInput = new int[] { 1, 0 };

    private void Awake()
    {
        playerStartPosition = playerRectTransform.localPosition;
        gridManager.GameEventUpdatedEvent += DisablePlayerMovement;
    }

    private void Update()
    {
        if (!canMove)
            return;

        int[] playerInput = GetPlayerInput();

        if (playerInput == defaultInput)
            return;

        StartCoroutine(MoveToTile(Xmovement: playerInput[0], Ymovement: playerInput[1]));
    }

    private int[] GetPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W))
            return upInput;

        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A))
            return leftInput;

        else if (Input.GetKeyDown(KeyCode.S))
            return downInput;

        else if (Input.GetKeyDown(KeyCode.D))
            return rightInput;

        return defaultInput;
    }

    private bool CheckIfTileIsWalkable(int XInput, int YInput)
    {
        Tile desiredTile = gridManager.GetTileAt(playerXGridPosition + XInput, playerYGridPosition + YInput);

        if (desiredTile != null && desiredTile.TileStatus != TileStatus.Obstacle)
        {
            return true;
        }

        return false;
    }

    // The player moves on the grid one tile at a time. Left, right, up or down.
    // We use the grid's X and Y axis to do so

    private IEnumerator MoveToTile(int Xmovement, int Ymovement)
    {
        if (CheckIfTileIsWalkable(XInput: Xmovement, YInput: Ymovement))
        {
            playerXGridPosition += Xmovement;
            playerYGridPosition += Ymovement;

            Vector2 destination = gridManager.GetTileAt(playerXGridPosition, playerYGridPosition).RectTransform.position;
            yield return StartCoroutine(AnimateMovement(destination));

            gridManager.UpdateTileStatus(playerXGridPosition, playerYGridPosition);
        }  
    }

    private IEnumerator AnimateMovement(Vector2 destination)
    {
        canMove = false;

        float elapsedTime = 0f;

        Vector2 startPosition = playerRectTransform.position;

        while (elapsedTime < playerMovementLerpDuration)
        {
            playerRectTransform.position = Vector2.Lerp(startPosition, destination, playerMovementAnimationCurve.Evaluate(elapsedTime / playerMovementLerpDuration));

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        canMove = true;
    }


    private void DisablePlayerMovement(GameState gameState)
    {
        if (gameState.CheckCurrentState(new List<string>() { "full win", "partial win", "lose" }) == true)
            canMove = false;
        else
        {
            canMove = true;
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
