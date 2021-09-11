using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButtonEffectPlayer : MonoBehaviour
{
    [SerializeField] private ColorButton greenButton;
    [SerializeField] private ColorButton redButton;
    [SerializeField] private ColorButton yellowButton;
    [SerializeField] private ColorButton blueButton;

    [SerializeField] private Transform buttonHighlight;

    private AudioSource audioSource;

    private Dictionary<Colors, Transform> colorButtonPositions;

    private Dictionary<Colors, AudioClip> colorButtonSounds;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        colorButtonPositions = new Dictionary<Colors, Transform>
        {
            { Colors.Green, greenButton.transform },
            { Colors.Red, redButton.transform },
            { Colors.Yellow, yellowButton.transform },
            { Colors.Blue, blueButton.transform }
        };

        colorButtonSounds = new Dictionary<Colors, AudioClip>
        {
            { Colors.Green, greenButton.AudioClip },
            { Colors.Red, redButton.AudioClip },
            { Colors.Yellow, yellowButton.AudioClip },
            { Colors.Blue, blueButton.AudioClip }
        };

        ToggleButtonHighlight(false);
    }

    private void ToggleButtonHighlight(bool onOff)
    {
        buttonHighlight.gameObject.SetActive(onOff);
    }

    public IEnumerator PlayButton(Colors color)
    {
        ToggleButtonHighlight(onOff: true);

        buttonHighlight.position = colorButtonPositions[color].position;

        audioSource.clip = colorButtonSounds[color];

        audioSource.Play();

        yield return new WaitForSeconds(0.75f);

        ToggleButtonHighlight(onOff: false);
    }

    public IEnumerator PlayButtonSequence(List<Colors> colorList)
    {
        foreach (Colors color in colorList)
        {
            yield return StartCoroutine(PlayButton(color));
        }
    }

}

public enum Colors
{
    Green,
    Red,
    Yellow,
    Blue
}