using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateGameEndScreen : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
    }
}
