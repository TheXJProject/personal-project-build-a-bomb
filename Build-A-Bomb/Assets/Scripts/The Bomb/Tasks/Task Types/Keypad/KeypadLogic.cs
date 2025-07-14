using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadLogic : MonoBehaviour
{
    [System.Serializable]
    struct Codes
    {
        public string name;
        public List<int> code;
    }

    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Constant Values:
    const int maxPossibleDifficultly = 20;
    const int minPossibleDifficultly = 1;

    // Inspector Adjustable Values:
    [Range(minPossibleDifficultly, maxPossibleDifficultly)] public int currentHardestDifficulty;
    [SerializeField] float showTime = 1;
    [SerializeField] Codes[] secretCodes;

    // Initialise In Inspector:
    public TaskInteractStatus statInteract;
    [SerializeField] GameObject TaskDisplay;
    [SerializeField] TextLogic display;
    [SerializeField] GameObject[] keys;

    // Runtime Variables:
    int numOfPressesNeeded = minPossibleDifficultly;
    int numCorrectPresses = 0;
    List<int> codeSequence;
    List<int> playerSequence;
    [HideInInspector] public bool canClickKeys;
    Coroutine myCoroutine;
    static string typeInSeq;
    static string showInSeq;
    bool isSetup;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // This instance is not set up yet
        isSetup = false;
        canClickKeys = false;
    }

    private void OnEnable()
    {
        TaskInteractStatus.onTaskFailed += ResetSwitch;
        TaskInteractStatus.onTaskDifficultySet += SetDifficulty;
    }

    private void OnDisable()
    {
        TaskInteractStatus.onTaskFailed -= ResetSwitch;
        TaskInteractStatus.onTaskDifficultySet -= SetDifficulty;
    }

    /// FUNCTION DESCRIPTION<summary>
    /// Called when a key is pressed. This function decides what <br />
    /// to do with the given input. <br />
    /// Parameter: key number to be processed.
    /// </summary>
    public void KeyToProcess(int keyNumber)
    {
        // Depending on keyNumber, began correct function
        // -10 is show the code required
        if (keyNumber == -10)
        {
            // Reset the player's numbers shown
            typeInSeq = "";

            // Start showing code sequence required
            myCoroutine = StartCoroutine(ShowSequence());
        }
        // -20 is attempt to submit entered code
        else if (keyNumber == -20)
        {
            // Reset the player's numbers shown
            typeInSeq = "";

            // Reset keys to show
            showInSeq = "";

            // Check the player sequence is correct
            CheckCode();
        }
        // 0 - 9 is a number the player has entered
        else if (0 <= keyNumber && keyNumber < 10)
        {
            // Reset keys to show
            showInSeq = "";

            // Show all the numbers entered at once
            typeInSeq += keyNumber.ToString();

            // Add number to player sequence and display it
            playerSequence.Add(keyNumber);

            // If the number entered is too big
            if (typeInSeq.Length > maxPossibleDifficultly)
            {
                // Show the player
                display.DisplayText("-Err-");
            }
            else
            {
                // Show the player their input
                display.DisplayText(typeInSeq);
            }

            if (Msg) Debug.Log("Key entered into player sequence: " + keyNumber);
        }
        else
        {
            Debug.LogWarning("Error, key number incorrect value: " + keyNumber);
        }
    }

    /// <summary>
    /// If the player right clicks out of the task, stop the coroutine.<br />
    /// </summary>
    void CoroutineCheck()
    {
        // If the TaskDisplay isn't active begin check
        if (!TaskDisplay.activeSelf)
        {
            if (Msg) Debug.Log("TaskDisplay is " + TaskDisplay.activeSelf + ".");

            // If the code sequence is being shown cancel it
            if (myCoroutine != null)
            {
                // Stop showing code
                StopCoroutine(myCoroutine);
                myCoroutine = null;

                // Apply reset
                ResetPlayerSequence();
                canClickKeys = true;
            }
        }
    }

    /// <summary>
    /// Coroutine that shows the code the player needs to enter.<br />
    /// </summary>
    IEnumerator ShowSequence()
    {
        float timeElapsed;
        bool inSeqAlreadyFilled = false;
        string tempSeq = "";

        // For each number in the code sequence
        for (int i = 0; i < codeSequence.Count; i++)
        {
            // Create a temp seq to compare
            tempSeq += codeSequence[i].ToString();
        }

        // If we are already showing the code
        if (showInSeq == tempSeq)
        {
            inSeqAlreadyFilled = true;
        }

        // Prevent any player interaction
        canClickKeys = false;

        // For each number in the code sequence, show it in order
        for (int i = 0; i < codeSequence.Count; i++)
        {
            timeElapsed = 0f;

            // If we are not already showing the sequence
            if (!inSeqAlreadyFilled)
            {
                showInSeq += codeSequence[i].ToString();
            }

            // Show element in code sequence
            if (TaskDisplay.activeSelf) display.DisplayText(showInSeq);

            if (0 <= codeSequence[i] && codeSequence[i] < 10)
            {
                // Only show key if task can be seen
                if (TaskDisplay.activeSelf) keys[codeSequence[i]].GetComponent<KeyLogic>().ShowKey(showTime);
            }
            else
            {
                Debug.LogWarning("Error, Code Sequence was not between 0 and 10!");
            }

            // Wait for set amount of time
            while (timeElapsed < showTime)
            {
                // Increment the time elapsed and continue
                timeElapsed += Time.deltaTime;

                // Check if corountine can continue
                CoroutineCheck();

                // Wait for the next frame
                yield return null;
            }
        }

        ResetPlayerSequence();

        // Allow the player to interact again
        canClickKeys = true;
    }

    /// FUNCTION DESCRIPTION<summary>
    /// Return the amount of common elements in each list. <br />
    /// Parameter 1: The list the player is trying to match. <br />
    /// Parameter 2: The list inputted by the player.
    /// </summary>
    int CountCommonElements(List<int> code, List<int> player)
    {
        //Use the smaller size list to compare
        int minLength = System.Math.Min(code.Count, player.Count);
        int count = 0;

        // For each element in the shortest length list
        for (int i = 0; i < minLength; i++)
        {
            // If we have a matching element increase the count
            if (code[i] == player[i])
            {
                count++;
            }

            if (Msg) Debug.Log("Code: " + code[i] + " Player: " + player[i]);
        }

        // Return number of similar elements
        return count;
    }

    /// FUNCTION DESCRIPTION<summary>
    /// When called by the hash button, this checks if the inputted code is correct. <br />
    /// Then it sets the task completion level and if the task is completed respectively.
    /// </summary>
    void CheckCode()
    {
        // This function only works if the task canBeSolved
        if (statInteract.isBeingSolved)
        {
            // Check the playerSequence is instantiated
            if (playerSequence == null)
            {
                Debug.LogWarning("Error, playerSequence not instantiated!");
            }
            else if (codeSequence == null)
            {
                Debug.LogWarning("Error, codeSequence not instantiated!");
            }
            else
            {
                // calculate number correctly pressed
                numCorrectPresses = CountCommonElements(codeSequence, playerSequence);

                // Set the number to compare the number of correct presses to
                int numToCompare = System.Math.Max(numOfPressesNeeded, playerSequence.Count);

                // Set the completion level
                statInteract.SetTaskCompletion((float)numCorrectPresses / numToCompare);

                if (Msg) Debug.Log(100 * ((float)numCorrectPresses / numToCompare) + "% Complete");

                // Check if task is completed
                if ((numCorrectPresses >= numOfPressesNeeded) && (playerSequence.Count == numOfPressesNeeded))
                {
                    statInteract.TaskCompleted();
                }
                else
                {
                    bool showStandardMsg = true;
                    int common;

                    // For everypossible secret code
                    foreach (Codes secretCode in secretCodes)
                    {
                        // Check if the player has entered the secret code
                        common = CountCommonElements(secretCode.code, playerSequence);
                        if (common == secretCode.code.Count && secretCode.code.Count == playerSequence.Count)
                        {
                            // If the player entered the secret code show a secret message
                            display.DisplayText(secretCode.name);

                            // Don't show the normal message
                            showStandardMsg = false;
                            break;
                        }
                    }

                    // If we are to not show any secrect messages
                    if (showStandardMsg)
                    {
                        // Otherwise show incorrect to player
                        display.DisplayText("-Err-");
                    }
                }

                // Reset sequence if additional attempts needed
                ResetPlayerSequence();
            }
        }
    }

    /// FUNCTION DESCRIPTION<summary>
    /// This function resets any input the player has entered. <br />
    /// Then it sets the task completion level and if the task is completed respectively.
    /// </summary>
    public void ResetPlayerSequence()
    {
        // Check the playerSequence is instantiated
        if (playerSequence == null)
        {
            Debug.LogWarning("Error, playerSequence not instantiated! (Reset attempted)");
        }
        else
        {
            // Reset playerSequence to empty
            playerSequence = new();
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Returns a list of randomly generated intergers. <br />
    /// Parameter: Number of elements in the list.
    /// </summary>
    List<int> GenerateRandomNumbers(int numNeeded)
    {
        // Our list to return
        List<int> randomNumbers = new();
        System.Random rand = new();

        // Keep going until we fill the requirements
        while (randomNumbers.Count < numNeeded)
        {
            // Get a random number between 0 and 9
            int newNumb = rand.Next(10);

            // Adds new random integer to list
            randomNumbers.Add(newNumb);
        }

        if (Msg) Debug.Log("New Sequence(" + numNeeded + " long): " + string.Join(" ", randomNumbers));

        // Returns the list
        return randomNumbers;
    }

    /// FUNCTION DESCRIPTION <summary>
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

            // Instantiate and generate keypad sequence
            codeSequence = new();
            codeSequence = GenerateRandomNumbers(numOfPressesNeeded);

            // Instantiate player sequence list
            playerSequence = new();

            // The player can now interact
            canClickKeys = true;
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by onTaskDifficultySet event only! <br />
    /// Retrieves a difficult setting and applies it to this task <br />
    /// instance. Then calls for the task to be setup.
    /// </summary>
    void SetDifficulty(GameObject triggerTask)
    {
        // When the onTaskDifficultySet event is called, check whether the triggering gameobject is itself
        if (triggerTask == gameObject.transform.parent.gameObject)
        {
            if (Msg) Debug.Log("Set Difficultly " + gameObject.transform.parent.gameObject);

            // Retrieves difficulty
            float difficulty = triggerTask.GetComponent<TaskStatus>().difficulty;

            // Sets difficulty level (the number of switches in this case)
            numOfPressesNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f);

            // The number of keys needed cannot be zero
            numOfPressesNeeded = Mathf.Max(numOfPressesNeeded, minPossibleDifficultly);

            SetupTask();
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by onTaskFailed event only! <br />
    /// Resets the task back to its state just after SetupTask <br />
    /// has been called.
    /// </summary>
    void ResetSwitch(GameObject trigger)
    {
        // When the onTaskDifficultySet event is called, check whether the triggering gameobject is itself
        if (trigger == gameObject)
        {
            if (Msg) Debug.Log("Reset Task");

            // Reset the player's numbers shown
            typeInSeq = "";

            // Reset keys to show
            showInSeq = "";

            // Reset anything the player has entered
            ResetPlayerSequence();

            // Show Reset
            CheckCode();

            // If the code sequence is being shown cancel it
            if (myCoroutine != null)
            {
                // Stop showing code
                StopCoroutine(myCoroutine);
                myCoroutine = null;
            }

            // Showing starting message
            display.DisplayDefault();

            // The player can interact with the task
            canClickKeys = true;
        }
    }
}
