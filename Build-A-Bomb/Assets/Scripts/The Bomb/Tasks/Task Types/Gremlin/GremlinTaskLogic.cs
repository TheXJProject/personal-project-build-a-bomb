using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GremlinTaskLogic : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Constant Values:
    const int maxPossibleDifficultly = 50;
    const int minPossibleDifficultly = 2;

    // Inspector Adjustable Values:
    [Range(minPossibleDifficultly, maxPossibleDifficultly)] public int currentHardestDifficulty;
    [SerializeField] [Range(0, 800)] float xSpawnRange;
    [SerializeField] [Range(0, 500)] float ySpawnRange;
    [SerializeField] float beginFadeTime = 0.1f;
    [SerializeField] float deathTime = 0.1f;
    [SerializeField] float pauseAfterTaskComplete = 0.1f;

    // Initialise In Inspector:
    [SerializeField] TaskInteractStatus statInteract;
    [SerializeField] GameObject gremlin;

    // Runtime Variables:
    int numOfHitsNeeded = minPossibleDifficultly;
    int numOfHits = 0;
    GameObject templin;
    bool isSetup;
    float pausedTime = 0;

    private void Awake()
    {
        // This instance is not set up yet
        isSetup = false;
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
        if (statInteract.isBeingSolvedAndSelected)
        {
            // Set the completion level
            statInteract.SetTaskCompletion((float)numOfHits / numOfHitsNeeded);

            // Check if task is completed
            if (numOfHits >= numOfHitsNeeded)
            {
                pausedTime += Time.deltaTime;

                if (pausedTime > pauseAfterTaskComplete)
                {
                    statInteract.TaskCompleted();
                }
            }
        }
    }

    /// <summary>
    /// Called by Gremlin gameobject. When the player <br />
    /// clicks on the Gremlin the remaining number of <br />
    /// times the player needs to click is reduced by one and the gremlin moves randomly.
    /// </summary>
    public void GremlinHit()
    {
        if (Msg) Debug.Log("Called function");

        // Checks if the task can be solved
        if (statInteract.isBeingSolvedAndSelected)
        {
            // Increases the total number of times Gremlin has been hit by one
            numOfHits++;

            // If not completed, destroy gremlin
            StartCoroutine(GremlinDeath(templin));

            // Check if task is completed
            if (numOfHits < numOfHitsNeeded)
            {
                // Then randomly put gremlin somewhere on the screen
                Vector3 randomPosition = new();
                randomPosition.x = Random.Range(0, xSpawnRange) - (xSpawnRange / 2f);
                randomPosition.y = Random.Range(0, ySpawnRange) - (ySpawnRange / 2f);
                randomPosition.z = gremlin.transform.localPosition.z;

                // Create temp gremlin
                templin = Instantiate(gremlin, Vector2.zero, Quaternion.identity, transform.GetChild(0).transform);

                // Place the gremlin
                templin.transform.localPosition = randomPosition;
                templin.transform.SetAsLastSibling();
            }
        }
    }

    IEnumerator GremlinDeath(GameObject gremer)
    {
        // TODO: add your animations stuff booooy
        GremlinHit gremerScript = gremer.GetComponent<GremlinHit>();
        gremer.GetComponent<Image>().sprite = gremerScript.splatSprite;
        gremerScript.mallet.SetActive(true);
        gremerScript.shadow.SetActive(false);
        gremer.GetComponent<RectTransform>().sizeDelta = new Vector2(112, 112);

        float elapsed = 0f;
        Color startColor = gremer.GetComponent<Image>().color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        yield return new WaitForSeconds(beginFadeTime);
        while (elapsed < deathTime)
        {
            elapsed += Time.deltaTime;
            gremer.GetComponent<Image>().color = Color.Lerp(startColor, endColor, elapsed / deathTime);
            yield return null; 
        }

        gremer.GetComponent<Image>().color = endColor; 

        Destroy(gremer);
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
            // Randomly spawn a gremlin on the screen
            Vector3 randomPosition = new();
            randomPosition.x = Random.Range(0, xSpawnRange) - (xSpawnRange / 2f);
            randomPosition.y = Random.Range(0, ySpawnRange) - (ySpawnRange / 2f);
            randomPosition.z = gremlin.transform.localPosition.z;

            // Create temp gremlin
            templin = Instantiate(gremlin, Vector2.zero, Quaternion.identity, transform.GetChild(0).transform);

            // Place the gremlin
            templin.transform.localPosition = randomPosition;

            pausedTime = 0;

            // This instance is now setup
            isSetup = true;
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
            numOfHitsNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f);

            // The number of hits needed cannot be zero
            numOfHitsNeeded = Mathf.Max(numOfHitsNeeded, minPossibleDifficultly);

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

            // If not completed, destroy gremlin
            Destroy(templin);

            // Then randomly put gremlin somewhere on the screen
            Vector3 randomPosition = new();
            randomPosition.x = Random.Range(0, xSpawnRange) - (xSpawnRange / 2f);
            randomPosition.y = Random.Range(0, ySpawnRange) - (ySpawnRange / 2f);
            randomPosition.z = gremlin.transform.localPosition.z;

            // Create temp gremlin
            templin = Instantiate(gremlin, Vector2.zero, Quaternion.identity, transform.GetChild(0).transform);

            // Place the gremlin
            templin.transform.localPosition = randomPosition;
            templin.transform.SetAsLastSibling();

            // Reset the number of times the player has hit
            numOfHits = 0;

            pausedTime = 0;

            // Set the completion level
            statInteract.SetTaskCompletion((float)numOfHits / numOfHitsNeeded);
        }
    }
}
