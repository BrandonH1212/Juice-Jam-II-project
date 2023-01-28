using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class CardBehaviour : MonoBehaviour
{
    public CardBase CurrentCard;
    public List<ModifierOnCards> ModifierOnCards = new();
    public PlayerController PlayerController;

    public void Subscribe() 
    {
        PlayerController = GetComponent<PlayerController>();
        ModifierOnCards = PlayerController.GetModifierOnCards();
        PlayerController.OnEffectsChanged.AddListener(modifers => ModifierOnCards = modifers);
    }

    public void ShootProjectile() 
    {
        Instantiate(CurrentCard.Projectile);
    }
}

[CardBehaviourAttribute("ShootRegularly")]
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
        if (CurrentCard != null) 
        {
            TimeRemaining -= Time.deltaTime;
            if (TimeRemaining <= 0) 
            {
                ShootProjectile();
                ModifierOnCards[] fireRateList = ModifierOnCards.Where(c => c.PropertyToAffect == CardProperty.FireRate).ToArray();
                float fireRate = 0;
                if (fireRateList.Length == 1) fireRate = fireRateList[0].Value;
                if (fireRate != 0) TimeRemaining = 1 / fireRate;
            }
        }
    }
}

[CardBehaviourAttribute("ShootOnce")]
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

