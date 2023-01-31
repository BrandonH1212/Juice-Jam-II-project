using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIcardDisplayScript : MonoBehaviour
{
    [SerializeField] private bool _fromInventory = false;
    [SerializeField] private int _cardIndex = 0;
    [SerializeField] private List<GameObject> StatDisplaySlots;
    [SerializeField] private List<GameObject> ModifierDisplaySlots;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;


    private GameObject _playerControllerObj;
    private PlayerController _ControlerScript;
    private CardBase _card;

    private void Start()
    {
        Invoke("SetUp", 0.1f);
    }

    private void HideDisplays()
    {
        foreach (GameObject slot in StatDisplaySlots) slot.SetActive(false);
        foreach (GameObject slot in ModifierDisplaySlots) slot.SetActive(false);

    }

    private void ShowStats()
    {
        HideDisplays();
        _title.text = _card.Info.cardName;
        _description.text = _card.Info.cardDescription;

        int index = 0;
        foreach (GameObject slot in StatDisplaySlots)
        {
            if (index >= _card.BaseValue.Count) break;

            UIStatDisplayScript displayScript = slot.GetComponent<UIStatDisplayScript>();

            if (displayScript != null)
            {
                slot.SetActive(true);
                Sprite sprite = StatInfo.Data[_card.BaseValue[index].StatToAffect].Icon;
                //print(_card.BaseValue[index].StatToAffect.ToString());
                displayScript.SetStatDisplay(sprite, Mathf.Round(_card.BaseValue[index].Value * 100f) / 100f, _card.BaseValue[index].StatToAffect); // Round to 2 decimal places WTF c#

            }
            index++;
        }

        index = 0;

        foreach (GameObject slot in ModifierDisplaySlots)
        {
            if (index >= _card.Modifiers.Count) break;

            UIModifierDisplayScript ModifierDisplayScript = slot.GetComponent<UIModifierDisplayScript>();

            if (ModifierDisplayScript != null)
            {
                slot.SetActive(true);
                Sprite sprite = StatInfo.Data[_card.BaseValue[index].StatToAffect].Icon;
                //print(_card.BaseValue[index].StatToAffect.ToString());
                ModifierDisplayScript.SetModifierDisplay(_card.Modifiers[index]);

            }
            index++;
        }
    }


            private void SetUp()
    {

        _playerControllerObj = GameObject.Find("Player");
        _ControlerScript = _playerControllerObj.GetComponent<PlayerController>();
        print(_ControlerScript);
        if (_fromInventory)
        {
            // not implemented...
        }
        else
        {
            _card = _ControlerScript.InitialCards[_cardIndex];
            print(_card.Info.cardName);
            ShowStats();
        }

    }



}
