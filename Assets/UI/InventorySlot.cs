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
        print("OnDrop");
        if (eventData.pointerDrag != null)
        {
            UIcardDisplayScript DisplayScript = eventData.pointerDrag.GetComponent<UIcardDisplayScript>();

            if (DisplayScript != null)
            {
                if (DisplayScript._cardIndex == _cardIndex) return;


                if (DisplayScript._fromInventory)
                {
                    if (DisplayScript._cardIndex >= playerController.InventoryCards.Count) return;
                    
                    playerController.SwapEquippedCardWithInventoryCard(_cardIndex, DisplayScript._cardIndex);
                }
                else
                {
                    if (DisplayScript._cardIndex >= playerController.EquipedCards.Count) return;
                    playerController.SwapEquippedCards(_cardIndex, DisplayScript._cardIndex);

                }
            }
        }
    }

}
