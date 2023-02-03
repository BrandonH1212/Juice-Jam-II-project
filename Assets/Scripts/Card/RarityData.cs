using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RarityData", menuName = "Card Rarity Data")]
public class RarityData : ScriptableObject
{
    public CardRarity rarity;
    public Color rarityColor;
    public Color raritySubColor;
    public List<CardBase> rarityCards;
}