using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private bool useBuiltInGravity;

    public static float CenterOfMass
    {
        get => _centerOfMass;
        private set => _centerOfMass = value;
    }
    private static float _centerOfMass;
    [SerializeField] [Range(-1f, 1f)] private float centerOfMass;

    public static float StabilizeSpeed
    {
        get => _stabilizeSpeed;
        private set => _stabilizeSpeed = value;
    }
    private static float _stabilizeSpeed;
    [Header("Upright Stabilization")]
    [SerializeField] [Range(0.01f, 5f)] private float stabilizeSpeed;

    public static float StabilityFactor
    {
        get => _stabilityFactor;
        private set => _stabilityFactor = value;
    }
    private static float _stabilityFactor;
    [SerializeField] [Range(0.01f, 5f)] private float stabilityFactor;

    void Awake()
    {
        CenterOfMass = centerOfMass;
        StabilizeSpeed = stabilizeSpeed;
        StabilityFactor = stabilityFactor;
    }
}
