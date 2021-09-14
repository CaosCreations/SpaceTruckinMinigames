using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    [SerializeField] private Colors color;

    [field: SerializeField]
    public AudioClip AudioClip { get; private set; }

    private Button button;

    private GameLoopManager gameLoopManager;
    private void Awake()
    {
        gameLoopManager = GameObject.FindObjectOfType<GameLoopManager>();
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(PlayButton);
    }

    public void PlayButton()
    {
       StartCoroutine(gameLoopManager.SelectColor(color));
    }
}
