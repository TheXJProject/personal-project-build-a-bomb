using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTutorial : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.instance.PlayTutorial();
    }

}
