using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lover Values", menuName = "ScriptableObjects/Lover Values", order = 0)]
public class LoverValuesScriptableObject : ScriptableObject
{
    [Header("Movement Values")]
    [SerializeField] [Range(0.01f, 360f)] private float _rotationSpeed;
    [SerializeField] [Range(0.01f, 10f)] private float _moveSpeed;
    [SerializeField] [Range(0f, 89.9f)] private float _slopeOffset;
    [SerializeField] [Range(0.01f, 89.9f)] private float _stepOffset;

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
        get => _moveSpeed;
        private set => _moveSpeed = value;
    }
    public float StepOffset
    {
        get => _stepOffset;
        private set => _stepOffset = value;
    }
    #endregion
}
