using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput input;

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float accelerationTime = 3f;
    [SerializeField] float decelerationTime = 3f;
    [SerializeField] float moveRotationAngle = 50f;
    [SerializeField] float paddingX = 0.2f;
    [SerializeField] float paddingY = 0.2f;

    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectile3;
    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleTop;
    [SerializeField] Transform muzzleBottom;
    [SerializeField, Range(0, 2)] int weaponPower = 0;
    [SerializeField] float fireInterval = 0.2f;

    WaitForSeconds waitForFireInterval;

    new Rigidbody2D rigidbody;

    Coroutine moveCoroutine;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
    }

    void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
    }

    void Start()
    {
        rigidbody.gravityScale = 0f;

        waitForFireInterval = new WaitForSeconds(fireInterval);

        input.EnableGameplayInput();
    }

    #region MOVE
    void Move(Vector2 moveInput)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
        // moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveInput.normalized * moveSpeed, moveRotation));
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveInput.normalized * moveSpeed, Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));
        StartCoroutine(MovePositionLimitCoroutine());
    }

    void StopMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        StopCoroutine(MovePositionLimitCoroutine());
    }

    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        float t = 0f;

        while (t < time)
        {
            t += Time.fixedDeltaTime / time;
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t / time);
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t / time);

            yield return null;
        }
    }

    IEnumerator MovePositionLimitCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);

            yield return null;
        }
    }
    #endregion

    #region FIRE
    void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }

    void StopFire()
    {
        StopCoroutine(nameof(FireCoroutine));
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(projectile1, muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(projectile1, muzzleTop.position);
                    PoolManager.Release(projectile1, muzzleBottom.position);
                    break;
                case 2:
                    PoolManager.Release(projectile1, muzzleMiddle.position);
                    PoolManager.Release(projectile2, muzzleTop.position);
                    PoolManager.Release(projectile3, muzzleBottom.position);
                    break;
                default:
                    break;
            }

            yield return waitForFireInterval;
        }
    }
    #endregion
}