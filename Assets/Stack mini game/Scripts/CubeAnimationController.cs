using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CubeAnimationController : MonoBehaviour
{
    private const string normalSpeedAnimationState = "NormalSpeed";
    private const string slowSpeedAnimationState = "SlowSpeed";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetNormalSpeedAnimationState();
    }

    public void SetNormalSpeedAnimationState()
    {
        SetAnimationState(normalSpeedAnimationState);
    }

    public void SetSlowSpeedAnimateState()
    {
        SetAnimationState(slowSpeedAnimationState);
    }

    private void SetAnimationState(string stateName)
    {
        animator.Play(stateName);
    }
}
