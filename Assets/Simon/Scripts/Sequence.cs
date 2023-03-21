using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sequence : MonoBehaviour
{
    public List<ColorButton> ColorButtonSequence { get; private set; }

    [SerializeField] private ColorButton[] allColorButtons;

    private int sequenceIndex = 0;

    public void CreateColorButtonSequence(int sequenceLength)
    {
        if(sequenceLength <= 0)
        {
            Debug.LogError("The length of the sequence should be larger than 0");
        }

        ColorButtonSequence = new List<ColorButton>();

        for(int i = 0; i < sequenceLength; i++)
        {
            ExtendColorButtonSequence();
        }
    }

    public void IterateSequence()
    {
       sequenceIndex++;
    }

    public bool SequenceReachedLastItem => sequenceIndex >= ColorButtonSequence.Count;

    public void ResetSequenceIndex()
    {
        sequenceIndex = 0;
    }

    private ColorButton GetRandomColorButton()
    {
        return allColorButtons[UnityEngine.Random.Range(0, allColorButtons.Length)];
    }

    public void ExtendColorButtonSequence()
    {
        ColorButtonSequence.Add(GetRandomColorButton());
    }

    public bool ColorButtonIsSameAsCurrentSequenceColor(ColorButton colorButton)
    {
        return colorButton == ColorButtonSequence[sequenceIndex];
    }
}
