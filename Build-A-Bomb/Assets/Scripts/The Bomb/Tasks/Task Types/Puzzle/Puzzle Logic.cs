using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class PuzzleLogic : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Constant Values:
    const int maxPossibleDifficultly = 7;
    const int minPossibleDifficultly = 1;
    public const int finalPosition = 0;
    const int minRequiredForPipeInteraction = 3;
    const int randomKeyMultiplier = 1000;

    // Inspector Adjustable Values:
    [Range(minPossibleDifficultly, maxPossibleDifficultly)] public int currentHardestDifficulty;
    // Inspector Adjustable Values:
    [Header("(Number of possible rotations for the symbol)")] public int maxPositions;
    [SerializeField] float flashButtonTime;
    [SerializeField] int maxAreaWidth;

    // Initialise In Inspector:
    public TaskInteractStatus statInteract;
    [SerializeField] Image light_;
    [SerializeField] Sprite lightOff;
    [SerializeField] Sprite lightOn;
    [SerializeField] Image button;
    [SerializeField] GameObject pipe;

    // Runtime Variables:
    int numOfConnectionsNeeded = minPossibleDifficultly;
    int numOfConnections = 0;
    GameObject[] pipeList;
    int[] pipeStartList;
    int randomKey = 0;
    bool isSetup;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // This instance is not set up yet
        isSetup = false;
        light_.sprite = lightOff;
        button.color = Color.red;
    }

    private void OnEnable()
    {
        TaskInteractStatus.onTaskFailed += ResetTask;
        TaskInteractStatus.onTaskDifficultySet += SetDifficulty;
    }

    private void OnDisable()
    {
        TaskInteractStatus.onTaskFailed -= ResetTask;
        TaskInteractStatus.onTaskDifficultySet -= SetDifficulty;
    }

    private void Update()
    {
        if (!statInteract.isBeingSolvedAndSelected) return;

        numOfConnections = 0;

        // For each pipe
        foreach (var pipe in pipeList)
        {
            // Check if it is matchinig
            if (pipe.GetComponent<Pipe>().inCorrectPos)
            {
                numOfConnections++;
            }
        }

        // Set light colour
        // If no connections
        if (numOfConnections == 0)
        {
            light_.sprite = lightOff;
        }
        // Otherwise, if we have made all connections
        else if (numOfConnections == numOfConnectionsNeeded)
        {
            light_.sprite = lightOn;
        }
        // Otherwise, show in progress
        else
        {
            light_.sprite = lightOff;
        }

        // Set the completion level
        statInteract.SetTaskCompletion((float)numOfConnections / numOfConnectionsNeeded);
    }

    public void SetSeconPos(int ordernum)
    {
        // If not able to get second position
        if (numOfConnectionsNeeded < minRequiredForPipeInteraction) return;

        // Find the next pipe and add one to its position
        ordernum = (ordernum + 1) % numOfConnectionsNeeded;
        pipeList[ordernum].GetComponent<Pipe>().RotateOnePos();
    }

    /// <summary>
    /// Called by Button gameobject. When the player <br />
    /// clicks on the Button the function will check if <br />
    /// the task is complete.
    /// </summary>
    public void CompleteCheck(BaseEventData data)
    {
        // Checks if the task can be solved
        if (statInteract.isBeingSolvedAndSelected)
        {
            PointerEventData newData = (PointerEventData)data;
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                // Flash the button press
                StartCoroutine(FlashButton());

                // Check if task is completed
                if (numOfConnections >= numOfConnectionsNeeded)
                {
                    statInteract.TaskCompleted();
                }
            }
        }
    }

    IEnumerator FlashButton()
    {
        // Flash the button as green for a set time
        button.color = Color.green;
        yield return new WaitForSeconds(flashButtonTime);
        button.color = Color.red;
    }

    void ResetPipes()
    {
        // For each pipe enter it's starting position
        for (int i = 0; i < numOfConnectionsNeeded; i++)
        {
            if (pipeList[i] == null)
            {
                Debug.LogWarning("Error, pipe doesn't exsist!");
                return;
            }
            pipeList[i].GetComponent<Pipe>().SetStartPosition(pipeStartList[i]);
        }
    }

    bool OkayCheck()
    {
        // Make sure the given problem is solvable
        // First get the total for the problem
        int total = 0;
        for (int i = 0; i < numOfConnectionsNeeded; i++)
        {
            total += pipeStartList[i];
        }

        // If the total is odd or zero it won't be solvable
        if (total == 0 || total % 2 != 0)
        {
            return false;
        }

        // If the number of pipes is odd then it will be solvable from here
        if (numOfConnectionsNeeded % 2 != 0)
        {
            return true;
        }
        else
        {
            // The total of ever other position mod (maxPositions) should be equal to
            // the mod (maxPositions) total of missed positions
            int total2 = 0;
            for (int i = 0; i < numOfConnectionsNeeded; i++)
            {
                if (i % 2 != 0)
                {
                    total2 += pipeStartList[i];
                    total -= pipeStartList[i];
                }
            }

            // If these two values are equal the positions are balanced and the problem is solvable
            return (total % maxPositions == total2 % maxPositions);
        }
    }

    void SetPipePositions(int key)
    {
        bool okay;
        int attempts = 0;

        // Repeat untill the selection has passed all the checks
        do
        {
            // If we have less than the number of pipes required for linking, use a different calculation system
            if (numOfConnectionsNeeded < minRequiredForPipeInteraction)
            {
                okay = true;
                // For each position
                for (int i = 0; i < numOfConnectionsNeeded; i++)
                {
                    // Set a position using the given key
                    pipeStartList[i] = key % maxPositions;
                    key = (key - pipeStartList[i]) / maxPositions;

                    // If the key is less than zero, reset it
                    if (key < 0)
                    {
                        // Try next seed along
                        key = ++randomKey;
                    }

                    // If a pipe starts in a complete position, try again
                    if (pipeStartList[i] == 0)
                    {
                        okay = false;
                        break;
                    }
                }
            }
            else
            {
                // For each position
                for (int i = 0; i < numOfConnectionsNeeded; i++)
                {
                    // Set a position using the given key
                    pipeStartList[i] = key % maxPositions;
                    key = (key - pipeStartList[i]) / maxPositions;

                    // If the key is less than or equal to zero, reset it
                    if (key <= 0)
                    {
                        // Try next seed along
                        key = ++randomKey;
                    }
                }

                // Check if set of positions is okay
                okay = OkayCheck();
            }

            attempts++;

            // Exit if too many attempts
            if (attempts > 1000 && !okay)
            {
                // Used a default set position
                for (int i = 0; i < numOfConnectionsNeeded; i++)
                {
                    // Set a position using the given key
                    pipeStartList[i] = maxPositions / 2;
                }
             
                // Exit
                break;
            }
        } while (!okay);
    }

    /// <summary>
    /// Called by SetDifficulty method only! <br />
    /// Starts required setup for the task. <br />
    /// </summary>
    void SetupTask()
    {
        // This function can only be activated once
        if (isSetup)
        {
            Debug.LogWarning("Error, this task is already set up!");
        }
        else
        {
            // This instance is now setup
            isSetup = true;

            // Initialise the pipe arrays and list
            pipeList = new GameObject[numOfConnectionsNeeded];
            pipeStartList = new int[numOfConnectionsNeeded];

            // Calculate position for each pipe
            float halfWidth = (float)maxAreaWidth / 2;
            float seperation = (float)maxAreaWidth / (numOfConnectionsNeeded + 1);

            // Spawn in a number of pipes equal to the difficulty
            for (int i = 0; i < numOfConnectionsNeeded; i++)
            {
                // Create a pipe object
                var thispipe = Instantiate(pipe, Vector2.zero, Quaternion.identity, transform.GetChild(0).transform);
                thispipe.GetComponent<Pipe>().Setup();

                // Set order number
                thispipe.GetComponent<Pipe>().orderNumber = i;

                pipeList[i] = thispipe;
            }

            // Create array 0,1,2,3..,numOfConnectionsNeeded-1
            int[] randomNumbers = Enumerable.Range(0, numOfConnectionsNeeded).ToArray();

            // Shuffle numbers around
            for (int i = 0; i < randomNumbers.Length; i++)
            {
                int randIndex = Random.Range(i, randomNumbers.Length);
                (randomNumbers[i], randomNumbers[randIndex]) = (randomNumbers[randIndex], randomNumbers[i]);
            }

            // Arrange to pipes according to the random number array
            for (int i = 0; i < numOfConnectionsNeeded; i++)
            {
                // Use the indexs from the random number array
                int index = randomNumbers[i];

                // Get current posistion of pipe
                Vector3 currentPos = pipeList[index].transform.localPosition;

                // put the pipe in the correct position
                currentPos.x = seperation * (i + 1) - halfWidth;
                pipeList[index].transform.localPosition = currentPos;
                pipeList[index].transform.SetAsFirstSibling();
            }

            // Set a one time random key (seed) for positions based on number of positions a pipe can have
            randomKey = Random.Range(0, maxPositions * randomKeyMultiplier);

            // Calculate pipe starting positions
            SetPipePositions(randomKey);

            // Initilise pipe positions
            ResetPipes();
        }
    }

    /// <summary>
    /// Called by onTaskDifficultySet event only! <br />
    /// Retrieves a difficult setting and applies it to this task <br />
    /// instance. Then calls for the task to be setup.
    /// </summary>
    void SetDifficulty(GameObject triggerTask)
    {
        // When the onTaskDifficultySet event is called, check whether the triggering gameobject is itself
        if (triggerTask == gameObject.transform.parent.gameObject)
        {
            if (Msg) Debug.Log("Set Difficultly");

            // Retrieves difficulty
            float difficulty = triggerTask.GetComponent<TaskStatus>().difficulty;

            // Sets difficulty level (the number of hits needed in this case)
            numOfConnectionsNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f);

            // The number of hits needed cannot be zero
            numOfConnectionsNeeded = Mathf.Max(numOfConnectionsNeeded, minPossibleDifficultly);

            SetupTask();
        }
    }

    /// <summary>
    /// Called by onTaskFailed event only! <br />
    /// Resets the task back to its state just after SetupTask <br />
    /// has been called.
    /// </summary>
    void ResetTask(GameObject trigger)
    {
        // When the onTaskDifficultySet event is called, check whether the triggering gameobject is itself
        if (trigger == gameObject)
        {
            if (Msg) Debug.Log("Reset Task");

            // Reset all the pipes back to start positions
            ResetPipes();

            // Reset the number of times the player has hit
            numOfConnections = 0;

            // Set the completion level
            statInteract.SetTaskCompletion((float)numOfConnections / numOfConnectionsNeeded);
        }
    }
}
