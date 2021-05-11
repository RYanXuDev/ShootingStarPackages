using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("---- MOVE ----")]
    [SerializeField] float paddingX;
    [SerializeField] float paddingY;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float moveRotationAngle = 25f;

    [Header("---- FIRE ----")]
    [SerializeField] GameObject[] projectiles;
    [SerializeField] Transform muzzle;
    [SerializeField] float minFireInterval;
    [SerializeField] float maxFireInterval;
    
    void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        Vector3 targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            // if has not arrived targetPosition
            if (Vector3.Distance(transform.position, targetPosition) > Mathf.Epsilon)
            {
                // keep moving to targetPosition
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                // make enemy rotate with x axis while moving
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }
            else
            {
                // set a new targetPosition
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }

            yield return null;
        }
    }

    IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            foreach (var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
        }
    }
}