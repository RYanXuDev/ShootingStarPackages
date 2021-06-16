using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;
    [SerializeField] float damage;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;

    protected GameObject target;

    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectly());
    }

    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.TakeDamage(damage);
            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            gameObject.SetActive(false);
        }
    }
}