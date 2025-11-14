using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMangerFinishLoadAnim : MonoBehaviour
{
    void FinishedLoadAnimation()
    {
        GameManager.instance.waitForAnimation = false;
    }
}
