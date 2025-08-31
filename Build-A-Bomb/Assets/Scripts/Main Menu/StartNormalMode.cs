using UnityEngine;

public class StartNormalMode : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.instance.PlayNormalMode();
    }
}
