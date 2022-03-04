using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// CollisionObjective is any objective that requires a collision to be completed.
/// </summary>
public class CollisionObjective : Objective
{
    [Serializable]
    public class ColliderCouple
    {
        [Tooltip("Male rigidbodies are those that collide with female colliders.")]
        public Rigidbody[] possibleMaleRigidbodies;

        [Tooltip("Females colliders are those that male rigidbodies collide with.")]
        public Collider[] possibleFemaleColliders;

        [Tooltip("How many collisions are currently occurring between males and females of this set.")]
        [NonSerialized]
        public int numberOfCollisions;
    }

    [Tooltip("All sets of colliders that must be colliding simultaneously to complete this objective.")]
    [SerializeField]
    protected ColliderCouple[] _colliderSets;

    protected List<CollisionObjectiveMale> _males = new List<CollisionObjectiveMale>();

    public override void Initialize()
    {
        IsCompleted = false;
        foreach (ColliderCouple couple in _colliderSets)
        {
            couple.numberOfCollisions = 0;
            foreach (Rigidbody male in couple.possibleMaleRigidbodies)
            {
                CollisionObjectiveMale newMale = male.gameObject.AddComponent<CollisionObjectiveMale>();
                newMale.couple = couple;
                _males.Add(newMale);
            }
        }
    }

    public override void CheckCompletion()
    {
        if (Array.TrueForAll(_colliderSets, x => x.numberOfCollisions > 0))
        {
            if (IsCompletionImmutable)
            {
                //clean up male collision detectors if they don't need to be checked anymore
                foreach (CollisionObjectiveMale male in _males)
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
