using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCornersPositionTracker : MonoBehaviour
{
    public float LeftCornerXPosition { get; private set; }
    public float RightCornerXPosition { get; private set; }

    public void UpdateCornerPositions()
    {
        LeftCornerXPosition = transform.position.x - transform.localScale.x / 2;
        RightCornerXPosition = transform.position.x + transform.localScale.x / 2;
    }
}
