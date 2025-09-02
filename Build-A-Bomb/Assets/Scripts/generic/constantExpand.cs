using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class constantExpand : MonoBehaviour
{
    [SerializeField] GameObject[] rings;
    [SerializeField] float minScale;
    [SerializeField] float scalingSpeed;
    [SerializeField] float spawnSpeed;

    float timer = 0;
    int nextRingToSpawn = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnSpeed)
        {
            timer = 0;
            rings[nextRingToSpawn].SetActive(true);
            rings[nextRingToSpawn].transform.localScale = new Vector2 (minScale, minScale);
            ++nextRingToSpawn;
            nextRingToSpawn = nextRingToSpawn % rings.Length;
        }

        // Increase scale of both transforms given
        float increaseAmount = 1+(scalingSpeed * Time.deltaTime);
        foreach (var ring in rings)
        {
            ring.transform.localScale = ring.transform.localScale * increaseAmount;
        }
    }
}
