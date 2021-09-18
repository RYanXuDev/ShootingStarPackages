using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float explosionDamage = 100f;
    [SerializeField] Collider2D explosionCollider;

    WaitForSeconds waitExplosionTime = new WaitForSeconds(0.1f);

    void OnEnable()
    {
        StartCoroutine(ExplosionCoroutine());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If there is any enemy within the trigger range, the enemy takes explosion damage
        // 当检测到任何敌人在触发器范围内，则敌人受到爆炸伤害
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(explosionDamage);
        }
    }

    IEnumerator ExplosionCoroutine()
    {
        // Enable the explosion collider when this VFX spawned
        // 当特效生成时启用爆炸检测碰撞体
        explosionCollider.enabled = true;

        yield return waitExplosionTime;

        // Disable the explosion collider
        // 爆炸检测完毕后关闭碰撞体
        explosionCollider.enabled = false;
    }
}