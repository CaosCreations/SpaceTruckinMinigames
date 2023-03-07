using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.Random;

[Serializable]
public class GameState
{
    [SerializeField] private StackMiniGame_StateRegister stateRegister;

    public string CurrentState { get; private set; }

    public void SetCurrentState(string newState)
    {
        if(TryGetState(newState.ToLower().Trim(), out string result) == true)
                CurrentState = result;
    }

    public bool TryGetState(string state, out string result)
    {
        state = state.ToLower().Trim();

        foreach (string item in stateRegister.States) 
        {
            if (item == state)
            {
                result = item;
                return true;
            }
        }

        throw new ArgumentException("The state:" + state + ", you tried to get or set doesn't exist in the register. " +
                                            "Please check the spelling");
    }

}
