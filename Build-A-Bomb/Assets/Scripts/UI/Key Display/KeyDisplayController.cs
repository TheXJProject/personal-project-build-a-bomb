using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDisplayController : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] List<GameObject> orderedKeyDisplaysBeg;
    [SerializeField] List<GameObject> orderedKeyDisplaysMid;
    [SerializeField] List<GameObject> orderedKeyDisplaysEnd;

    private void OnEnable()
    {
        TaskStatus.onKeyDecided += FindAndAssignKeyDisplay;
    }

    private void OnDisable()
    {
        TaskStatus.onKeyDecided -= FindAndAssignKeyDisplay;
    }

    void FindAndAssignKeyDisplay(GameObject triggerTask)
    {
        // Look for an unnused key display in the beginning displays
        for (int i = 0; i < orderedKeyDisplaysBeg.Count; i++)
        {
            if (!orderedKeyDisplaysBeg[i].activeSelf)
            {
                AssignKeyDisplay(triggerTask, orderedKeyDisplaysBeg[i]);
                return;
            }
        }

        // Look for an unnused key display in the middle displays
        for (int i = 0; i < orderedKeyDisplaysMid.Count; i++)
        {
            if (!orderedKeyDisplaysMid[i].activeSelf)
            {
                AssignKeyDisplay(triggerTask, orderedKeyDisplaysMid[i]);
                return;
            }
        }

        // Look for an unnused key display in the end displays
        for (int i = 0; i < orderedKeyDisplaysEnd.Count; i++)
        {
            if (!orderedKeyDisplaysEnd[i].activeSelf)
            {
                AssignKeyDisplay(triggerTask, orderedKeyDisplaysEnd[i]);
                return;
            }
        }
    }

    void AssignKeyDisplay(GameObject triggerTask, GameObject availableDisplay)
    {
        // Actiate the display
        availableDisplay.SetActive(true);

        // Get the most recently added key
        TaskStatus taskStats = triggerTask.GetComponent<TaskStatus>();
        int taskKey = taskStats.keys[taskStats.keys.Count - 1];

        // Assign the key and corresponding task in the display itself
        availableDisplay.GetComponent<KeyDisplayVisuals>().AssignTaskLetter(triggerTask, taskKey);

        // Return from function since task is complete
        return;
    }
}
