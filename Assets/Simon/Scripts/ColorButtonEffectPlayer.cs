using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButtonEffectPlayer : MonoBehaviour
{
    [SerializeField] private Transform buttonHighlight;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }

    private void ToggleButtonHighlight()
    {
        buttonHighlight.gameObject.SetActive(!buttonHighlight.gameObject.activeSelf);
    }

    public IEnumerator PlayButtonAudioAndVisual(ColorButton colorButton)
    {
        ToggleButtonHighlight();

        buttonHighlight.position = colorButton.Rectransform.position;

        audioSource.clip = colorButton.AudioClip;

        audioSource.Play();

        yield return new WaitForSeconds(0.5f);

        ToggleButtonHighlight();

        yield return new WaitForSeconds(0.15f);
    }

    public IEnumerator PlayButtonSequence(List<ColorButton> colorButtons)
    {
        foreach (ColorButton colorButton in colorButtons)
        {
            yield return StartCoroutine(PlayButtonAudioAndVisual(colorButton));
        }
    }
}