using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    private Sequence sequence = new Sequence();

    private bool lastSequenceItemReached = false;

    private bool SequenceAndButtonColorSame = true;

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        sequence.CreateColorSequence(3);
        yield return StartCoroutine(sequence.PlaySequenceCoroutine());

        while(lastSequenceItemReached == false && SequenceAndButtonColorSame == true)
        {
            yield return null;
        }

        if(lastSequenceItemReached == true)
        {
            sequence.ExtendColorSequence();
        }
    }



    private void ResetGameLoop()
    {
        lastSequenceItemReached = false;

        SequenceAndButtonColorSame = true;
    }

    

    


}
