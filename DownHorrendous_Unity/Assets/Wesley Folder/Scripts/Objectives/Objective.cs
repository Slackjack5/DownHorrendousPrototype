using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all objectives.
/// </summary>
public abstract class Objective : MonoBehaviour
{
    [Tooltip("Name of the objective (used for UI).")]
    [SerializeField]
    protected string _name;

    /* UNUSED: see ObjectivesInRangeOfLovers function at bottom of LevelManager script.
    protected Vector3[] _positions; //where the objective is (used in LevelManager to find ObjectivesInRange)
    */

    [Tooltip("Once this objective is completed, can it not be uncompleted?")]
    [SerializeField]
    protected bool _isCompletionImmutable = true;

    [Tooltip("All objectives that must be completed before this one can be. Leave empty if this objective has no prerequisites. Prerequisite objectives must be included in the level.")]
    [SerializeField]
    protected Objective[] _prerequisiteObjectives;

    #region Properties
    public bool IsCompleted { get; protected set; }
    public string Name
    {
        get => _name;
        protected set => _name = value;
    }
    /* UNUSED: see ObjectivesInRangeOfLovers function at bottom of LevelManager script.
    public Vector3[] Positions
    {
        get => _positions;
        protected set => _positions = value;
    }
    */
    public bool IsCompletionImmutable
    {
        get => _isCompletionImmutable;
        protected set => _isCompletionImmutable = value;
    }
    public Objective[] PrerequisiteObjectives
    {
        get => _prerequisiteObjectives;
        protected set => _prerequisiteObjectives = value;
    }
    #endregion

    public virtual void Initialize() { }

    /// <summary>
    /// Checks whether this objective is completed.
    /// </summary>
    public abstract void CheckCompletion();
}
