using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private int _cardIndex = 0;

    private PlayerController playerController;
    

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

    }


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            UIcardDisplayScript DisplayScript = eventData.pointerDrag.GetComponent<UIcardDisplayScript>();
            
            if (DisplayScript != null)
            {
                if (DisplayScript._cardIndex == _cardIndex && !DisplayScript._fromInventory) return;


                if (DisplayScript._fromInventory)
                {
                    if (DisplayScript._cardIndex >= playerController.InventoryCards.Count) return;

                    playerController.EquipInventoryCard(DisplayScript._cardIndex, _cardIndex);
                }
                else
                {
                    if (DisplayScript._cardIndex >= playerController.EquipedCards.Count) return;
                    playerController.SwapCard(DisplayScript._cardIndex, _cardIndex);

                }
            }
        }
    }

}
