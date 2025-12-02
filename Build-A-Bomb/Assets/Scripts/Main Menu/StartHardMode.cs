using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHardMode : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.instance.PlayHardMode();
    }

}
