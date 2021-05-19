using System.Collections;
using UnityEngine;

public class EnemyProjectile_Aiming : Projectile
{
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectionCoroutine));
        base.OnEnable();
    }

    IEnumerator MoveDirectionCoroutine()
    {
        yield return null;

        if (target.activeSelf)
        {
            moveDirection = (target.transform.position - transform.position).normalized;
        }
    }
}