using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.VolumeComponent;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// The direction that the player is moving right now.
    /// The player position will be updated using this field, in fixedUpdate().
    /// </summary>
    private Vector2 _movementDirectionNormalized = new Vector2(0,0);
    private Rigidbody2D _rigidbody2D = null;
    private SpriteRenderer _spriteRenderer = null;
    private Animator _animator = null;

    // has to do this since unity doesn't suuport ObserableCollection in inspector natively. I am sorry.
    private List<CardBaseInstance> _lastFrameEquipedCards = new();


    [SerializeField]
    public GameObject OnDamagedEffect;

    // DEV:
    // public GameObject BasicShot;
    // public float TimeToShoot = 1;

    public float Health { get; private set; } = 100;
    public float MovementSpeed { get; private set; } = 20;
    public float Shield { get; private set; }
    public float ShieldRegenerationSpeed { get; private set; }
    public float DamageReduction { get; private set; }
    public float DamagePower { get; private set; }

    public UnityEvent OnDamage;
    public UnityEvent OnKilled;
    public UnityEvent OnShieldBroken;
    public UnityEvent OnCardsChanged = new();

    public List<CardBase> InitialCards;

    public List<CardBaseInstance?> EquipedCards = new();
    public List<CardBase> InventoryCards = new();


    private CardAcquiredDisplay CardDisplay = null;
    
    public CardBaseInstance CreateCardBaseInstance(CardBase c)
    {
        CardBaseInstance instanceCreated = new(c, null, this);
        if (c.BehaviourName != "")
        {
            CardBehaviour newlyAddedInstance = this.gameObject.AddComponent(CardBehaviourAttribute.GetTypeWithBehaviourName(c.BehaviourName)) as CardBehaviour;
            newlyAddedInstance.CurrentCard = instanceCreated;
            instanceCreated.ComponentOnPlayerController = newlyAddedInstance;
        }
        return instanceCreated;
    }

    public CardBaseInstance EquipCard(CardBase c)
    {
        var card = CreateCardBaseInstance(c);
        EquipedCards.Add(card);
        return card;
    }

    public CardBaseInstance EquipCard(CardBase c, int index)
    {
        var card = CreateCardBaseInstance(c);
        EquipedCards.Insert(index, card);
        return card;
    }

    public void SwapCard(int index1, int index2)
    {
        (EquipedCards[index2], EquipedCards[index1]) = (EquipedCards[index1], EquipedCards[index2]);
    }

    public void EquipInventoryCard(int cardIndexInInventory, int slotToReplace) 
    {
        var newInstance = CreateCardBaseInstance(InventoryCards[cardIndexInInventory]);
        // if the card name is "Empty Slot" dont add it to the inventory (hacky fix for now )
        if (EquipedCards[slotToReplace].CardBase.Info.cardName == "Empty Slot")
        {
            InventoryCards.RemoveAt(cardIndexInInventory);
            EquipedCards[slotToReplace] = newInstance;
            return;
        }
        
        InventoryCards[cardIndexInInventory] = EquipedCards[slotToReplace].CardBase;
        EquipedCards[slotToReplace] = newInstance;

    }

    public float damageInterval = 0.2f;
    private float timeSinceLastDamage = 0;


    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (timeSinceLastDamage >= damageInterval)
            {
                Health -= 5;
                timeSinceLastDamage = 0;
                Instantiate(OnDamagedEffect, transform.position, new Quaternion());
                if (Health <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    Health = 100;
                }
            }
        }
    }



    public CardBase GetRandomCard(CardRarity rarity)
    {
        CardDataManager dataManager = CardDataManager.instance;

        CardBase randomCard = dataManager.GetRandomCard(rarity);

        return randomCard;
    }

    public void AcquireNewCard(CardRarity rarity= CardRarity.Common)
    {
        InventoryCards.Add(GetRandomCard(rarity));

        if (InventoryCards.Count == 0) CardDisplay.Display(0);
        else CardDisplay.Display(InventoryCards.Count-1);

    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        CardDisplay = FindObjectOfType<CardAcquiredDisplay>();
        
        InitialCards.ForEach(cardBase => 
        {
            if (cardBase == null) EquipedCards.Add(null);
            else EquipCard(cardBase);
        });
        AcquireNewCard();
    }

    // Update is called once per frame
    void Update()
    {
        InferMovementDirection();

        #region Check for list change
        if (!EquipedCards.SequenceEqual(_lastFrameEquipedCards)) OnCardsChanged.Invoke();
        _lastFrameEquipedCards = new(EquipedCards);
        #endregion
    }

    void InferMovementDirection() 
    {
        Vector2 output = new(0,0);

        if (Input.GetKey(KeyCode.W)) output.y += MovementSpeed;
        if (Input.GetKey(KeyCode.A)) output.x -= MovementSpeed;
        if (Input.GetKey(KeyCode.S)) output.y -= MovementSpeed;
        if (Input.GetKey(KeyCode.D)) output.x += MovementSpeed;

        //transform.rotation = Quaternion.EulerRotation(0,0, Mathf.Atan2(output.y, output.x) * 180f / (float)Math.PI);

        _movementDirectionNormalized = 10 * output;
    }

    private void FixedUpdate()
    {
        _rigidbody2D.AddForce(new Vector2(_movementDirectionNormalized.x, _movementDirectionNormalized.y));
        _rigidbody2D.velocity = Vector3.ClampMagnitude(_rigidbody2D.velocity, MovementSpeed);

        // Update the animator 
        _animator.SetBool("IsMoving", _movementDirectionNormalized.magnitude > 0);

        // flip the sprite based on the velocity
        _spriteRenderer.flipX = _rigidbody2D.velocity.x > 0;
        timeSinceLastDamage += Time.deltaTime;
    }

    public void TakeDamage(float damage) 
    {
        if (Shield > 0)
        {
            if (Shield - damage <= 0) OnShieldBroken.Invoke();
            Shield = Math.Max(0, Shield - damage);
        }
        else
        {
            if (Health - damage <= 0) OnKilled.Invoke();
            Health = Math.Max(0, Health - damage);
        }
    }

    public bool TryGetEffectiveStatsOnCard(CardBaseInstance cardInstance, out Dictionary<Stat, float> effectiveStats) 
    {
        int index = -1;

        if (cardInstance != null)
        {
            for (int i = 0; i < EquipedCards.Count; i++)
            {
                if (EquipedCards[i] != null)
                {
                    var currentCard = EquipedCards[i];
                    if (currentCard.MemoryAddress == cardInstance.MemoryAddress) index = i;
                }
            }
        }
        if (index != -1) { effectiveStats = cardInstance.CardBase.GetFinalValue(EquipedCards.ToList(), index); }
        
        else effectiveStats = CardBase.InitializeEmptyStatsList();
        return index != -1;
    }
}
