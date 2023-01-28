using System.Collections;
using System.Collections.Generic;
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
    Notadjacent  // modifiers get added to all cards except the card to the left and right of this card
}

public enum TragetType
{
    Random,
    Closest,
    Furthest,
    MostHealth,
    LeastHealth,
}



[System.Serializable]
public struct CardInfo
{
    public string cardName;
    public string cardDescription;
    public CardRarity cardRarity;
    public Sprite cardIcon;
}

[System.Serializable]
public struct Modifiers
{
    public float Damage;
    public float Size;
    public float Speed;
    public float FireRate;
    public float LifeSteal;
    public float Penetration;
    public float CriticalChance;
    public float CriticalDamageModifier;
}

[System.Serializable]
public struct PlayerModifiers
{
    public float Health;
    public float MovementSpeed;
    public float Shields;
    public float ShieldRegeneration;
}


[System.Serializable]
public struct ProjectileSettings
{
    public Modifiers BaceProjectileModifiers;
    public float PeriodicDamage;
    public float PeriodicDamageInterval;
    public float SplashDamage;
    public float SplashDamageRadius;
    public float TargetAttackRange;
    public GameObject ProjectilePrefab;
    public TragetType TragetType;

}

[CreateAssetMenu(fileName = "CardBace", menuName = "Card", order = 0)]
public class CardBace : ScriptableObject
{
    public CardInfo cardInfo;
    public PlayerModifiers playerModifiers;
    public ProjectileSettings ProjectileSettings;
    public Modifiers effectModifiers;
    public CardModifiersType cardModifiersType;

}

