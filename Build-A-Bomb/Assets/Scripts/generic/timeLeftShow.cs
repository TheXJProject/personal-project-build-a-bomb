using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class timeLeftShow : MonoBehaviour
{
    private void OnEnable()
    {
        if (GameManager.instance != null)
        {
            GetComponent<TextMeshProUGUI>().text = "Nice.I'm very proud of you. You had " + GameManager.instance.timeRemainingAfterWin + " seconds left";
        }
    }
}
