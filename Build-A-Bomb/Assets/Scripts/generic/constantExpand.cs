using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class constantExpand : MonoBehaviour
{
    [SerializeField] Transform bits1;
    [SerializeField] Transform bits2;
    [SerializeField] Transform bits3;
    [SerializeField] float minScale;
    [SerializeField] float midScale1;
    [SerializeField] float midScale2;
    [SerializeField] float scalingSpeed;
    [SerializeField] float spawnSpeed;

    float timer = 0;
    int currentBiggest = 2;

    private void Start()
    {
        bits1.localScale = new Vector2(midScale1, midScale1);
        bits2.localScale = new Vector2(minScale, minScale); 
        bits3.localScale = new Vector2(midScale2, midScale2);
    }
    void Update()
    {
        // If the size of one of the backgrounds increases beyond max size, reset it to min size
        // if (bits1.localScale.x > maxScale) bits1.localScale = new Vector2(minScale, minScale);
        // else if (bits2.localScale.x > maxScale) bits2.localScale = new Vector2(minScale, minScale);
        // else if (bits3.localScale.x > maxScale) bits3.localScale = new Vector2(minScale, minScale);

        timer += Time.deltaTime;
        if (timer > spawnSpeed)
        {
            timer = 0;
            switch (currentBiggest)
            {
                case 0:
                    bits1.localScale = new Vector2(minScale, minScale);
                    break;
                case 1:
                    bits2.localScale = new Vector2(minScale, minScale);
                    break;
                case 2:
                    bits3.localScale = new Vector2(minScale, minScale);
                    break;
                default:
                    break;
            }
            currentBiggest = (++currentBiggest) % 3;
        }

        // Increase scale of both transforms given
        float increaseAmount = 1+(scalingSpeed * Time.deltaTime);
        bits1.localScale = bits1.localScale * increaseAmount;
        bits3.localScale = bits3.localScale * increaseAmount;
        bits2.localScale = bits2.localScale * increaseAmount;
    }
}
