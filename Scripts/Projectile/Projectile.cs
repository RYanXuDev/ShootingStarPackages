using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;
    [SerializeField] AudioData[] hitSFX;
    [SerializeField] float damage;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;

    protected GameObject target;

    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectlyCoroutine());
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.TakeDamage(damage);
            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(hitSFX);
            gameObject.SetActive(false);
        }
    }

    IEnumerator MoveDirectlyCoroutine()
    {
        while (gameObject.activeSelf)
        {
            Move();

            yield return null;
        }
    }

    protected void SetTarget(GameObject target) => this.target = target;

    public void Move() => transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
}