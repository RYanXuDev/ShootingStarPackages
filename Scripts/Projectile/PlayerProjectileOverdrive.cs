using System.Collections;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    [SerializeField] float moveRotationAngle = 50f;

    protected override void OnEnable()
    {
        transform.rotation = Quaternion.identity;
        target = EnemyManager.Instance.RandomEnemy;

        if (target == null)
        {
            StartCoroutine(nameof(MoveDirectlyCoroutine));
        }
        else 
        {
            StartCoroutine(TrackTargetCoroutine(Random.Range(-moveRotationAngle, moveRotationAngle)));
        }
    }
}