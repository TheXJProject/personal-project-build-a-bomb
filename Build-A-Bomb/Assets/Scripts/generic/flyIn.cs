using System.Collections;
using UnityEngine;

public class flyIn : MonoBehaviour
{
    [SerializeField] float flyInStartSpeed;
    [SerializeField] float flyInEndSpeed;

    [SerializeField] float offScreenYStart;
    [SerializeField] float offScreenXStart;

    public bool canFlyIn = true;

    Coroutine flyingIn;
    Vector2 targetPos;
    float curFlyInSpeed;
    float disToPos;
    float StartToPos;
    private void Awake()
    {
        targetPos = GetComponent<RectTransform>().position;
    }
    private void OnEnable()
    {
        if (canFlyIn)
        {
            canFlyIn = false;
            Vector2 startPos = new Vector2(Random.Range(-offScreenXStart, offScreenXStart), Random.Range(-offScreenYStart, offScreenYStart));
            GetComponent<RectTransform>().position = startPos;
            StartToPos = Vector3.Distance(GetComponent<RectTransform>().position, targetPos);
            disToPos = StartToPos;

            if (flyingIn != null) StopCoroutine(flyingIn);
            flyingIn = StartCoroutine(FlyToTarget());
        }
    }

    IEnumerator FlyToTarget()
    {
        while (disToPos > 0.01f)
        {
            disToPos = Vector3.Distance(GetComponent<RectTransform>().position, targetPos);
            curFlyInSpeed = ((disToPos / StartToPos) * (flyInStartSpeed - flyInEndSpeed)) + flyInEndSpeed;
            GetComponent<RectTransform>().position = Vector3.MoveTowards(GetComponent<RectTransform>().position, targetPos, curFlyInSpeed * Time.deltaTime);

            yield return null;
        }

        GetComponent<RectTransform>().position = targetPos;

    }
}
