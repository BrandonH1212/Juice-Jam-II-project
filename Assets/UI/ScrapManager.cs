using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrapManager : MonoBehaviour
{
    [SerializeField] private GameObject _scrapPanel;
    [SerializeField] private Button _scrapButton;
    [SerializeField] private ScrapSlot _scrapSlot1;
    [SerializeField] private ScrapSlot _scrapSlot2;

    private PlayerController _playerController;

    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _scrapButton.onClick.AddListener(Scrap);
    }


    private void FixedUpdate()
    {
        if (_scrapSlot1._isLinked && _scrapSlot2._isLinked)
        {
            _scrapButton.interactable = true;
        }
        else
        {
            _scrapButton.interactable = false;
        }

    }
    
    public void Scrap()
    {
        if (_playerController.InventoryCards[_scrapSlot1._linkedCardIndex].Rarity == _playerController.InventoryCards[_scrapSlot2._linkedCardIndex].Rarity)
        {
            _scrapSlot1._isLinked = false;
            _scrapSlot2._isLinked = false;
            _scrapSlot1._linkedCardIndex = -1;
            _scrapSlot2._linkedCardIndex = -1;
            _scrapSlot1._linkedCardDisplayScript.gameObject.SetActive(false);
            _scrapSlot2._linkedCardDisplayScript.gameObject.SetActive(false);
            _scrapPanel.SetActive(false);
            CardRarity rarity = _playerController.InventoryCards[_scrapSlot1._linkedCardIndex].Rarity + 1;
            _playerController.InventoryCards.RemoveAt(_scrapSlot1._linkedCardIndex);
            _playerController.InventoryCards.RemoveAt(_scrapSlot2._linkedCardIndex);
            _playerController.InventoryCards.Add(CardDataManager.instance.GetRandomCard(rarity));

        }
    }

}
