using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    [Header("Dependencies")]

    [SerializeField] private StackMiniGame_GameplayManager gameplayManager;

    [SerializeField] private CubeStack cubeStack;

    [SerializeField] private CubeSpawner cubeSpawner;

    private Transform currentMovingCube;

    [Header("Gameplay")]
    [Range(0.01f, 100f)]
    [SerializeField] private float normalMovingSpeed;
    [Range(0.01f, 100f)]
    [SerializeField] private float slowMovingSpeed;
    [Range(0.0f, 15f)]
    [SerializeField] private float slowDownDistanceThreshold;

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
        Temp();
    }

    public void ChangeDirection()
    {
        direction *= -1;
    }

    private void Temp()
    {
        if(currentMovingCube == null || canMoveCube == false)
           return;

        if (cubeStack.StackedCubes.Count == 1)
        {
            MoveCube(normalMovingSpeed);
            return;
        }

        float cubesDistance = Mathf.Abs(cubeStack.StackedCubes[cubeStack.StackedCubes.Count - 2].transform.position.x - currentMovingCube.position.x);

        if (cubesDistance <= slowDownDistanceThreshold)
            MoveCube(slowMovingSpeed);

        else
            MoveCube(normalMovingSpeed);
    }

    private void MoveCube(float speed)
    {
        currentMovingCube.position += new Vector3(speed * Time.deltaTime, 0f, 0f) * direction;
    }

    private void ToggleMoveCube(bool onOff)
    {
        canMoveCube = onOff;
    }

    public IEnumerator FreezeCubeMovement(float seconds)
    {
        ToggleMoveCube(false);

        yield return new WaitForSeconds(seconds);

        ToggleMoveCube(true);
    }


    private void SetMovingCube(GameObject cube)
    {
        currentMovingCube = cube.transform;
    }
}
