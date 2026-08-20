using UnityEngine;
using UnityEngine.EventSystems;

public class NewMonoBehaviourScript : MonoBehaviour, IDropHandler
{
    public Level3Manager level3Manager;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableIngredient ingredient =
            eventData.pointerDrag.GetComponent<DraggableIngredient>();

        if (ingredient != null)
        {
            level3Manager.CheckIngredient(ingredient);
        }
    }
}
