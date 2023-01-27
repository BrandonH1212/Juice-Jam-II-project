using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected ProjectileSettings ProjectileSettings = new();
    
    // public float ProjectileType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class ProjectileSettings
{
    // public Modifiers BaceProjectileModifiers;
    [SerializeField] public float PeriodicDamage  = 1;
    [SerializeField] public float PeriodicDamageInterval = 1;
    [SerializeField] public float SplashDamage = 1;
    [SerializeField] public float SplashDamageRadius = 1;
}