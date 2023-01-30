using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class CardBehaviour : MonoBehaviour
{
    public CardBaseInstance CurrentCard;
    public PlayerController PlayerController;
    protected Dictionary<Stat, float> _effectiveStatThisCard = new();

    public void Subscribe() 
    {
        PlayerController = GetComponent<PlayerController>();
        PlayerController.OnCardsChanged.AddListener(UpdateEffectiveStats);
        UpdateEffectiveStats();
    }

    /// <summary>
    /// Return whether the current card is removed. If removed, destroy this script.
    /// </summary>
    /// <returns></returns>
    private bool CheckForUnsubscribion() 
    {
        if (!PlayerController.EquipedCards.Any(cardBaseInstance => cardBaseInstance.MemoryAddress == CurrentCard.MemoryAddress))
        {
            PlayerController.GetComponents<CardBehaviour>().ToList().ForEach(component =>
            {
                if (component.CurrentCard.MemoryAddress == CurrentCard.MemoryAddress) DestroyImmediate(component);
            });
            return true;
        }

        return false;
    }

    public void UpdateEffectiveStats() 
    {
        if (!CheckForUnsubscribion()) 
        {
            if (PlayerController.TryGetEffectiveStatsOnCard(CurrentCard, out Dictionary<Stat, float> effectiveStats)) 
            {
                _effectiveStatThisCard = effectiveStats;
            }
        }
    }

    public GameObject ShootProjectile() 
    {
        if (CurrentCard.CardBase.ProjectilePrefab != null) return Instantiate(CurrentCard.CardBase.ProjectilePrefab, transform.position, new Quaternion());
        else return null;
    }
}

[CardBehaviour("ShootRegularly")]
public class ShootRegularly : CardBehaviour
{  
    public float TimeRemaining = 1;
    public bool ShootStarted = true;

    public void Start() 
    {
        base.Subscribe();
    }

    public void FixedUpdate()
    {
        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining <= 0 && _effectiveStatThisCard[Stat.FireRate] > 0)
        {
            ShootProjectile();
            TimeRemaining = 1 / _effectiveStatThisCard[Stat.FireRate];
        }
    }
}

[CardBehaviour("ShootOnce")]
public class ShootOnce : CardBehaviour
{
    public bool Shot = false;

    public void Start()
    {
        base.Subscribe();
    }

    public void FixedUpdate()
    {
        if (CurrentCard != null && !Shot)
        {
            ShootProjectile();
            Shot = true;
        }
    }
}

