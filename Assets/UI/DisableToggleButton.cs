using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableToggleButton : MonoBehaviour
{
    //this button disables and enables whatever  is in the toggle group
    [SerializeField] private Button _toggleButton;
    [SerializeField] private GameObject _toggleObj;

    void Start()
    {
        _toggleButton.onClick.AddListener(Toggle);

    }

    private void Toggle()
    {
        _toggleObj.SetActive(!_toggleObj.activeSelf);
    }
}
