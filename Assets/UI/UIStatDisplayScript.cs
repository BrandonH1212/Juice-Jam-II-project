using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIStatDisplayScript : MonoBehaviour
{
    [SerializeField] private Image Icon;
    [SerializeField] private TMP_Text Value;

    public void SetStatDisplay(Sprite icon, float value)
    {
        Icon.sprite = icon;
        Value.text = value.ToString();
    }
}
