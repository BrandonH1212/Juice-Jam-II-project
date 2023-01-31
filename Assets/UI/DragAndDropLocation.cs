using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropLocation : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<RectTransform>().SetParent(transform);
        eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
