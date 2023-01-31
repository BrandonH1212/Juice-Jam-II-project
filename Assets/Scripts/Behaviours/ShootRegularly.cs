using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

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
