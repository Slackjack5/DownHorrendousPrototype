using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class for male rigidbodies that are involved in a CollisionObjective. Male rigidbodies are those that collide with female colliders.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CollisionObjectiveMale : MonoBehaviour
{
    [NonSerialized]
    public CollisionObjective.ColliderCouple couple;

    private void OnCollisionEnter(Collision collision)
    {
        if (Array.Exists(couple.possibleFemaleColliders, x => x == collision.collider))
        {
            couple.numberOfCollisions++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (Array.Exists(couple.possibleFemaleColliders, x => x == collision.collider))
        {
            couple.numberOfCollisions--;
        }
    }
}
