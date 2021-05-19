using UnityEngine;

public class EnemyProjectile : Projectile
{
    void Awake()
    {
        if (moveDirection != Vector2.left)
        {
            transform.rotation = Quaternion.FromToRotation(Vector2.left, moveDirection);
        }
    }
}