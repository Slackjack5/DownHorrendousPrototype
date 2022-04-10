using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventManager
{
    public delegate void CompleteObjective(Objective completedObjective);
    public delegate void UncompleteObjective(Objective uncompletedObjective);
    public delegate void WinLevel();

    [Tooltip("Event that fires when an objective goes from being uncompleted to completed.")]
    public CompleteObjective completeObjective;

    [Tooltip("Event that fires when an objective goes from being completed to uncompleted.")]
    public UncompleteObjective uncompleteObjective;

    [Tooltip("Event that fires when the level is finished.")]
    public WinLevel winLevel;
}
