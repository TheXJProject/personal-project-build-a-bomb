using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bolt : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Inspector Adjustable Values:
    public float boltTime = 1;
    [SerializeField] GameObject boltVisualOutline;
    [SerializeField] GameObject boltVisualInnerEdge;
    [SerializeField] GameObject boltVisualInner;
    [SerializeField] float boltSpinSpeed;
    [SerializeField] float boltSpinSpeedVariance;
    [SerializeField] float finalBoltScale;
    [SerializeField] float finalBoltInnerScale;

    [SerializeField] Color boltOutCol;
    [SerializeField] Color boltOutEdgeCol;
    [SerializeField] Color boltWindingCol;
    [SerializeField] Color boltWindingEdgeCol;
    [SerializeField] Color boltInCol;
    [SerializeField] Color boltInEdgeCol;
    [SerializeField] float startBoltScale;
    [SerializeField] float startBoltInnerScale;

    //public flyIn flyInLogic;
    // Runtime Variables:
    [HideInInspector] public bool complete;
    bool boltingInProgress = false;
    float timeElapsed = 0f;
    BoltingLogic mainLogic;
    float spinSpeedAfterVariance;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // This instance is not complete when just spawned
        complete = false;
        boltingInProgress = false;
        timeElapsed = 0f;
        mainLogic = gameObject.transform.parent.parent.parent.parent.GetComponent<BoltingLogic>();
        spinSpeedAfterVariance = boltSpinSpeed + Random.Range(-boltSpinSpeedVariance, boltSpinSpeedVariance);
    }

    private void OnEnable()
    {
        // If the bolt is not complete, when re-entering the task reset the bolt
        if (!complete)
        {
            ResetBolt();
        }
    }

    private void Update()
    {
        // Check if the left button is pressed initially
        if (Input.GetMouseButton(0) && boltingInProgress && mainLogic.statInteract.isBeingSolvedAndSelected)
        {
            // TODO: Replace with call for animation!
            boltVisualInnerEdge.GetComponent<Image>().color = boltWindingEdgeCol;
            boltVisualInner.GetComponent<Image>().color = boltWindingCol;
            
            boltVisualOutline.transform.Rotate(Vector3.forward, spinSpeedAfterVariance * Time.deltaTime);

            // Wait for set amount of time, checking if the button stays pressed
            if (timeElapsed < boltTime)
            {
                // Increment the time elapsed and continue
                timeElapsed += Time.deltaTime;
                float newBoltScale = (((boltTime - timeElapsed) / boltTime) * (startBoltScale - finalBoltScale)) + finalBoltScale;
                float newBoltInnerScale = (((boltTime - timeElapsed) / boltTime) * (startBoltInnerScale - finalBoltInnerScale)) + finalBoltInnerScale;
                boltVisualOutline.transform.localScale = new Vector2(newBoltScale, newBoltScale);
                boltVisualInnerEdge.transform.localScale = new Vector2(newBoltInnerScale, newBoltInnerScale);
            }
            else
            {
                if (Msg) Debug.Log("Bolt completed.");

                // After a set amount of time, if the button is still held, log success
                complete = true;
                boltingInProgress = false;

                // Play each sound, non-priority, using default volume, with random pitch
                //AudioManager.instance.PlaySFX("Finished Bolt", true, null, true);
                AudioManager.instance.PlaySFX("Finished Bolt", true, null, true);
                //AudioManager.instance.PlaySFX("Finished Bolting", false, null, true);

                // TODO: Replace with call for animation!
                boltVisualInnerEdge.GetComponent<Image>().color = boltInEdgeCol;
                boltVisualInner.GetComponent<Image>().color = boltInCol;
                boltVisualOutline.transform.localScale = new Vector2(finalBoltScale, finalBoltScale);
                boltVisualInnerEdge.transform.localScale = new Vector2(finalBoltInnerScale, finalBoltInnerScale);

                // Call check complete function
                mainLogic.CheckIfComplete();
            }
        }
        else
        {
            if (boltingInProgress && !complete)
            {
                // Stopped bolting
                AudioManager.instance.PlaySFX("Finished Bolting", false, null, true);
            }
            
            // If the player lets go of the bolt and it is not complete
            if (!complete)
            {
                // Reset the bolt
                ResetBolt();
            }
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by the Bolt gameobject. When the player holds <br />
    /// left click on the Bolt a timer starts. <br />
    /// </summary>
    public void CompleteBolt(BaseEventData data)
    {
        // Check if the task can be solved
        if (mainLogic.statInteract.isBeingSolvedAndSelected && !complete)
        {
            PointerEventData newData = (PointerEventData)data;
            // Check left click is pressed
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                if (Msg) Debug.Log("Bolting in!");

                // Play each sound, non-priority, using default volume, with random pitch
                AudioManager.instance.PlaySFX("Start Bolting", true, null, true);

                // Start checking that left click is held
                boltingInProgress = true;
            }
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Resets the Bolt to uncomplete. <br />
    /// </summary>
    public void ResetBolt()
    {
        
        if (Msg) Debug.Log("Bolt Reset.");
        
        // This bolt is reset
        complete = false;
        boltingInProgress = false;
        timeElapsed = 0;

        // TODO: Replace with call for animation!
        boltVisualInnerEdge.GetComponent<Image>().color = boltOutEdgeCol;
        boltVisualInner.GetComponent<Image>().color = boltOutCol;
        boltVisualInnerEdge.transform.localScale = new Vector2(startBoltInnerScale, startBoltInnerScale);
        boltVisualOutline.transform.localScale = new Vector2(startBoltScale, startBoltScale);
        boltVisualOutline.transform.rotation = Quaternion.identity;
    }
}
