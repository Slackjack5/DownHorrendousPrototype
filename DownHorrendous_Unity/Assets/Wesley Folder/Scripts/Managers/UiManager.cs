using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiManager
{
    public void Initialize()
    {
        Services.levelEventManager.completeObjective += DisplayCheckmark;
        Services.levelEventManager.uncompleteObjective += UndisplayCheckmark;
    }

    private void DisplayCheckmark(Objective completedObjective)
    {
        if (!completedObjective.Checkmark.enabled)
        {
            completedObjective.Checkmark.enabled = true;
        }
    }

    private void UndisplayCheckmark(Objective uncompletedObjective)
    {
        if (uncompletedObjective.Checkmark.enabled)
        {
            uncompletedObjective.Checkmark.enabled = false;
        }
    }
}
