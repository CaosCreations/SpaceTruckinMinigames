using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    [SerializeField] private GameplayManager gameplayManager;

    [SerializeField] private CubeSpawner cubeSpawner;

    private Transform currentMovingCube;

    [SerializeField] private float movingSpeed;

    private float direction = 1;

    private bool canMoveCube;

    private void Awake()
    {
        cubeSpawner.CubeSpawnedEvent += SetMovingCube;

        gameplayManager.GameEndEvent += () => ToggleMoveCube(false);
        gameplayManager.GameResetEvent += () => ToggleMoveCube(true);

        ChangeMovingCubeDirectionCollider[] ChangeMovingCubeDirectionColliders = GetComponentsInChildren<ChangeMovingCubeDirectionCollider>();

        foreach(ChangeMovingCubeDirectionCollider item in ChangeMovingCubeDirectionColliders)
        {
            item.CollisionWithMovingCubeEvent += ChangeDirection;
        }
    }

    private void Update()
    {
        MoveCube();
    }

    public void ChangeDirection()
    {
        direction *= -1;
    }

    private void MoveCube()
    {
        if(currentMovingCube != null && canMoveCube == true)
            currentMovingCube.position += new Vector3(movingSpeed * 5 * Time.deltaTime, 0f, 0f) * direction;
    }

    private void ToggleMoveCube(bool onOff)
    {
        canMoveCube = onOff;
    }

    private void SetMovingCube(GameObject cube)
    {
        currentMovingCube = cube.transform;
    }
}
