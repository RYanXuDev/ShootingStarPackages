using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    TrailRenderer trail;

    void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }

    void OnDisable()
    {
        trail.Clear();
    }
}