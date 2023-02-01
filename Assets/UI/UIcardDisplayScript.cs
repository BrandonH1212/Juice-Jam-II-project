using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIcardDisplayScript : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] public bool _fromInventory = false;
    [SerializeField] public int _cardIndex = 0;
    [SerializeField] private List<GameObject> StatDisplaySlots;
    [SerializeField] private List<GameObject> ModifierDisplaySlots;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;

    [SerializeField] private bool _isDragable = true;
    
    private GameObject _playerControllerObj;
    private PlayerController _ControlerScript;
    private CanvasGroup _canvasGroup;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    public CardBaseInstance _card;
    public CardBase _cardBase;
    private Vector3 originalPosition;
    
    

    private void Start()
    {
        _playerControllerObj = GameObject.Find("Player");
        _ControlerScript = _playerControllerObj.GetComponent<PlayerController>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvas = GetComponentInParent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        Invoke("SetUp", 1);
    }

    private void HideDisplays()
    {
        foreach (GameObject slot in StatDisplaySlots) slot.SetActive(false);
        foreach (GameObject slot in ModifierDisplaySlots) slot.SetActive(false);

    }

    private void ShowStats()
    {
        HideDisplays();
        _title.text = _cardBase.Info.cardName;
        _description.text = _cardBase.Info.cardDescription;

        List<StatFloatPair> values = new List<StatFloatPair>();
        if (_card != null)
        {
            foreach (KeyValuePair < Stat,float> item  in _card._Stats)
            {
                if (item.Value != 0f)
                {
                    StatFloatPair toAdd = new();
                    toAdd.StatToAffect = item.Key;
                    toAdd.Value = item.Value;
                    values.Add(toAdd);

                }
            }
        }
        else values = _cardBase.BaseValue;


        int index = 0;
        foreach (GameObject slot in StatDisplaySlots)
        {
            if (index >= values.Count) break;

            UIStatDisplayScript displayScript = slot.GetComponent<UIStatDisplayScript>();
            if (displayScript != null)
            {
                slot.SetActive(true);
                Sprite sprite = StatInfo.Data[values[index].StatToAffect].Icon;
                //print(_card.BaseValue[index].StatToAffect.ToString());
                float statValue = 0;

                if (_card != null) statValue = _card._Stats[values[index].StatToAffect];
                else statValue = values[index].Value;

                if (statValue != 0)
                {
                    displayScript.SetStatDisplay(sprite, Mathf.Round(statValue * 100f) / 100f, values[index].StatToAffect); // Round to 2 decimal places WTF c#
                }
            }
            index++;
        }

        index = 0;

        foreach (GameObject slot in ModifierDisplaySlots)
        {
            if (index >= _cardBase.Modifiers.Count) break;

            UIModifierDisplayScript ModifierDisplayScript = slot.GetComponent<UIModifierDisplayScript>();

            if (ModifierDisplayScript != null)
            {
                slot.SetActive(true);
                Sprite sprite = StatInfo.Data[_cardBase.Modifiers[index].StatToAffect].Icon;
                //print(_card.BaseValue[index].StatToAffect.ToString());
                ModifierDisplayScript.SetModifierDisplay(_cardBase.Modifiers[index]);

            }
            index++;
        }
    }

    private void SetUp()
    {

        gameObject.SetActive(true);
        if (!_fromInventory && _cardIndex >= _ControlerScript.EquipedCards.Count)
        {
            gameObject.SetActive(false);
            return;
        }
        if (_fromInventory && _cardIndex >= _ControlerScript.InventoryCards.Count)
        {
            gameObject.SetActive(false);
            return;
        }

        if (_fromInventory)
        {
            _card = null;
            _cardBase = _ControlerScript.InventoryCards[_cardIndex];
        }
        else
        {
            _card = _ControlerScript.EquipedCards[_cardIndex];
            _cardBase = _card.CardBase;
        }

        ShowStats();
    }

    private float timeSinceLastCheck;

    void Update()
    {
        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck >= 0.1f)
        {
            timeSinceLastCheck = 0f;
            SetUp();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = originalPosition;
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        transform.position = Input.mousePosition * _canvas.scaleFactor + new Vector3(0, 0, 10);


    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        _canvasGroup.alpha = .6f;
        _canvasGroup.blocksRaycasts = false;
    }
}