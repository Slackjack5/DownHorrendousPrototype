using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private bool useBuiltInGravity;

    public static float CenterOfMass { get; private set; }
    [SerializeField] [Range(-1f, 1f)] private float centerOfMass;

    public static float StabilizeSpeed { get; private set; }
    [Header("Upright Stabilization")]
    [SerializeField] [Range(0.01f, 5f)] private float stabilizeSpeed;

    public static float StabilityFactor { get; private set; }
    [SerializeField] [Range(0.01f, 5f)] private float stabilityFactor;

    void Awake()
    {
        CenterOfMass = centerOfMass;
        StabilizeSpeed = stabilizeSpeed;
        StabilityFactor = stabilityFactor;
    }
}
