using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrapSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private int _slot = 0;

    public UIcardDisplayScript _linkedCardDisplayScript;
    

    public bool _isLinked = false;
    public int _linkedCardIndex  = 0;
    

    private void Start()
    {
        _linkedCardDisplayScript = GetComponentInChildren<UIcardDisplayScript>();
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            UIcardDisplayScript DisplayScript = eventData.pointerDrag.GetComponent<UIcardDisplayScript>();

            if (DisplayScript != null)
            {
                _linkedCardIndex = DisplayScript._cardIndex;
                _linkedCardDisplayScript._cardIndex = _linkedCardIndex;
                _linkedCardDisplayScript.gameObject.SetActive(true);
                _isLinked = true;
            }
        }
    }
}

