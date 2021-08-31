using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPlayer : MonoBehaviour
{
    [SerializeField] private Transform greenButton;
    [SerializeField] private Transform redButton;
    [SerializeField] private Transform yellowButton;
    [SerializeField] private Transform blueButton;
    [SerializeField] private Transform buttonHighlight;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip greenSound;
    [SerializeField] private AudioClip redSound;
    [SerializeField] private AudioClip yellowSound;
    [SerializeField] private AudioClip blueSound;

    private Dictionary<Colors, Transform> colorButtonPositions;

    private Dictionary<Colors, AudioClip> colorButtonSounds;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        colorButtonPositions = new Dictionary<Colors, Transform>
        {
            { Colors.Green, greenButton },
            { Colors.Red, redButton },
            { Colors.Yellow, yellowButton },
            { Colors.Blue, blueButton }
        };

        colorButtonSounds = new Dictionary<Colors, AudioClip>
        {
            { Colors.Green, greenSound },
            { Colors.Red, redSound },
            { Colors.Yellow, yellowSound },
            { Colors.Blue, blueSound }
        };
    }

    public void PlayButton(Colors color)
    {
        buttonHighlight.position = colorButtonPositions[color].position;

        audioSource.clip = colorButtonSounds[color];

        audioSource.Play();
    }



}
