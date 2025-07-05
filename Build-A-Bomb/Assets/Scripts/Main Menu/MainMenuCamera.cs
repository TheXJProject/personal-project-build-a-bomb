using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    // Inspector Adjustable Values:
    [Range(0, 10)] float exampleAdjustable;

    public Vector2 settings;
    public float openingCameraSize;
    [SerializeField] float nextCameraSize;
    [SerializeField] float SettingsCameraSize, settingsYPos;
}
