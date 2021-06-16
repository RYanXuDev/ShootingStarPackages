using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [SerializeField] StatsBar_HUD statsBar_HUD;
    [SerializeField] bool regenerateHealth = true;
    [SerializeField] float healthRegenerateTime;
    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;

    [Header("---- INPUT ----")]
    [SerializeField] PlayerInput input;

    [Header("---- MOVE ----")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float accelerationTime = 3f;
    [SerializeField] float decelerationTime = 3f;
    [SerializeField] float moveRotationAngle = 50f;
    [SerializeField] float paddingX = 0.2f;
    [SerializeField] float paddingY = 0.2f;

    [Header("---- FIRE ----")]
    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectile3;
    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleTop;
    [SerializeField] Transform muzzleBottom;
    [SerializeField, Range(0, 2)] int weaponPower = 0;
    [SerializeField] float fireInterval = 0.2f;

    [Header("---- DODGE ----")]
    [SerializeField, Range(0, 100)] int dodgeEnergyCost = 25;
    [SerializeField] float maxRoll = 720f;
    [SerializeField] float rollSpeed = 360f;
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);

    bool isDodging = false;

    float currentRoll;
    float dodgeDuration;

    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitHealthRegenerateTime;

    Coroutine moveCoroutine;
    Coroutine healthRegenerateCoroutine;
    
    new Rigidbody2D rigidbody;

    new Collider2D collider;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        dodgeDuration = maxRoll / rollSpeed;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
    }

    void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
    }

    void Start()
    {
        rigidbody.gravityScale = 0f;

        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);

        statsBar_HUD.Initialize(health, maxHealth);

        input.EnableGameplayInput();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        statsBar_HUD.UpdateStats(health, maxHealth);

        if (gameObject.activeSelf)
        {
            if (regenerateHealth)
            {
                if (healthRegenerateCoroutine != null)
                {
                    StopCoroutine(healthRegenerateCoroutine);
                }

                healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
            }
        }
    }

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        statsBar_HUD.UpdateStats(health, maxHealth);
    }

    public override void Die()
    {
        statsBar_HUD.UpdateStats(0f, maxHealth);
        base.Die();
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

        while (t < 1f)
        {
            t += Time.fixedDeltaTime / time;
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t);

            yield return null;
        }

        // while (t < time)
        // {
        //     t += Time.fixedDeltaTime;
        //     rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t / time);
        //     transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t / time);

        //     yield return null;
        // }
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

    #region DODGE
    void Dodge()
    {
        if (isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;

        StartCoroutine(nameof(DodgeCoroutine));
    }

    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        PlayerEnergy.Instance.Use(dodgeEnergyCost);
        collider.isTrigger = true;
        currentRoll = 0f;

        // Change player's scale 改变玩家的缩放值
        // * Method 01
        // var scale = transform.localScale;

        // while (currentRoll < maxRoll)
        // {
        //     currentRoll += rollSpeed * Time.deltaTime;
        //     transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

        //     if (currentRoll < maxRoll / 2f)
        //     {
        //         scale.x = Mathf.Clamp(scale.x - Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
        //         scale.y = Mathf.Clamp(scale.y - Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
        //         scale.z = Mathf.Clamp(scale.z - Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);
        //     }
        //     else 
        //     {
        //         scale.x = Mathf.Clamp(scale.x + Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
        //         scale.y = Mathf.Clamp(scale.y + Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
        //         scale.z = Mathf.Clamp(scale.z + Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);
        //     }

        //     transform.localScale = scale;

        //     yield return null;
        // }

        // * Method 02
        // var t1 = 0f;
        // var t2 = 0f;

        // while (currentRoll < maxRoll)
        // {
        //     currentRoll += rollSpeed * Time.deltaTime;
        //     transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

        //     if (currentRoll < maxRoll / 2f)
        //     {
        //         t1 += Time.deltaTime / dodgeDuration;
        //         transform.localScale = Vector3.Lerp(transform.localScale, dodgeScale, t1);
        //     }
        //     else 
        //     {
        //         t2 += Time.deltaTime / dodgeDuration;
        //         transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, t2);
        //     }

        //     yield return null;
        // }

        // * Method 3
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);
            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);

            yield return null;
        }

        collider.isTrigger = false;
        isDodging = false;
    }
    #endregion
}