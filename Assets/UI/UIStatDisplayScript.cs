using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class UIStatDisplayScript : MonoBehaviour
{
    private Image Icon;
    private TMP_Text ValText;
    private ToolipMouseOver _mouseOver;


    void Start()
    {
        Icon = GetComponentsInChildren<Image>()[1];
        ValText = GetComponentInChildren<TMP_Text>();
        _mouseOver = GetComponent<ToolipMouseOver>();

    }

    public void SetStatDisplay(Sprite icon, float value, Stat Stat)
    {
        Icon.sprite = icon;
        ValText.text = value.ToString();
        _mouseOver.title = StatInfo.Data[Stat].DisplayName;
        _mouseOver.description = StatInfo.Data[Stat].ToolTipDescription;
    }
}
