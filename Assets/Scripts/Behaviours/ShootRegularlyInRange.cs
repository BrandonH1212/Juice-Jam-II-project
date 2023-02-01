using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[CardBehaviour("ShootRegularlyInRange")]
public class ShootRegularlyInRange : CardBehaviour 
{

    
    public float TimeRemaining = 1;
    public bool ShootStarted = true;
    private List<GameObject> TargetObjects;

    public void Start() 
    {
        base.Subscribe();
    }

    public void FixedUpdate()
    {
        
        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining <= 0 && _effectiveStatThisCard[Stat.FireRate] > 0)
         {
            
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _effectiveStatThisCard[Stat.Range]);
            TargetObjects = new List<GameObject>();

            foreach (Collider2D c in colliders)
            {
                if (c.gameObject.tag == "Enemy")
                {
                    if (!TargetObjects.Contains(c.gameObject))
                        TargetObjects.Add(c.gameObject);
                }
            }
            
            if (TargetObjects.Count > 0)
                ShootProjectile();
            
            TimeRemaining = (float)ConstantValues.FireRate / _effectiveStatThisCard[Stat.FireRate];
        }
    }
}
