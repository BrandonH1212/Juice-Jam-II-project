using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIStatDisplayScript : MonoBehaviour
{
    private Image Icon;
    private TMP_Text ValText;
    //private TooltipMouseOver _mouseOver;

    void Start()
    {
        Icon = GetComponentsInChildren<Image>()[1];
        ValText = GetComponentInChildren<TMP_Text>();
        /*
        _mouseOver = GetComponent<TooltipMouseOver>();
        if (_mouseOver == null)
        {
            _mouseOver = gameObject.AddComponent<TooltipMouseOver>();
        }
        */
    }

    public void SetStatDisplay(Sprite icon, float value, Stat stat)
    {
        Icon.sprite = icon;
        ValText.text = value.ToString();
        //_mouseOver.title = StatInfo.Data[stat].DisplayName;
        //_mouseOver.description = StatInfo.Data[stat].ToolTipDescription;
    }
}
