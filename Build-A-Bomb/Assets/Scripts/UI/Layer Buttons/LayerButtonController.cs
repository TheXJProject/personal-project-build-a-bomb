using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerButtonController : MonoBehaviour
{
    // Event Actions:
    public static event Action<GameObject> onLayerButtonSpawned;
    public static event Action<float> onNonInitialLayerButtonSpawned;

    // Inspector Adjustable Values:
    [SerializeField] GameObject layerButtonPrefab;
    [Range(0f, 500f)][SerializeField] float allButtonsHeight; // Makes sure that the buttons cannot be a certain percentage of the screen height away from the centre
    [Range(0f, 50f)][SerializeField] float gapHeight;     // Percent height of the screen that the gap height between buttons is

    // Runtime Variables
    List<GameObject> layerButtons = new List<GameObject>();
    float oldTopBottomHeight;
    float newTopBottomHeight;
    float amountToLowerButtons;
    bool firstLayerSpawned = false;

    private void OnEnable()
    {
        BombStatus.onLayerCreated += DetermineButtonSpawn;
    }

    private void OnDisable()
    {
        BombStatus.onLayerCreated -= DetermineButtonSpawn;
    }

    /// <summary>
    /// Spawns either the first layer button, or the next layer button
    /// </summary>
    void DetermineButtonSpawn(GameObject triggerLayer)
    {
        if (!firstLayerSpawned)
        {
            SpawnFirstButton(triggerLayer);
            firstLayerSpawned = true;
        }
        else
        {
            SpawnButton(triggerLayer);
        }
    }

    /// <summary>
    /// First button spawned initialises the space above and below the button
    /// </summary>
    void SpawnFirstButton(GameObject triggerLayer)
    {
        // Instantiate and set the button
        GameObject newButton = Instantiate(layerButtonPrefab, transform);
        SetButton(newButton, triggerLayer);

        // Determine the buttons height
        float h = layerButtonPrefab.GetComponent<RectTransform>().rect.height;
        newTopBottomHeight = (allButtonsHeight - h) / 2;

        // Set the button position and since there are no other buttons, it doesn't need to be affected by any "justSpawned" affects
        SetNewButtonPosition(newButton);
        newButton.GetComponent<LayerButtonMovement>().justSpawned = false;

        // Signal a layer button has been spawned
        onLayerButtonSpawned?.Invoke(triggerLayer);
    }

    /// <summary>
    /// Buttons spawned are added to list of buttons, then positioning is calculated before setting the button position
    /// </summary>
    void SpawnButton(GameObject triggerLayer)
    {
        GameObject newButton = Instantiate(layerButtonPrefab, transform);
        SetButton(newButton, triggerLayer);
        UpdateSpacingInformation();
        SetNewButtonPosition(newButton);
        onNonInitialLayerButtonSpawned?.Invoke(amountToLowerButtons);
        onLayerButtonSpawned?.Invoke(triggerLayer);
    }

    /// <summary>
    /// Button is added to button list and give correct information about its corresponding layer 
    /// </summary>
    void SetButton(GameObject newButton, GameObject triggerLayer) 
    {
        layerButtons.Add(newButton);
        newButton.GetComponent<LayerButtonPress>().correspondingLayer = triggerLayer;
        newButton.GetComponent<LayerButtonAppearance>().correspondingLayer = triggerLayer;
    }

    /// <summary>
    /// Button prefab is already set to be in the correct x position, so only the height needs to be worked out based on current spacing information
    /// </summary>
    void SetNewButtonPosition(GameObject newButton) 
    {
        float buttonHeight = layerButtonPrefab.GetComponent<RectTransform>().rect.height;
        float newSpawnHeight = (allButtonsHeight / 2) - newTopBottomHeight - (buttonHeight / 2); // The calculated distance from the top of the set max button height
        newButton.transform.localPosition = new Vector2(newButton.transform.localPosition.x, newSpawnHeight);
    }

    /// <summary>
    /// Space from the y region which tasks must be contained is calcualted based on a preset height and gap width
    /// </summary>
    void UpdateSpacingInformation()
    {
        oldTopBottomHeight = newTopBottomHeight;
        int n = layerButtons.Count;
        float h = layerButtonPrefab.GetComponent<RectTransform>().rect.height;
        newTopBottomHeight = (allButtonsHeight - ((n * h) + ((n - 1) * gapHeight))) / 2;
        amountToLowerButtons = oldTopBottomHeight - newTopBottomHeight;
    }
}
