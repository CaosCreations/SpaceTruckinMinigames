using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(RectTransform))]
public class Tile : MonoBehaviour
{
    public TileStatus TileStatus;

    public RawImage TileGraphic { get; private set; }

    public RectTransform RectTransform { get; private set; }

    private void Awake()
    {
        TileGraphic = GetComponent<RawImage>();
        RectTransform = GetComponent<RectTransform>();
    }
}

public enum TileStatus
{
    obstacle,
    touched,
    untouched
}
