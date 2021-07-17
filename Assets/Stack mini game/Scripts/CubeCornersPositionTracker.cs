using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCornersPositionTracker : MonoBehaviour
{
    [SerializeField] private float leftCornerXPosition;
    [SerializeField] private float rightCornerXPosition;

    private void updateCornerPositions()
    {
        leftCornerXPosition = transform.position.x - transform.localScale.x / 2;
        rightCornerXPosition = transform.position.x + transform.localScale.x / 2;
    }
}
