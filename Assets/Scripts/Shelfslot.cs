using UnityEngine;
using UnityEngine.EventSystems;

public class Shelfslot : MonoBehaviour, IDropHandler
{
    public int slotNumber;

    public Level5Manager level5Manager;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableCheese cheese =
            eventData.pointerDrag.GetComponent<DraggableCheese>();

        if (cheese != null)
        {
            level5Manager.CheckPosition(slotNumber);
        }
    }
}
