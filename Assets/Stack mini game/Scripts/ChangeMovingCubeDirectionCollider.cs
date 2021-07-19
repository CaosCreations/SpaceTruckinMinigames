using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMovingCubeDirectionCollider : MonoBehaviour
{
    public Action collisionWithMovingCubeEvent;


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision with " + other);
        collisionWithMovingCubeEvent.Invoke();
    }

}
