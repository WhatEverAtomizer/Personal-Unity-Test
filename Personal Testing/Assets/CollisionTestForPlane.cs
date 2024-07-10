using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTestForPlane : MonoBehaviour
{
    //[SerializeField] private Transform
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Lift Wheels");
    }
}
