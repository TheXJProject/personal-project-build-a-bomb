using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTutorial : MonoBehaviour
{
    bool played = false;

    private void OnMouseDown()
    {
        GameManager.instance.PlayTutorial();

        if (!played)
        {
            played = true;

            if (Random.Range(0, 2) == 0)
            {
                //play sfx
                AudioManager.instance.PlaySFX("StartGame1", true, null, true);
            }
            else
            {
                //play sfx
                AudioManager.instance.PlaySFX("StartGame2", true, null, true);
            }
        }
    }
}
