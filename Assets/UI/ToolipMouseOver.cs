using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolipMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UiToolTip _toolTip;
    private Button _button;

    public string title;
    public string description;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _toolTip = FindObjectOfType<UiToolTip>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _toolTip.ShowToolTip(title, description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _toolTip.HideToolTip();
    }



}
