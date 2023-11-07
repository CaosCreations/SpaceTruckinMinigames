using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animator))]
public class ButtonAnimator : MonoBehaviour
{
    [SerializeField] private string animationStateName;

    private void Awake()
    {
        Animator animator = GetComponent<Animator>();

        Button button = GetComponent<Button>();

        button.onClick.AddListener(() => animator.Play(animationStateName));
    }
}
