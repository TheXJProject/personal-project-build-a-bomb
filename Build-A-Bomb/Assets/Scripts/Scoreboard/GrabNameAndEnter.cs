using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrabNameAndEnter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private SubmitChoice submit;

    public void SubmitText()
    {
        submit.PlayerEnteredName(inputText.text);
    }
}
