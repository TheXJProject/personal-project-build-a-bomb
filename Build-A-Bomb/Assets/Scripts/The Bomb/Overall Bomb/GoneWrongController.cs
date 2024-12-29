using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoneWrongController : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] float initialIntervalTime = 5f;

    // Runtime Variables:
    bool goingWrong = false;
    float timeSinceLastGoneWrong = 0f;
    System.Random rnd = new System.Random();
    [HideInInspector] public List<GameObject> tasksToGoWrong = new List<GameObject>();
    int numTasksGoingWrongNow = 0;

    private void OnEnable()
    {
        TaskStatus.onTaskCompleted += CheckTasksGoingWrong;
    }

    private void OnDisable()
    {
        TaskStatus.onTaskCompleted -= CheckTasksGoingWrong;
    }


    private void Update()
    {
        // If the controller is going wrong
        if (goingWrong)
        {
            // Progress the time tracker variables
            timeSinceLastGoneWrong += Time.deltaTime;

            // So at every interval make tasks go wrong
            if (timeSinceLastGoneWrong > initialIntervalTime)
            {
                MakeTasksGoWrong(HowManyTasksGoWrong());
                timeSinceLastGoneWrong = 0f;
            }
        }
    }

    /// <summary>
    /// Called when the bomb starts going wrong, initialises and begins everything 
    /// </summary>
    public void StartGoingWrong()
    {
        goingWrong = true;
    }

    /// <summary>
    /// When called, goes through tasks in the parameter layer given adds them <br/>
    /// to a list which should contain completed tasks that could go wrong
    /// </summary>
    public void AddNewLayerToChooseFrom(GameObject newLayerToGoWrong)
    {
        LayerStatus layerStats = newLayerToGoWrong.GetComponent<LayerStatus>();

        // Add every task in the layer to the list of tasks that could go wrong and set them to not be going wrong
        for (int i = 0; i < layerStats.tasks.Count; i++)
        {
            tasksToGoWrong.Add(layerStats.tasks[i]);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void CheckTasksGoingWrong(GameObject taskCompleted)
    {
        if (taskCompleted.GetComponent<TaskStatus>().hasBeenSolved)
        {
            --numTasksGoingWrongNow;
        }
    }

    /// <summary>
    /// Determine a good number of tasks to go wrong
    /// </summary>
    int HowManyTasksGoWrong()
    {
        // TODO: Make the number of tasks going wrong a better number
        return 1;
    }

    /// <summary>
    /// Using the array of tasks that are going wrong, set the correct number to go wrong
    /// </summary>
    void MakeTasksGoWrong(int numTasksToGoWrong)
    {
        int tasksChosen = 0;
        int tasksAlreadyGoneWrong = 0;

        // For every task in the list of tasks that could go wrong
        for (int taskNum = 0; taskNum < tasksToGoWrong.Count; taskNum++)
        {
            // Determine the number of tasks that still need to be chosen to go wrong
            int remainingToChoose = numTasksToGoWrong - tasksChosen;

            // Determine the number of tasks that are remaining to choose from, accounting for tasks in the list that are already going wrong
            // ((numTasksGoingWrongNow - tasksAlreadyGoneWrong) balances probability calculation so that it ignores tasks already going wrong)
            int remainingToChooseFrom = tasksToGoWrong.Count - taskNum - (numTasksGoingWrongNow - tasksAlreadyGoneWrong);

            // If there are no tasks to choose from, then there is no possible taskt that can go wrong, so break
            if (remainingToChooseFrom == 0) break;

            // Randomly decide if the task should be chosen based on the probability of numTasksLeftToChoose / numTasksToChooseFrom
            if (rnd.Next() % remainingToChooseFrom < remainingToChoose)
            {
                // If the tasks is already going wrong, account for that
                if (tasksToGoWrong[taskNum].GetComponent<TaskStatus>().isGoingWrong) ++tasksAlreadyGoneWrong; 
                else
                { 
                    // Else make it go wrong and account for that
                    tasksChosen++;
                    tasksToGoWrong[taskNum].GetComponent<TaskStatus>().TaskGoneWrong();

                    // Then if the correct amount of tasks have been chosen to go wrong: break
                    if (tasksChosen == numTasksToGoWrong) break;
                }
            }
        }

        // Update the current number of tasks going wrong
        numTasksGoingWrongNow += tasksChosen;
    }
}
