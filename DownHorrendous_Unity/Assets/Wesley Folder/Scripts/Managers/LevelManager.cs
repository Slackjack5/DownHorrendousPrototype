using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    [Tooltip("The level this is managing.")]
    [SerializeField]
    private Level _level;

    [Tooltip("Objectives that haven't been completed yet that can be completed.")]
    private List<Objective> _activeObjectives;

    private void Awake()
    {
        Services.levelManager = this;
        Services.Initialize();
        foreach(Objective objective in _level.Objectives)
        {
            objective.Initialize();
        }
    }

    void Start()
    {
        _activeObjectives = Array.FindAll(_level.Objectives, x => x.PrerequisiteObjectives.Length == 0).ToList();

        #region subscribe to event manager
        Services.levelEventManager.completeObjective += OnObjectiveCompletion;
        Services.levelEventManager.uncompleteObjective += OnObjectiveUncompletion;
        #endregion
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        #region Check Objectives
        if (_activeObjectives.Count > 0)
        {
            CheckObjectives(_activeObjectives);
            /* UNUSED: see ObjectivesInRangeOfLovers function at bottom.
            List<Objective> objectivesInRangeOfLovers = ObjectivesInRangeOfLovers(Services.lovers);
            if (objectivesInRangeOfLovers?.Count > 0)
            {
                CheckObjectives(objectivesInRangeOfLovers);
            }
            */
        }
        #endregion
    }

    /// <summary>
    /// Checks whether the given objectives are completed and fires the CompleteObjective event if they are.
    /// </summary>
    /// <param name="objectives">Objectives to check completion status of.</param>
    private void CheckObjectives(List<Objective> objectives)
    {
        List<Objective> newlyCompletedObjectives = new List<Objective>();
        List<Objective> newlyUncompletedObjectives = new List<Objective>();
        foreach (Objective objective in objectives)
        {
            if (!objective.IsCompleted)
            {
                //check if objective went from being uncompleted to completed
                objective.CheckCompletion();
                if (objective.IsCompleted)
                {
                    newlyCompletedObjectives.Add(objective);
                }
            }
            else
            {
                //check if objective went from being completed to uncompleted
                objective.CheckCompletion();
                if (!objective.IsCompleted)
                {
                    newlyUncompletedObjectives.Add(objective);
                }
            }
        }
        foreach(Objective objective in newlyCompletedObjectives)
        {
            Services.levelEventManager.completeObjective.Invoke(objective);
        }
        foreach(Objective objective in newlyUncompletedObjectives)
        {
            Services.levelEventManager.uncompleteObjective.Invoke(objective);

        }
    }

    private void OnObjectiveCompletion(Objective objective)
    {
        Debug.Log(objective.Name + " completed.");
        if (objective.IsCompletionImmutable)
        {
            //clean up objective if it doesn't need to be checked anymore
            _activeObjectives.Remove(objective);
        }
        //check if a previously uncompletable objective (i.e. one with a prerequisite) is now completable
        Objective[] incompleteObjectivesWithPrerequisites = Array.FindAll(_level.Objectives, x => x.PrerequisiteObjectives.Length > 0 && !x.IsCompleted && !_activeObjectives.Contains(x));
        foreach(Objective objectiveWithPrerequisites in incompleteObjectivesWithPrerequisites)
        {
            if(Array.TrueForAll(objectiveWithPrerequisites.PrerequisiteObjectives, x => x.IsCompleted))
            {
                _activeObjectives.Add(objectiveWithPrerequisites);
            }
        }
    }

    private void OnObjectiveUncompletion(Objective uncompletedObjective)
    {
        //check if a previously completable objective (i.e. one with a prerequisite) is now uncompletable
        Objective[] activeObjectivesWithPrerequisites = _activeObjectives.FindAll(x => x.PrerequisiteObjectives.Length > 0).ToArray();
        foreach (Objective objectiveWithPrerequisites in activeObjectivesWithPrerequisites)
        {
            if (Array.Exists(objectiveWithPrerequisites.PrerequisiteObjectives, x => !x.IsCompleted))
            {
                _activeObjectives.Remove(objectiveWithPrerequisites);
            }
        }
    }

    /* UNUSED: way of only checking nearby objectives. Not used because:
     * A. only checks in range of lovers, so would fail if objective is not near lovers (i.e. shoot a ball into the net).
     * B. LoverRangeDistance would have to be big enough to account for all sizes of colliders, since transform.position is used to determine positions.
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lovers">Lovers to check which objectives are in range of.</param>
    /// <returns>All active objectives that are within range of the given lovers.</returns>
    private List<Objective> ObjectivesInRangeOfLovers(List<Lover> lovers)
    {
        List<Objective> objectivesInRangeOfLovers = new List<Objective>();
        foreach (Lover lover in lovers)
        {
            foreach (Objective objective in _activeObjectives)
            {
                if (objectivesInRangeOfLovers.Contains(objective))
                {
                    continue;
                }
                foreach (Vector3 objectivePosition in objective.Positions)
                {
                    if(Vector3.Distance(lover.transform.position, objectivePosition) < Constants.LoverRangeDistance)
                    {
                        objectivesInRangeOfLovers.Add(objective);
                        continue;
                    }
                }
            }
        }
        return objectivesInRangeOfLovers;
    }
    */
}
