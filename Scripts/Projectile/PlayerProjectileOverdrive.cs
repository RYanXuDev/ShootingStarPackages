using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    [SerializeField] ProjectileGuidanceSystem guidanceSystem;
    
    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);
        transform.rotation = Quaternion.identity;

        if (target == null)
        {
            base.OnEnable();
        }
        else 
        {
            StartCoroutine(guidanceSystem.HomingCoroutine(target));
        }
    }
}