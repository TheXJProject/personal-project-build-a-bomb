using UnityEngine;
using UnityEngine.EventSystems;

public class GremlinHit : MonoBehaviour
{
    // The gremlin starts not hit
    bool hit = false;
    public void GremlinGotHit(BaseEventData data)
    {
        // If this gremlin is not hit yet
        if (!hit)
        {
            PointerEventData newData = (PointerEventData)data;
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                // If gremlin clicked on tell main script
                GremlinTaskLogic parentScript = GetComponentInParent<GremlinTaskLogic>();
                parentScript.GremlinHit();
                
                // The gremlin is now hit
                hit = true;
            }
        }
    }
}