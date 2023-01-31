using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIXpBar : MonoBehaviour
{
    private static UIXpBar instance;

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

    public UnityEvent onXPAdded;
    public float currentXP;
    private float maxXP = 1000f;
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
        //AddXP(100);
    }

    public void AddXP(float xp)
    {
        currentXP += xp;
        currentXP = Mathf.Clamp(currentXP, 0f, maxXP);
        _bar.rectTransform.localScale = new Vector3(currentXP / maxXP, 0.8f, 1);
        print(currentXP);
        onXPAdded.Invoke();
        if (currentXP == maxXP)
        {
            currentXP = 0;
        }
    }




}