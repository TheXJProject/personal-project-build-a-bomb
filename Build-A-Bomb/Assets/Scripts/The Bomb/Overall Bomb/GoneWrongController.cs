using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoneWrongController : MonoBehaviour
{
    // Inpector Adjustable Values:
    [SerializeField] bool tasksGoWrong = true;
    [SerializeField] [Range(1,15)] int difficultyCurb; // Where difficultyCurb is c in equation, y = (-2 ^ (d-cx)) + 1
    [SerializeField] [Range(0,6)] float difficultyOffset; // Where difficultyOffset is d in equation, y = (-2 ^ (d-cx)) + 1
    [SerializeField] float initialIntervalTime = 20f;
    [SerializeField] float finalIntervalTime = 7f;
    [SerializeField] float intervalTimeVariation = 4f;
    [SerializeField] float initialAmountFailing = 1f;
    [SerializeField] float finalAmountFailing = 4f;

    // Initialise In Inspector:
    [SerializeField] BombStatus bombStats;

    // Runtime Variables:
    float currentIntervalTime;
    float currentAmountFailing;
    float intervalDiff;
    float amountFailDiff;
    float curbRes = 0f;
    int currentLayer = 0;
    int maxLayers;
    bool goingWrong = false;
    float timeSinceLastGoneWrong = 0f;
    System.Random rnd = new System.Random();
    [HideInInspector] public List<GameObject> tasksToGoWrong = new List<GameObject>();
    int numTasksGoingWrongNow = 0;

    private void Awake()
    {
        maxLayers = bombStats.layersToBeSpawned.Count - (bombStats.layerToStartGoingWrong+1);
        intervalDiff = finalIntervalTime - initialIntervalTime;
        amountFailDiff = finalAmountFailing - initialAmountFailing;

        float variance = (rnd.Next(-(int)(intervalTimeVariation * 10), (int)(intervalTimeVariation * 10))) / 10.0f;

        currentIntervalTime = initialIntervalTime + variance;
        currentAmountFailing = initialAmountFailing;
    }

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
            if (timeSinceLastGoneWrong > currentIntervalTime)
            {
                // Make the task(s) go wrong
                if (tasksGoWrong) MakeTasksGoWrong(HowManyTasksGoWrong());

                // Reset variables for the next interval
                timeSinceLastGoneWrong = 0f;

                // Calculate variance of time intervals then the next time interval and number of tasks to go wrong
                float variance = (rnd.Next(-(int)(intervalTimeVariation * 10), (int)(intervalTimeVariation * 10))) / 10.0f;
                currentIntervalTime = initialIntervalTime + (intervalDiff * curbRes) - variance;
                currentAmountFailing = initialAmountFailing + (amountFailDiff * curbRes);
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

        // Increase the difficulty per layer only once the bomb has started going wrong
        if (goingWrong) 
        {
            ++currentLayer;
            // Calculate difficulty between 0 and 1 based on y = (-2 ^ (-cx)) + 1, where c is the value used to control the shape of the curb
            curbRes = -MathF.Pow(2.0f, difficultyOffset - (difficultyCurb * ((float)currentLayer / maxLayers))) + 1.0f;
        }
    }

    /// <summary>
    /// Called whenever a task is completed, if that task is a task that was previously completed, then it went wrong but is now fixed
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
        // The value of the number of tasks to go wrong is a float, so calculate the probability that the value to use rounds up or down
        int probPlusOne = (int)((currentAmountFailing - (int)currentAmountFailing) * 100.0f);
        
        // Using the probability value, determine whether to increase the max value that the return val could be by one
        int plusOne = (rnd.Next(1, 100) < probPlusOne) ? 1 : 0;

        // Return a number between1 and the current value rounded down (+ plusOne)
        int returnVal = (rnd.Next(1, ((int)currentAmountFailing + 1 + plusOne)));
        return returnVal;
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
