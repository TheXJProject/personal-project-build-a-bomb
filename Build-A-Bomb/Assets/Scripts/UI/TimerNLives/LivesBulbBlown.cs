using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesBulbBlown : MonoBehaviour
{
    public static event Action onBulbBlown;

    public void bulbBlown()
    {
        onBulbBlown?.Invoke();
    }
}
