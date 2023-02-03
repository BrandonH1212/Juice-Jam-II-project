using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableToggleButton : MonoBehaviour
{
    //this button disables and enables whatever  is in the toggle group
    [SerializeField] private Button _toggleButton;
    [SerializeField] private GameObject _toggleObj;

    [SerializeField] private List<GameObject> _hideOthers;

    private List<UIcardDisplayScript> _cardDisplays = new List<UIcardDisplayScript>();

    void Start()
    {
        _toggleButton.onClick.AddListener(Toggle);
        _cardDisplays = new List<UIcardDisplayScript>(_toggleObj.gameObject.GetComponentsInChildren<UIcardDisplayScript>());
        _toggleObj.SetActive(false);

    }

    private void Toggle()
    {
        _toggleObj.SetActive(!_toggleObj.activeSelf);

        if (_toggleObj.activeSelf)
        {
            Time.timeScale = 0.25f;
            foreach (UIcardDisplayScript cardDisplay in _cardDisplays)
            {
                //print(cardDisplay.gameObject.name);
                cardDisplay.timeSinceLastCheck = 0;
                cardDisplay.gameObject.SetActive(true);
            }
        }
        else
            { Time.timeScale = 1; }

        foreach (GameObject obj in _hideOthers)
        {
            obj.SetActive(false);
        }
    }
}
