using UnityEngine;
using UnityEngine.UI;

public class RefuelerLogic : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] Image dockImage;

    // Runtime Variables:
    bool isSetup;

    public void ResetFuelerAndDock()
    {
        // Set colours for this object


        // Set colours for dock
        dockImage.color = Color.red;
    }
}
