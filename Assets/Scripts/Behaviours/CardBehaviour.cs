using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
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
    /// Return whether the current card is removed in the player cards list. If removed, destroy this script.
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

    public Dictionary<Stat, float> GetEffectiveStats() => _effectiveStatThisCard;


    public void UpdateEffectiveStats()
    {
        if (CheckForUnsubscribion()) return;
        if (PlayerController.TryGetEffectiveStatsOnCard(CurrentCard, out Dictionary<Stat, float> effectiveStats)) _effectiveStatThisCard = effectiveStats;
    }

    /// <summary>
    /// Spawn a projectile prefab at the player location.
    /// </summary>
    /// <returns></returns>
    public GameObject ShootProjectile() => ShootProjectile(transform.position);

    /// <summary>
    /// Spawn a projectile prefab at a given position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject ShootProjectile(Vector3 position)
    {
        if (CurrentCard.CardBase.ProjectilePrefab != null)
        {
            var projectileObject = Instantiate(CurrentCard.CardBase.ProjectilePrefab, position, new Quaternion());
            projectileObject.GetComponent<Projectile>().ApplyStats(_effectiveStatThisCard);
            return projectileObject;
        }
        else return null;
    }
}