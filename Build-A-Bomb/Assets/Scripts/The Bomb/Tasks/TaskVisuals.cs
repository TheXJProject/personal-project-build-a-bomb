using System.Collections;
using UnityEngine;

public class TaskVisuals : MonoBehaviour
{
    [SerializeField] Material xRayFinished;
    [SerializeField] Material xRayUnfinished;
    [SerializeField] Material xRayWorking;
    [SerializeField] SpriteRenderer OuterRing;
    [SerializeField] SpriteRenderer InnerRing;
    [SerializeField] SpriteRenderer InsideInnerRing;
    [SerializeField] SpriteRenderer symbol;
    [SerializeField] SpriteRenderer centreXRay;
    [SerializeField] GameObject InsideInnerRingObject;

    Color normalColourInner;
    Color normalColourOuter;
    [SerializeField] Color beingSolvedColour = Color.yellow;
    [SerializeField] Color solvedColour = Color.green;
    [SerializeField] Color goneWrongColour = Color.red;
    [SerializeField] Color lightsOutColour = Color.black;

    [SerializeField] float timePureWhite;
    [SerializeField] float timePopping;
    [SerializeField] float endScale;
    [SerializeField] Color finalPingColour;
    [SerializeField] Color finalInnerPingColour;

    Coroutine lightsOutWait;
    [HideInInspector] public int taskLayer;
    [HideInInspector] public int taskNumberInLayer;
    [HideInInspector] public int numberTasksInLayer;

    private void Awake()
    {
        normalColourOuter = OuterRing.color; // Normal colour is set to whatever the colour is when the games starts
        normalColourInner = InnerRing.color; // Normal colour is set to whatever the colour is when the games starts
    }

    private void OnEnable()
    {
        TaskStatus.onTaskBegan += TaskBeingSolved;
        TaskStatus.onTaskFailed += TaskFailed;
        TaskStatus.onTaskCompleted += TaskCompleted;
        TaskStatus.onTaskGoneWrong += TaskGoneWrong;

        DisableAndEnableForEndGame.onLightsOut += LightsOut;
    }

    private void OnDisable()
    {
        TaskStatus.onTaskBegan -= TaskBeingSolved;
        TaskStatus.onTaskFailed -= TaskFailed;
        TaskStatus.onTaskCompleted -= TaskCompleted;
        TaskStatus.onTaskGoneWrong -= TaskGoneWrong;

        DisableAndEnableForEndGame.onLightsOut -= LightsOut;
    }


    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.parent.transform.rotation.z * -1.0f);
    }

    void TaskBeingSolved(GameObject task) // Sets the colour for when the task is being solved
    {
        if (task != gameObject) { return; }
        OuterRing.color = beingSolvedColour; // TODO: change this to effect it with xray coding
        GetComponent<SpriteRenderer>().material = xRayWorking;
    }

    void TaskFailed(GameObject task) // Sets the tasks colour back to the orignal colour for when the task went wrong
    {
        if (task != gameObject) { return; }
        GetComponent<SpriteRenderer>().material = xRayUnfinished;

        if (task.GetComponent<TaskStatus>().isGoingWrong) 
        {
            OuterRing.color = goneWrongColour; // TODO: change this to effect it with xray coding
        }
        else
        {
            OuterRing.color = normalColourOuter; // TODO: change this to effect it with xray coding
        }
    }

    void TaskCompleted(GameObject task) // Sets the colour for when the task is completed
    {
        if (task != gameObject) { return; }
        OuterRing.color = solvedColour; // TODO: change this to effect it with xray coding
        GetComponent<SpriteRenderer>().material = xRayFinished;
    }

    void TaskGoneWrong(GameObject task) // Sets the colour for when the task goes wrong
    {
        if (task != gameObject) { return; }
        OuterRing.color = goneWrongColour; // TODO: change this to effect it with xray coding
        GetComponent<SpriteRenderer>().material = xRayUnfinished;
    }


    void LightsOut(float maxRandWaitTime)
    {
        float maxLay = GameManager.instance.currentLayer + 1.0f;
        float percentThroughThisLayer = (((taskLayer + 1) / maxLay) - (taskLayer / maxLay)) * ((float)taskNumberInLayer / numberTasksInLayer);
        float waitTime = maxRandWaitTime * ((taskLayer / maxLay) + percentThroughThisLayer);
        if (lightsOutWait != null) StopCoroutine(lightsOutWait);
        lightsOutWait = StartCoroutine(LightsOutWait(waitTime));
    }

    IEnumerator LightsOutWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        OuterRing.enabled = false;
        symbol.enabled = false;
        centreXRay.enabled = false;
        yield return new WaitForSeconds(timePureWhite);

        Color colorInner = finalPingColour;
        Color colorXRay = finalInnerPingColour;
        Vector3 endScaleVector = new Vector3(endScale, endScale, 1);
        float time = 0f;
        while (time < timePopping)
        {
            time += Time.deltaTime;
            float t = time / timePopping;
            colorInner.a = Mathf.Lerp(1, 0, t);
            colorXRay.a = Mathf.Lerp(1, 0, t);
            InsideInnerRing.color = colorInner;
            InnerRing.color = colorXRay;
            InsideInnerRingObject.transform.localScale = Vector3.Lerp(Vector3.one, endScaleVector, Mathf.Sqrt(t));

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
