using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.Random;

[Serializable]
public class GameState
{
    [SerializeField] private string[] states;

    public string CurrentState { get; private set; }

    public void SetCurrentState(string newState)
    {
        string result;

        if(TryGetState(newState.ToLower().Trim(), out result) == true)
                CurrentState = result;
    }

    public bool TryGetState(string state, out string result)
    {
        state.ToLower().Trim();

        foreach (string item in states) 
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
