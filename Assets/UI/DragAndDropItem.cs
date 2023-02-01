using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DragAndDropItem : MonoBehaviour, IDragHandler, IDropHandler
{
    public Vector3 originalPosition;


    public void Start()
    {
        originalPosition = transform.position;
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

    }

    public void OnDrop(PointerEventData eventData)
    {
        transform.position = originalPosition;
    }
    
}
