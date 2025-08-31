using UnityEngine;
using UnityEngine.EventSystems;

public class GremlinHit : MonoBehaviour
{
    public void GremlinGotHit(BaseEventData data)
    {
        PointerEventData newData = (PointerEventData)data;
        if (newData.button.Equals(PointerEventData.InputButton.Left))
        {
            // If gremlin clicked on tell main script
            GremlinTaskLogic parentScript = GetComponentInParent<GremlinTaskLogic>();
            parentScript.GremlinHit();
        }
    }
}