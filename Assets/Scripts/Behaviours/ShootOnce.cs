using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[CardBehaviour("ShootOnce")]
public class ShootOnce : CardBehaviour
{
    public void Start()
    {
        base.Subscribe();
        ShootProjectile();
    }
}

