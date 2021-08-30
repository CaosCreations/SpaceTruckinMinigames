using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SequenceBuilder : MonoBehaviour
{
    private int numberOfColors = Enum.GetNames(typeof(Colors)).Length;

    public List<Colors> CreateColorSequence(int sequenceLength)
    {
        if(sequenceLength >= 0)
        {
            Debug.LogError("The length of the sequence should be larger than 0");
        }

        List<Colors> colorList = new List<Colors>();

        for(int i = 0; i < sequenceLength; i++)
        {
            ExtendColorSequence(colorList);
        }

        return colorList;
    }

    private Colors getRandomColor()
    {
        return (Colors)UnityEngine.Random.Range(0, numberOfColors);
    }

    // Extend sequence
    private void ExtendColorSequence(List<Colors> colorList)
    {
        colorList.Add(getRandomColor());
    }
}
