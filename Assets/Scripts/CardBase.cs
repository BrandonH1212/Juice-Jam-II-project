using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    Range,
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

public static class StatInfo
{
    public static Dictionary<Stat, StatUIInfoStruct> Data = new()
    {
        { Stat.Range, new StatUIInfoStruct { DisplayName = "Range", ToolTipDescription = "Tracking range for firing", Icon = Resources.Load<Sprite>("Assets/UI/icons/range icon.png"), IconColor = Color.blue } },
        { Stat.Damage, new StatUIInfoStruct { DisplayName = "Damage", ToolTipDescription = "Damage inflicted on contact", Icon = Resources.Load<Sprite>("Assets/UI/icons/placeholder icon.png"), IconColor = Color.red } },
        { Stat.ProjectileSpeed, new StatUIInfoStruct { DisplayName = "Bullet Speed", ToolTipDescription = "Traveling speed of bullets", Icon = Resources.Load<Sprite>("Assets/UI/icons/range icon.png"), IconColor = Color.blue } },
        { Stat.ProjectileSize, new StatUIInfoStruct { DisplayName = "Size", ToolTipDescription = "Bullet size", Icon = Resources.Load<Sprite>("Assets/UI/icons/placeholder icon.png"), IconColor = Color.blue } },
        { Stat.FireRate, new StatUIInfoStruct { DisplayName = "Fire Rate", ToolTipDescription = "Frequency of firing", Icon = Resources.Load<Sprite>("Assets/UI/icons/placeholder icon.png"), IconColor = Color.yellow } },
        { Stat.Penetration, new StatUIInfoStruct { DisplayName = "Penetration", ToolTipDescription = "Amount of enemies that can be passed through before the bullet is destroyed", Icon = Resources.Load<Sprite>("Assets/UI/icons/placeholder icon.png"), IconColor = Color.yellow } },
        { Stat.PeriodicDamage, new StatUIInfoStruct { DisplayName = "Periodic Damage", ToolTipDescription = "Damage inflicted on each damage tick", Icon = Resources.Load<Sprite>("Assets/UI/icons/placeholder icon.png"), IconColor = Color.green } },
        { Stat.PeriodicInterval, new StatUIInfoStruct { DisplayName = "Periodic Damage Interval", ToolTipDescription = "Time between each tick of periodic damage", Icon = Resources.Load<Sprite>("Assets/UI/icons/placeholder icon.png"), IconColor = Color.green } },
        { Stat.PeriodicCount, new StatUIInfoStruct { DisplayName = "Periodic Damage Duration", ToolTipDescription = "Amount of damage ticks before the periodic damage wears off", Icon = Resources.Load<Sprite>("Assets/UI/icons/placeholder icon.png"), IconColor = Color.green } },
        { Stat.SplashDamage, new StatUIInfoStruct { DisplayName = "Splash Damage", ToolTipDescription = "Damage inflicted to nearby ememies on contact", Icon = Resources.Load<Sprite>("Assets/UI/icons/placeholder icon.png"), IconColor = Color.magenta } },
        { Stat.SplashRadius, new StatUIInfoStruct { DisplayName = "Splash Radius", ToolTipDescription = "Range of effect of the splash damage", Icon = Resources.Load<Sprite>("Assets/UI/icons/placeholder icon.png"), IconColor = Color.magenta } },
        { Stat.CriticalChance, new StatUIInfoStruct { DisplayName = "Critical Chance", ToolTipDescription = "Chance for a critical hit", Icon = Resources.Load<Sprite>("Assets/UI/icons/placeholder icon.png"), IconColor = Color.yellow } },
        { Stat.CriticalDamage, new StatUIInfoStruct { DisplayName = "Critical Damage", ToolTipDescription = "Percentage increase of damage when a critical hit occurs", Icon = Resources.Load<Sprite>("Assets/UI/icons/placeholder icon.png"), IconColor = Color.yellow } },
        { Stat.LifeSteal, new StatUIInfoStruct { DisplayName = "Life Steal", ToolTipDescription = "Amount of health you gain when an enemy is hit", Icon = Resources.Load<Sprite>("Assets/UI/icons/placeholder icon.png"), IconColor = Color.red } },
    };
}

public struct StatUIInfoStruct
{
    public String DisplayName;
    public String ToolTipDescription;
    public Sprite Icon;
    public Color IconColor;
}

public enum EffectOperator 
{
    Summation,
    Multiplication,
    Min,
    Max
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
    public GameObject ProjectilePrefab;
    public string BehaviourName = "";

    public Dictionary<Stat, float> GetFinalValue(List<CardBaseInstance> allCards, int currentCardIndex) 
    {
        List<Dictionary<Stat, float>> allCardsStat = new();

        // Initialize all cards stats with THEIR base values
        foreach (CardBaseInstance card in allCards) 
        {
            Dictionary<Stat, float> currentCardStat = new();
            foreach (StatFloatPair pair in card.CardBase.BaseValue) currentCardStat.Add(pair.StatToAffect, pair.Value);
            foreach (Stat stat in Enum.GetValues(typeof(Stat))) { if (!currentCardStat.ContainsKey(stat)) currentCardStat.Add(stat, 0); }
            allCardsStat.Add(currentCardStat);
        }

        // Populate Values and Modifiers from LEFT TO RIGHT
        for (int i = 0; i < allCards.Count; i++)
        {
            CardBase currentCard = allCards[i].CardBase;

            foreach (Modifier mod in currentCard.Modifiers) 
            {
                Func<float, EffectOperator, float, float> applyEffect = (originalValue, effectOperator, val) => 
                {
                    if (effectOperator == EffectOperator.Summation) return originalValue + val;
                    else if (effectOperator == EffectOperator.Min) return Math.Min(originalValue, val);
                    else if (effectOperator == EffectOperator.Max) return Math.Max(originalValue, val);
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

/// <summary>
/// This class represent an instance of the CardBase data class.
/// This class should not be created other than player controller script normally.
/// </summary>
[System.Serializable]
public sealed class CardBaseInstance 
{
    [SerializeField]
    public CardBase CardBase;
    public Component ComponentOnPlayerController;
    public PlayerController PlayerController;
    public string MemoryAddress;

    public CardBaseInstance(CardBase cardBase, Component componentOnController, PlayerController controller) 
    {
        CardBase = cardBase;
        ComponentOnPlayerController = componentOnController;
        PlayerController = controller;
        MemoryAddress = Get(this);
    }
    public static string Get(object a)
    {
        GCHandle handle = GCHandle.Alloc(a, GCHandleType.Pinned);
        try
        {
            IntPtr pointer = GCHandle.ToIntPtr(handle);
            return "0x" + pointer.ToString("X");
        }
        finally
        {
            handle.Free();
        }
    }
}