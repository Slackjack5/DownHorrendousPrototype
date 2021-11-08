using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static float SlipHeight
    {
        get => _slipHeight;
        private set => _slipHeight = value;
    }
    private static float _slipHeight;
    [SerializeField] [Range(1f, 10f)] private float slipHeight;

    public static float SlipTorqueMagnitude
    {
        get => _slipTorqueMagnitude;
        private set => _slipTorqueMagnitude = value;
    }
    private static float _slipTorqueMagnitude;
    [SerializeField] [Range(1f, 10f)] private float slipTorqueMagnitude;

    public static LayerMask GroundMask
    {
        get => _groundMask;
        private set => _groundMask = value;
    }
    private static LayerMask _groundMask;
    [SerializeField] private LayerMask groundMask;

    public static int SlipperyLayer
    {
        get => _slipperyLayer;
        private set => _slipperyLayer = value;
    }
    private static int _slipperyLayer;
    [SerializeField] private int slipperyLayer;

    void Awake()
    {
        SlipHeight = slipHeight;
        SlipTorqueMagnitude = slipTorqueMagnitude;
        GroundMask = groundMask;
        SlipperyLayer = slipperyLayer;
    }

}
