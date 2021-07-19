using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    public Transform CurrentMovingCube;

    [SerializeField] private float movingSpeed;

    [SerializeField] private ChangeMovingCubeDirectionCollider leftChangeDirectionCollider;

    [SerializeField] private ChangeMovingCubeDirectionCollider rightChangeDirectionCollider;

    private float direction = 1;

    private void Awake()
    {
        leftChangeDirectionCollider.collisionWithMovingCubeEvent += ChangeDirection;
        rightChangeDirectionCollider.collisionWithMovingCubeEvent += ChangeDirection;
    }

    private void Update()
    {
        moveCube();
    }

    public void ChangeDirection()
    {
        direction *= -1;
    }

    private void moveCube()
    {
        CurrentMovingCube.position += new Vector3(movingSpeed * 5 * Time.deltaTime, 0f, 0f) * direction;
    }
}
