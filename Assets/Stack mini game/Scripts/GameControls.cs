using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour
{
    [SerializeField] private StackMiniGame_GameplayManager gameplayManager;

    private void Update()
    {
        CheckSpaceBarInput();
    }

    private void CheckSpaceBarInput()
    {
        if(Input.GetKeyDown("space"))
        {
            StartCoroutine(gameplayManager.DoPlayButton());
        }
    }
}
