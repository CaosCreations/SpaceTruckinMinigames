using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField] private Transform greenButton;
    [SerializeField] private Transform redButton;
    [SerializeField] private Transform yellowButton;
    [SerializeField] private Transform blueButton;
    [SerializeField] private Transform buttonHighligth;

    private Dictionary<Colors, Transform> colorButtonPositions;

    private void Awake()
    {
        colorButtonPositions = new Dictionary<Colors, Transform>
        {
            { Colors.Green, greenButton },
            { Colors.Red, redButton },
            { Colors.Yellow, yellowButton },
            { Colors.Blue, blueButton }
        };
    }



}
