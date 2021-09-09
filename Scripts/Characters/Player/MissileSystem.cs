using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] GameObject missilePrefab = null;
    [SerializeField] AudioData launchSFX = null;

    public void Launch(Transform muzzleTransform)
    {
        // Release a missile clone form object pool
        PoolManager.Release(missilePrefab, muzzleTransform.position);
        // Play missile launch SFX
        AudioManager.Instance.PlayRandomSFX(launchSFX);
    }
}