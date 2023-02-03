using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardDataManager : MonoBehaviour
{
    public static CardDataManager instance;
    public RarityData[] rarityData;
    private Dictionary<CardRarity, RarityData> rarityDataDictionary;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        rarityDataDictionary = new Dictionary<CardRarity, RarityData>();
        foreach (var data in rarityData)
        {
            rarityDataDictionary[data.rarity] = data;
        }
    }

    public RarityData GetRarityData(CardRarity rarity)
    {
        return rarityDataDictionary[rarity];
    }

    public CardBase GetRandomCard(CardRarity rarity)
    {
        var rarityData = GetRarityData(rarity);
        return rarityData.rarityCards[Random.Range(0, rarityData.rarityCards.Count)];
    }
}