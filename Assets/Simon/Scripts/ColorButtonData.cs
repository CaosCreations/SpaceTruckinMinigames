using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color button data", menuName = "ScriptableObjects/Simon/Color button data", order = 1)]
public class ColorButtonData : ScriptableObject
{
    [SerializeField] private Color color;

    public Color Color => color;

    [SerializeField] private AudioClip audioClip;

    public AudioClip AudioClip => audioClip;
}
