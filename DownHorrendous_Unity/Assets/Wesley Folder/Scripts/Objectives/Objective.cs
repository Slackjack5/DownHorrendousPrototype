using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Base class for all objectives.
/// </summary>
public abstract class Objective : MonoBehaviour
{
    [Tooltip("Name of the objective (used for UI).")]
    [SerializeField] protected string _name;

    [Tooltip("Once this objective is completed, can it not be uncompleted?")]
    [SerializeField] protected bool _isCompletionImmutable = true;

    [Tooltip("All objectives that must be completed before this one can be. Leave empty if this objective has no prerequisites. Prerequisite objectives must be included in the level.")]
    [SerializeField] protected Objective[] _prerequisiteObjectives;

    [Tooltip("Clipboard checkmark for this objective.")]
    [SerializeField] protected RawImage _checkmark;

    #region Properties
    public bool IsCompleted { get; protected set; }
    public string Name
    {
        get => _name;
        protected set => _name = value;
    }
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
    public RawImage Checkmark
    {
        get => _checkmark;
        protected set => _checkmark = value;
    }
    #endregion

    public virtual void Initialize() { }

    /// <summary>
    /// Checks whether this objective is completed.
    /// </summary>
    public abstract void CheckCompletion();
}
