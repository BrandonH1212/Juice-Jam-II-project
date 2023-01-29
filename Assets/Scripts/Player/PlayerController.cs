using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

//[Serializable]
//public class ObservedList<T> : IList<T>
//{
//    public delegate void ChangedDelegate(int index, T oldValue, T newValue);

//    [SerializeField] private List<T> _list = new List<T>();

//    // NOTE: I changed the signature to provide a bit more information
//    // now it returns index, oldValue, newValue
//    public event ChangedDelegate Changed;

//    public event Action Updated;

//    public IEnumerator<T> GetEnumerator()
//    {
//        return _list.GetEnumerator();
//    }

//    IEnumerator IEnumerable.GetEnumerator()
//    {
//        return GetEnumerator();
//    }

//    public void Add(T item)
//    {
//        _list.Add(item);
//        Updated?.Invoke();
//    }

//    public void Clear()
//    {
//        _list.Clear();
//        Updated?.Invoke();
//    }

//    public bool Contains(T item)
//    {
//        return _list.Contains(item);
//    }

//    public void CopyTo(T[] array, int arrayIndex)
//    {
//        _list.CopyTo(array, arrayIndex);
//    }

//    public bool Remove(T item)
//    {
//        var output = _list.Remove(item);
//        Updated?.Invoke();

//        return output;
//    }

//    public int Count => _list.Count;
//    public bool IsReadOnly => false;

//    public int IndexOf(T item)
//    {
//        return _list.IndexOf(item);
//    }

//    public void Insert(int index, T item)
//    {
//        _list.Insert(index, item);
//        Updated?.Invoke();
//    }

//    public void RemoveAt(int index)
//    {
//        _list.RemoveAt(index);
//        Updated?.Invoke();
//    }

//    public void AddRange(IEnumerable<T> collection)
//    {
//        _list.AddRange(collection);
//        Updated?.Invoke();
//    }

//    public void RemoveAll(Predicate<T> predicate)
//    {
//        _list.RemoveAll(predicate);
//        Updated?.Invoke();
//    }

//    public void InsertRange(int index, IEnumerable<T> collection)
//    {
//        _list.InsertRange(index, collection);
//        Updated?.Invoke();
//    }

//    public void RemoveRange(int index, int count)
//    {
//        _list.RemoveRange(index, count);
//        Updated?.Invoke();
//    }

//    public T this[int index]
//    {
//        get { return _list[index]; }
//        set
//        {
//            var oldValue = _list[index];
//            _list[index] = value;
//            Changed?.Invoke(index, oldValue, value);
//            // I would also call the generic one here
//            Updated?.Invoke();
//        }
//    }
//}

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
    

    // DEV:
    // public GameObject BasicShot;
    // public float TimeToShoot = 1;

    public float Health { get; private set; } = 100;
    public float MovementSpeed { get; private set; } = 10;
    public float Shield { get; private set; }
    public float ShieldRegenerationSpeed { get; private set; }
    public float DamageReduction { get; private set; }
    public float DamagePower { get; private set; }

    public UnityEvent OnDamage;
    public UnityEvent OnKilled;
    public UnityEvent OnShieldBroken;
    public UnityEvent<List<Modifier>> OnEffectsChanged = new();

    public List<CardBase> InitialCards;

    public List<Tuple<CardBase, Component>> EquipedCards = new();

    //public List<Modifier> GetModifierOnCards() 
    //{
    //    // return EquipedCards[0].Item1.ModifiersOnCards;
    //}

    public void EquipCard(CardBase c) 
    {
        //CardBehaviour newlyAddedInstance = this.gameObject.AddComponent(CardBehaviourAttribute.GetTypeWithBehaviourName(c.BehaviourTypeName)) as CardBehaviour;
        //newlyAddedInstance.CurrentCard = c;
        //EquipedCards.Add(new Tuple<CardBase, Component>(c, newlyAddedInstance));
        //OnEffectsChanged.Invoke(c.ModifiersOnCards);
    }

    // Start is called before the first frame update
    void Start()
    {

        //EquipedCards.Changed += (index, oldVal, newVal) => 
        //{
        //    // CardBase cardAdded = list[arg.index];
        //    Debug.Log("CardBase added");
        //    CardBehaviourAttribute.GetBehaviourType(newVal.BehaviourName);
        //};

        // foreach (CardBase c in InitialCards) EquipCard(c);

        int currentIndex = 0;
        foreach (CardBase card in InitialCards) 
        {
            Debug.Log($"Card {card.Info.cardName}: Final Damage - {card.GetFinalValue(InitialCards, currentIndex)[Stat.Damage]}");
            currentIndex++;
        }

        


        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        InferMovementDirection();

        //TimeToShoot -= Time.deltaTime;
        //if (TimeToShoot < 0) 
        //{
        //    var newProjectileObject = Instantiate(BasicShot);
        //    newProjectileObject.transform.position = transform.position;
        //    TimeToShoot = 0.2f;
        //}
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
}
