using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesBulbBlown : MonoBehaviour
{
    public static event Action onFuseBlown;

    public void bulbBlown()
    {
        onFuseBlown?.Invoke();
    }
}
