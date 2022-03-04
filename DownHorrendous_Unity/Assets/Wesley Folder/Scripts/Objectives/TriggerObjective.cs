using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// TriggerObjective is any objective that requires a collider(s) to enter a trigger to be completed.
/// </summary>
public class TriggerObjective : Objective
{
    [Serializable]
    public class TriggerCouple
    {
        [Tooltip("Male rigidbodies are those that trigger female colliders.")]
        public Rigidbody[] possibleMaleRigidbodies;

        [Tooltip("Females triggers are those that male rigidbodies trigger.")]
        public Collider[] possibleFemaleTriggers;

        [Tooltip("How many triggers are currently occurring between males and females of this set.")]
        [NonSerialized]
        public int numberOfTriggers;
    }

    [Tooltip("All sets of rigidbodies and triggers that must be triggering simultaneously to complete this objective.")]
    [SerializeField]
    protected TriggerCouple[] _triggerSets;

    protected List<TriggerObjectiveMale> _males = new List<TriggerObjectiveMale>();

    public override void Initialize()
    {
        IsCompleted = false;
        foreach (TriggerCouple couple in _triggerSets)
        {
            couple.numberOfTriggers = 0;
            foreach (Rigidbody male in couple.possibleMaleRigidbodies)
            {
                TriggerObjectiveMale newMale = male.gameObject.AddComponent<TriggerObjectiveMale>();
                newMale.couple = couple;
                _males.Add(newMale);
            }
        }
    }

    public override void CheckCompletion()
    {
        if (Array.TrueForAll(_triggerSets, x => x.numberOfTriggers > 0))
        {
            if (IsCompletionImmutable)
            {
                //clean up male trigger detectors if they don't need to be checked anymore
                foreach (TriggerObjectiveMale male in _males)
                {
                    Destroy(male);
                }
            }
            if (!IsCompleted)
            {
                IsCompleted = true;
            }
            return;
        }
        else
        {
            if (IsCompleted)
            {
                IsCompleted = false;
                return;
            }
        }
    }
}
