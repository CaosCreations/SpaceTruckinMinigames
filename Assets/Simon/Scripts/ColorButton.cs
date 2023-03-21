using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public AudioClip AudioClip { get; private set; }

    public RectTransform Rectransform { get; private set; }

    [SerializeField] private GameLoopManager gameLoopManager;

    [SerializeField] private ColorButtonData colorButtonData;

    private Button button;

    private RawImage rawImage;

    private void Awake()
    {
        AudioClip = colorButtonData.AudioClip;

        Rectransform = GetComponent<RectTransform>();

        rawImage = GetComponent<RawImage>();
        rawImage.color= colorButtonData.Color;

        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(PlayButton);
    }

    public void PlayButton()
    {
       StartCoroutine(gameLoopManager.SelectColor(this));
    }
}
