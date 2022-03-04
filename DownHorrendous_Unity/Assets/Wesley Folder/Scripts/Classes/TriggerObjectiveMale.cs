using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class for male rigidbodies that are involved in a TriggerObjective. Male rigidbodies are those that trigger female triggers.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class TriggerObjectiveMale : MonoBehaviour
{
    [NonSerialized]
    public TriggerObjective.TriggerCouple couple;

    private void OnTriggerEnter(Collider other)
    {
        if (Array.Exists(couple.possibleFemaleTriggers, x => x == other))
        {
            couple.numberOfTriggers++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Array.Exists(couple.possibleFemaleTriggers, x => x == other))
        {
            couple.numberOfTriggers--;
        }
    }
}
