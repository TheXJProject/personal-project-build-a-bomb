using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Leaderboards;
using UnityEngine;

public class GetAndShowScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI position;

    private void Start()
    {
        position.text = ((int)(GameManager.instance.timeRemainingAfterWin * 100)).ToString();
    }
}
