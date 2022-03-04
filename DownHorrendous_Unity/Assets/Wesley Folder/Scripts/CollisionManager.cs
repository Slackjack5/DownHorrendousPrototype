using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static float SlipHeight { get; private set; }
    [SerializeField] [Range(1f, 10f)] private float slipHeight;

    public static float SlipTorqueMagnitude { get; private set; }
    [SerializeField] [Range(1f, 10f)] private float slipTorqueMagnitude;

    public static LayerMask GroundMask { get; private set; }
    [SerializeField] private LayerMask groundMask;

    public static int SlipperyLayer { get; private set; }
    [SerializeField] private int slipperyLayer;

    void Awake()
    {
        SlipHeight = slipHeight;
        SlipTorqueMagnitude = slipTorqueMagnitude;
        GroundMask = groundMask;
        SlipperyLayer = slipperyLayer;
    }

}
