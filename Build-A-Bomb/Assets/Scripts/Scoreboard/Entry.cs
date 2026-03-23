using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Entry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI posText;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void SetPosition(int position)
    {
        posText.text = position.ToString();
    }

    public void SetName(string username)
    {
        usernameText.text = username;
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
