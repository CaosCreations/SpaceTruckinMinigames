using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sequence : MonoBehaviour
{
    public List<Colors> ColorSequence { get; private set; }

    private int sequenceIndex = 0;
    public void CreateColorSequence(int sequenceLength)
    {
        if(sequenceLength <= 0)
        {
            Debug.LogError("The length of the sequence should be larger than 0");
        }

        ColorSequence = new List<Colors>();

        for(int i = 0; i < sequenceLength; i++)
        {
            ExtendColorSequence();
        }
    }

    public void IterateSequence()
    {
       sequenceIndex++;
    }

    public bool SequenceReachedLastItem()
    {
        return sequenceIndex >= ColorSequence.Count;
    }

    public void ResetSequenceIndex()
    {
        sequenceIndex = 0;
    }

    private Colors getRandomColor()
    {
        return (Colors)UnityEngine.Random.Range(0, 4);
    }

    public void ExtendColorSequence()
    {
        ColorSequence.Add(getRandomColor());
    }

    public bool CompareColorWithCurrentSequenceColor(Colors color)
    {
        return color == ColorSequence[sequenceIndex];
    }
}
