using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lover Values", menuName = "ScriptableObjects/Lover Values", order = 0)]
public class LoverValuesScriptableObject : ScriptableObject
{
    [Header("Movement Values")]
    [Tooltip("How quickly a lover rotates.")]
    [SerializeField] [Range(0.01f, 360f)] private float _rotationSpeed;
    [Tooltip("How quickly a lover moves.")]
    [SerializeField] [Range(0.01f, 10f)] private float _moveSpeed;
    [Tooltip("The maximum angle (degrees) a lover can climb.")]
    [SerializeField] [Range(0f, 89.9f)] private float _slopeOffset;

    #region Properties
    public float RotationSpeed
    {
        get => _rotationSpeed;
        private set => _rotationSpeed = value;
    }
    public float MoveSpeed
    {
        get => _moveSpeed;
        private set => _moveSpeed = value;
    }
    public float SlopeOffset
    {
        get => _slopeOffset * Mathf.Deg2Rad;
        private set => _slopeOffset = value;
    }
    #endregion
}
