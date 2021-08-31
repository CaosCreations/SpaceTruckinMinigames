using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sequence : MonoBehaviour
{
    [SerializeField] ButtonPlayer buttonPlayer;

    private int numberOfColors = Enum.GetNames(typeof(Colors)).Length;

    private List<Colors> colorSequence;

    private int sequenceIndex = 0;

    public void CreateColorSequence(int sequenceLength)
    {
        if(sequenceLength <= 0)
        {
            Debug.LogError("The length of the sequence should be larger than 0");
        }

        colorSequence = new List<Colors>();

        for(int i = 0; i < sequenceLength; i++)
        {
            ExtendColorSequence();
        }
    }

    private void IterateSequence()
    {
        if (sequenceIndex < colorSequence.Count - 1)
        {
            sequenceIndex++;
        }

        else
            sequenceIndex = 0;
    }

    private Colors getRandomColor()
    {
        return (Colors)UnityEngine.Random.Range(0, numberOfColors);
    }

    // Extend sequence
    public void ExtendColorSequence()
    {
        colorSequence.Add(getRandomColor());
    }

    public bool CompareColorWithCurrentSequenceColor(Colors color)
    {
        return color == colorSequence[sequenceIndex];
    }

    public IEnumerator PlaySequenceCoroutine()
    {
        foreach (Colors color in colorSequence)
        {
            buttonPlayer.PlayButton(color);
            yield return new WaitForSeconds(1f);
        }
    }
}
