using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PageController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _cards = new List<GameObject>();
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private TextMeshProUGUI _pageText;
    private int _currentPage = 0;
    
    void Start()
    {
        _nextButton.onClick.AddListener(NextPage);
        _previousButton.onClick.AddListener(PreviousPage);
        _pageText.text = "Page " + (_currentPage + 1) + " of " + _cards.Count;
        _cards[_currentPage].SetActive(true);

    }

    private void UpdateCardsIndex()
    {
        int index = 0;
        foreach (GameObject card in _cards)
        {
            card.SetActive(true);
            UIcardDisplayScript _card = card.GetComponent<UIcardDisplayScript>();
            _card._cardIndex = index + (_currentPage * _cards.Count);
            index++;
        }
    }


    private void NextPage()
    {
        _currentPage++;
        if (_currentPage >= _cards.Count)
        {
            _currentPage = 0;
        }
        
        UpdateCardsIndex();
        
        _pageText.text = "Page " + (_currentPage + 1) + " of " + _cards.Count;
    }

    private void PreviousPage()
    {
        _currentPage--;
        if (_currentPage < 0)
        {
            _currentPage = _cards.Count - 1;
        }
        
        UpdateCardsIndex();
        
        _pageText.text = "Page " + (_currentPage + 1) + " of " + _cards.Count;
        
    }


}
