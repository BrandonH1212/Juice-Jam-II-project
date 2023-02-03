using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIXpBar : MonoBehaviour
{
    private static UIXpBar instance;

    [SerializeField]
    private TMPro.TextMeshProUGUI _xpText = null;

    public static UIXpBar Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIXpBar>();
                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(UIXpBar) + " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }
    private Image _bar;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _bar = GetComponentsInChildren<Image>()[1];
        _bar.rectTransform.localScale = new Vector3(0f, 0.8f, 1);
        _xpText.text = "Level: " + "XP: " + "XX/XX";

    }

    public void UpdateBar(XpInfo _xpInfo)
    {
        _bar.rectTransform.localScale = new Vector3((float)_xpInfo.currentXP / (float)_xpInfo.XpRequired, 0.8f, 1);
        _xpText.text = "Level: " + _xpInfo.currentLevel + " XP: " + _xpInfo.currentXP + "/" + _xpInfo.XpRequired;
    }


}