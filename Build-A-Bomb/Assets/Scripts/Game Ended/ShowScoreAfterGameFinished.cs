using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowScoreAfterGameFinished : MonoBehaviour
{
    private void OnEnable()
    {
        BeginVictoryScreenAnimatic.onVictoryScreenAnimaticStart += SetTextTimeLeft;
    }

    private void OnDisable()
    {
        BeginVictoryScreenAnimatic.onVictoryScreenAnimaticStart -= SetTextTimeLeft;
    }

    void SetTextTimeLeft()
    {
        GetComponent<TextMeshProUGUI>().text = GameManager.instance.timeRemainingAfterWin.ToString("N2");
    }
}
