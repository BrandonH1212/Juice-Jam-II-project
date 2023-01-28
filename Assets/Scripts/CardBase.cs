//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public enum CardRarity
//{
//    Common,
//    Uncommon,
//    Rare,
//    Epic,
//    Legendary
//}

//public enum CardModifiersType
//{
//    All,         // modifiers get added to all other cards 
//    AllLeft,     // modifiers get added to all cards to the left of this card
//    AllRight,    // modifiers get added to all cards to the right of this card
//    Left,        // modifiers get added to the card to the left of this card
//    Right,       // modifiers get added to the card to the right of this card
//    NotLeft,     // modifiers get added to all cards except the card to the left of this card
//    NotRight,    // modifiers get added to all cards except the card to the right of this card
//    Adjacent,    // modifiers get added to the card to the left and right of this card
//    Notadjacent  // modifiers get added to all cards except the card to the left and right of this card
//}

//public enum TragetType
//{
//    Fixed,
//    Random,
//    Closest,
//    Furthest,
//    MostHealth,
//    LeastHealth,
//}

//[System.Serializable]
//public struct CardInfo
//{
//    public string cardName;
//    public string cardDescription;
//    public CardRarity cardRarity;
//    public Sprite cardIcon;
//}

//[System.Serializable]
//public struct Modifiers
//{
//    public float Damage;
//    public float Size;
//    public float Speed;
//    public float FireRate;
//    public float LifeSteal;
//    public int Penetration;
//    public float CriticalChance;
//    public float CriticalDamageModifier;
//}

//[System.Serializable]
//public struct PlayerModifiers
//{
//    public float Health;
//    public float MovementSpeed;
//    public float Shields;
//    public float ShieldRegeneration;
//}


//[System.Serializable]
//public struct ProjectileSettings
//{
//    public Modifiers BaceProjectileModifiers;
//    public float PeriodicDamage;
//    public float PeriodicDamageInterval;
//    public float SplashDamage;
//    public float SplashDamageRadius;
//    public float TargetAttackRange;
//    public GameObject ProjectilePrefab;
//    public TragetType TragetType;

//}

//[CreateAssetMenu(fileName = "CardBace", menuName = "Card", order = 0)]
//public class CardBace : ScriptableObject
//{
//    public CardInfo cardInfo;
//    public PlayerModifiers playerModifiers;
//    public ProjectileSettings ProjectileSettings;
//    public Modifiers effectModifiers;
//    public CardModifiersType cardModifiersType;

//}

using System;
using System.Collections.Generic;
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

public enum CardModifiersType
{
    All,         // modifiers get added to all other cards 
    AllLeft,     // modifiers get added to all cards to the left of this card
    AllRight,    // modifiers get added to all cards to the right of this card
    Left,        // modifiers get added to the card to the left of this card
    Right,       // modifiers get added to the card to the right of this card
    NotLeft,     // modifiers get added to all cards except the card to the left of this card
    NotRight,    // modifiers get added to all cards except the card to the right of this card
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

public enum CardProperty 
{
    Damage,
    Size,
    FireRate,
    LifeSteal,
    Penetration,
    CriticalChance,
    CriticalDamageModifier
}

public enum EffectOperator 
{
    Summation,
    Multiplication
}

[System.Serializable]
public struct ModifierOnCards
{
    public CardProperty PropertyToAffect;
    public EffectOperator Operator;
    public float Value;
}

public enum PlayerProperty 
{
    Health,
    MovementSpeed,
    Shields,
    ShieldRegeneration
}

[System.Serializable]
public struct ModifierOnPlayer
{
    public PlayerProperty PropertyToAffect;
    public EffectOperator Operator;
    public float Value;
}

[System.Serializable]
public struct ModifierOnProjectile
{
    public ProjectileProperty PropertyToAffect;
    public EffectOperator Operator;
    public float Value;
}

public enum ProjectileProperty 
{
    PeriodicDamage,
    PeriodicDamageInterval,
    SplashDamage,
    SplashDamageRadius,
    TargetAttackRange,
}

[CreateAssetMenu(fileName = "CardBase", menuName = "Card", order = 0)]
[System.Serializable]
public sealed class CardBase : ScriptableObject 
{
    public CardRarity Rarity;
    public CardModifiersType ModifiersType;
    public CardInfo Info;
    public List<ModifierOnCards> ModifiersOnCards = new();
    public List<ModifierOnPlayer> ModifiersOnPlayer = new();
    public List<ModifierOnProjectile> ModifiersOnProjectiles = new();
    public GameObject Projectile;

    public string BehaviourTypeName { get; set; } = "";

    //public static Dictionary<CardProperty, float> GetResultingProperties(CardBase previousCard) 
    //{
    //    Dictionary<CardProperty, float> output = new();



    //    foreach (ModifierOnCards mod in ModifiersOnCards) 
    //    {
    //        if (output.ContainsKey(mod.PropertyToAffect)) 
    //        {
    //            if (mod.Operator == EffectOperator.Summation) output[mod.PropertyToAffect] += mod.Value;
    //            else output[mod.PropertyToAffect] *= mod.Value;
    //        }
    //    }
        
    //    return output;
    //}
}