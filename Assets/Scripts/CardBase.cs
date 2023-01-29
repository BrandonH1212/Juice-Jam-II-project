using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum CardRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public enum ModiferTarget
{
    All,         // modifiers get added to all other cards 
    AllLeft,     // modifiers get added to all cards to the left of this card
    AllRight,    // modifiers get added to all cards to the right of this card
    Left,        // modifiers get added to the card to the left of this card
    Right,       // modifiers get added to the card to the right of this card
    Adjacent,    // modifiers get added to the card to the left and right of this card
    Notadjacent  // modifiers get added to all cards except the card to the left and right of this card
}

[System.Serializable]
public struct CardInfo
{
    public string cardName;
    public string cardDescription;
    public CardRarity cardRarity;
    public Sprite cardIcon;
}

public enum Stat 
{
    TargetType,
    FirePattern,
    Damage,
    ProjectileSpeed,
    ProjectileSize,
    FireRate,
    Penetration,
    PeriodicDamage,
    PeriodicInterval,
    PeriodicCount,
    SplashDamage,
    SplashRadius,
    CriticalChance,
    CriticalDamage,
    LifeSteal,
}

public enum EffectOperator 
{
    Summation,
    Multiplication
}

[System.Serializable]
public struct Modifier
{
    public ModiferTarget Targets;
    public Stat StatToAffect;
    public EffectOperator Operator;
    public float Value;
}

[Serializable]
public struct StatFloatPair 
{
    public Stat StatToAffect;
    public float Value;
}

[CreateAssetMenu(fileName = "CardBase", menuName = "Card", order = 0)]
[System.Serializable]
public sealed class CardBase : ScriptableObject 
{
    public CardRarity Rarity;
    public CardInfo Info;
    public List<StatFloatPair> BaseValue = new();
    public List<Modifier> Modifiers = new();

    public Dictionary<Stat, float> GetFinalValue(List<CardBase> allCards, int currentCardIndex) 
    {
        List<Dictionary<Stat, float>> allCardsStat = new();

        // Initialize all cards stats with THEIR base values
        foreach (CardBase card in allCards) 
        {
            Dictionary<Stat, float> currentCardStat = new();
            foreach (StatFloatPair pair in card.BaseValue) currentCardStat.Add(pair.StatToAffect, pair.Value);
            foreach (Stat stat in Enum.GetValues(typeof(Stat))) { if (!currentCardStat.ContainsKey(stat)) currentCardStat.Add(stat, 0); }
            allCardsStat.Add(currentCardStat);
        }

        // Populate Values and Modifiers from LEFT TO RIGHT
        for (int i = 0; i < allCards.Count; i++)
        {
            CardBase currentCard = allCards[i];

            foreach (Modifier mod in currentCard.Modifiers) 
            {
                Func<float, EffectOperator, float, float> applyEffect = (originalValue, effectOperator, val) => 
                {
                    if (effectOperator == EffectOperator.Summation) return originalValue + val;
                    else return originalValue * val;
                };

                if (mod.Targets == ModiferTarget.All)
                {
                    allCardsStat.ForEach(stats => 
                    {
                        stats[mod.StatToAffect] = applyEffect(stats[mod.StatToAffect], mod.Operator, mod.Value);
                    });
                }
                else if (mod.Targets == ModiferTarget.Left)
                {
                    if (i != 0)
                    {
                        Dictionary<Stat, float> targetCardStats = allCardsStat[i - 1];
                        targetCardStats[mod.StatToAffect] = applyEffect(targetCardStats[mod.StatToAffect], mod.Operator, mod.Value);
                    }
                }
                else if (mod.Targets == ModiferTarget.Right)
                {
                    if (i != allCards.Count - 1)
                    {
                        Dictionary<Stat, float> targetCardStats = allCardsStat[i + 1];
                        targetCardStats[mod.StatToAffect] = applyEffect(targetCardStats[mod.StatToAffect], mod.Operator, mod.Value);
                    }
                }
                else if (mod.Targets == ModiferTarget.AllLeft)
                {
                    if (i != 0)
                    {
                        for (int j = i - 1; j >= 0; j--)
                        {
                            Dictionary<Stat, float> targetCardStats = allCardsStat[j];
                            targetCardStats[mod.StatToAffect] = applyEffect(targetCardStats[mod.StatToAffect], mod.Operator, mod.Value);
                        }
                    }
                }
                else if (mod.Targets == ModiferTarget.AllRight)
                {
                    if (i != allCards.Count - 1)
                    {
                        for (int j = i + 1; j <= allCards.Count - 1; j++)
                        {
                            Dictionary<Stat, float> targetCardStats = allCardsStat[j];
                            targetCardStats[mod.StatToAffect] = applyEffect(targetCardStats[mod.StatToAffect], mod.Operator, mod.Value);
                        }
                    }
                }
                else if (mod.Targets == ModiferTarget.Adjacent)
                {
                    if (i != 0)
                    {
                        Dictionary<Stat, float> targetCardStats = allCardsStat[i - 1];
                        targetCardStats[mod.StatToAffect] = applyEffect(targetCardStats[mod.StatToAffect], mod.Operator, mod.Value);
                    }
                    if (i != allCards.Count - 1)
                    {
                        Dictionary<Stat, float> targetCardStats = allCardsStat[i + 1];
                        targetCardStats[mod.StatToAffect] = applyEffect(targetCardStats[mod.StatToAffect], mod.Operator, mod.Value);
                    }
                }
                else if (mod.Targets == ModiferTarget.Notadjacent)
                {
                    for (int j = 0; j < allCards.Count; j++)
                    {
                        if (j == i - 1 || j == i + 1 || j == i) continue;
                        Dictionary<Stat, float> targetCardStats = allCardsStat[j];
                        targetCardStats[mod.StatToAffect] = applyEffect(targetCardStats[mod.StatToAffect], mod.Operator, mod.Value);
                    }
                }
                // ...
            }
        }

        return allCardsStat[currentCardIndex];
    }
}