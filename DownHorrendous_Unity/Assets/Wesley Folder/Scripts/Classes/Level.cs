using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all levels.
/// </summary>
public class Level : MonoBehaviour
{
    [Tooltip("All objectives that are completable in this level. The last objective must be the final objective that completes the level.")]
    [SerializeField] private Objective[] _objectives;

    public Objective[] Objectives
    {
        get => _objectives;
        private set => _objectives = value;
    }
}
