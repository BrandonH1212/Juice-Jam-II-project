using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UiToolTip : MonoBehaviour
{
    private TextMeshProUGUI _title;
    TextMeshProUGUI _description;

    private void Awake()
    {
        _title = GetComponentsInChildren<TextMeshProUGUI>()[0];
        _description = GetComponentsInChildren<TextMeshProUGUI>()[1];
        HideToolTip();
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out localPoint);
        transform.localPosition = localPoint;

    }
    
    public void ShowToolTip(string title, string description)
    {
        gameObject.SetActive(true);
        _title.text = title;
        _description.text = description;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}