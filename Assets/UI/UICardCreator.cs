using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;


public class UICardCreator : MonoBehaviour
{

    private Label _titleText;
    private Label _description;
    private UIDocument _rootUi;

    [SerializeField]
    private VisualTreeAsset _cardasset = null;

    [SerializeField]
    private CardBase _card = null;
    
    void Start()
    {
        _rootUi = GetComponent<UIDocument>();
        // add the card view to the root ui
        _cardasset.CloneTree(_rootUi.rootVisualElement);
        // get the title and description text elements
        _titleText = _rootUi.rootVisualElement.Q<Label>("card-title");
        _description = _rootUi.rootVisualElement.Q<Label>("card-description");
        print(_titleText);
        UpdateUi();


    }

    private void UpdateUi()
    {
        if (_card != null)
        {
            _titleText.text = _card.Info.cardName;
            _description.text = _card.Info.cardDescription;
        }
    }


}
