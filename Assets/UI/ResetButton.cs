using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _scrapSlot1;

    [SerializeField]
    private GameObject _scrapSlot2;


    private ScrapSlot _scrapScript1;
    private ScrapSlot _scrapScript2;
    private Button _button;
    void Start()
    {
        _scrapScript1 = _scrapSlot1.GetComponent<ScrapSlot>();
        _scrapScript2 = _scrapSlot2.GetComponent<ScrapSlot>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ResetSlots);

    }

    private void ResetSlots()
    {
        _scrapScript1._isLinked = false;
        _scrapScript2._isLinked = false;
        _scrapScript1._linkedCardDisplayScript.gameObject.SetActive(false);
        _scrapScript2._linkedCardDisplayScript.gameObject.SetActive(false);
    }

}
