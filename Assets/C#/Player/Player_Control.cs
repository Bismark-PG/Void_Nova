using UnityEngine;
using System.Collections;

public class Player_Control : MonoBehaviour
{
    [SerializeField] private SpriteRenderer BackgroundRenderer;
    [SerializeField] private Dash_UI DashUI;
    Bounds MapBounds;
    private Make_Bullet Bullet_Script;

    public GameObject Player;
    private SpriteRenderer SpriteBlink;
    private Coroutine BlinkCoroutine;
    public AudioClip UltimateCharge;
    public AudioClip UltimateFire;
    public AudioClip DashSFX;
    private AudioSource DashSource;
    private AudioSource UltimaSource;
    public float AudioVolume = 1.0f;

    public float PlayerSpeed = 10.0f;

    float MapMinX, MapMaxX;
    float MapMinY, MapMaxY;
    float P_ExtentX, P_ExtentY;

    public float DashRange = 5.0f;
    public float DashCooldown = 5.0f;
    public float DashDuration = 0.5f;
    private float DashCooldownTimer = 0f;
    private bool IsDashing = false;
    private Vector2 DashDirection;
    private float DashTimeLeft = 0f;

    private Effect_2D_Camera_Movement Camera_Script;

    public GameObject LaserPrefab;
    private bool IsUsingUltimate = false;
    private float UltTimer = 0f;
    public float UltDuration = 5f;
    private Vector3 LockedDirection;

    private bool IsRespawning = false;
    private bool IsInvincible = false;
    public bool GetInvincible() => IsInvincible;

    private bool IsRotationLocked = false;
    public bool IsRotationLockedPublic => IsRotationLocked;
    public bool IsDead { get; private set; } = false;

    private void Start()
    {
        GameObject CamObj = Camera.main.gameObject;
        Camera_Script = CamObj.GetComponent<Effect_2D_Camera_Movement>();
        Bullet_Script = GetComponent<Make_Bullet>();
        DashUI?.Initialize(DashCooldown);

        DashSource = GetComponent<AudioSource>();
        if (DashSource == null)
            DashSource = gameObject.AddComponent<AudioSource>();

        UltimaSource = GetComponent<AudioSource>();
        if (UltimaSource == null)
            UltimaSource = gameObject.AddComponent<AudioSource>();

        if (GetComponent<Rigidbody2D>() == null)
        {
            var RB = gameObject.AddComponent<Rigidbody2D>();
            RB.gravityScale = 0;
            RB.bodyType = RigidbodyType2D.Kinematic;
        }

        if (BackgroundRenderer != null)
        {
            MapBounds = BackgroundRenderer.bounds;
        }
        else
        {
            Debug.LogError("Can`t Find BackgroundRenderer.");
        }

        SpriteBlink = GetComponent<SpriteRenderer>();
        Bounds PlayerBounds = SpriteBlink.bounds;

        MapMinX = MapBounds.min.x;
        MapMaxX = MapBounds.max.x;
        MapMinY = MapBounds.min.y;
        MapMaxY = MapBounds.max.y;

        P_ExtentX = PlayerBounds.extents.x;
        P_ExtentY = PlayerBounds.extents.y;
    }

    private void Update()
    {
        if (!IsRotationLocked)
        {
            Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 Dir = (MousePos - transform.position).normalized;
            float Angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, Angle);
        }
        if (IsRespawning) return;

        if (DashCooldownTimer > 0f)
            DashCooldownTimer -= Time.deltaTime;

        if (IsDashing || IsUsingUltimate)
        {
            if (IsDashing)
            {
                float DashSpeed = PlayerSpeed * DashRange;
                transform.position += (Vector3)(DashDirection * DashSpeed * Time.deltaTime);

                Vector3 ClampedPos = transform.position;
                ClampedPos.x = Mathf.Clamp(ClampedPos.x, MapMinX + P_ExtentX, MapMaxX - P_ExtentX);
                ClampedPos.y = Mathf.Clamp(ClampedPos.y, MapMinY + P_ExtentY, MapMaxY - P_ExtentY);
                transform.position = ClampedPos;

                DashTimeLeft -= Time.deltaTime;

                if (DashTimeLeft <= 0f)
                {
                    IsDashing = false;
                    IsInvincible = false;

                    if (Camera_Script != null)
                        Camera_Script.IsDashing = false;

                    CheckOverlapWithEnemy();
                }
            }

            else if (IsUsingUltimate)
            {
                UltTimer -= Time.deltaTime;
                if (UltTimer <= 0f)
                {
                    if (Camera_Script != null)
                        Camera_Script.IsUltima = false;
                    EndUltimate();
                }
            }
            return;
        }

        Vector2 PlayerMove = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) PlayerMove.y += 1;
        if (Input.GetKey(KeyCode.S)) PlayerMove.y -= 1;
        if (Input.GetKey(KeyCode.A)) PlayerMove.x -= 1;
        if (Input.GetKey(KeyCode.D)) PlayerMove.x += 1;

        PlayerMove = PlayerMove.normalized;

        if (Input.GetKeyDown(KeyCode.R)
            && Ultimate_Controller.Instance.CanUseUltimate)
        {
            StartUltimate();
            if (Camera_Script != null)
                Camera_Script.IsUltima = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)
            && DashCooldownTimer <= 0f && PlayerMove != Vector2.zero)
        {
            StartCoroutine(DashSound());

            DashDirection = PlayerMove;
            IsDashing = true;
            DashTimeLeft = DashDuration;

            IsInvincible = true;
            DashCooldownTimer = DashCooldown;

            if (Camera_Script != null)
                Camera_Script.IsDashing = true;

            if (DashUI != null)
                DashUI.StartCooldown();

            return;
        }

        Vector3 PlayerPOS = transform.position;
        PlayerPOS += (Vector3)(PlayerMove * PlayerSpeed * Time.deltaTime);

        PlayerPOS.x = Mathf.Clamp(PlayerPOS.x, MapMinX + P_ExtentX, MapMaxX - P_ExtentX);
        PlayerPOS.y = Mathf.Clamp(PlayerPOS.y, MapMinY + P_ExtentY, MapMaxY - P_ExtentY);
        
        transform.position = PlayerPOS;
    }
    IEnumerator DashSound()
    {
        if (DashSFX != null && DashSource != null)
            DashSource.PlayOneShot(DashSFX, AudioVolume);
        yield return null;
    }
    void StartUltimate()
    {
        if (IsUsingUltimate) return;

        IsUsingUltimate = true;
        IsRotationLocked = true;
        IsInvincible = true;

        UltTimer = UltDuration;
        LockedDirection = transform.right.normalized;

        Ultimate_Controller.Instance.ConsumeUltimate();

        if (Bullet_Script != null)
            Bullet_Script.Fire = false;

        StartCoroutine(UltimateEffect());
    }

    IEnumerator UltimateEffect()
    {
        // Effect
        if (UltimateCharge != null && UltimaSource != null)
            UltimaSource.PlayOneShot(UltimateCharge);

        yield return new WaitForSeconds(2f);

        // Fireeeeeeeeeeeeeeeee
        Vector3 FireDirection = LockedDirection.normalized;
        Vector3 FirePosition = transform.position + FireDirection * 0.5f;

        GameObject Laser = Instantiate(LaserPrefab, FirePosition, Quaternion.identity);

        float Angle = Mathf.Atan2(FireDirection.y, FireDirection.x) * Mathf.Rad2Deg;
        Laser.transform.rotation = Quaternion.Euler(0f, 0f, Angle);

        // Effect
        if (Camera_Script != null)
            Camera_Script.ShakeCamera(3f, 0.5f);

        if (UltimateFire != null && UltimaSource != null)
            UltimaSource.PlayOneShot(UltimateFire);

        yield return new WaitForSeconds(UltDuration - 2f);

        // Done
        Destroy(Laser);
    }

    void EndUltimate()
    {
        IsUsingUltimate = false;
        IsRotationLocked = false;
        IsInvincible = false;

        if (Bullet_Script != null)
            Bullet_Script.Fire = true;
    }

    public void StartRebirth(float Interval)
    {
        if (BlinkCoroutine != null)
            StopCoroutine(BlinkCoroutine);

        BlinkCoroutine = StartCoroutine(RebirthForSeconds(Interval));
    }

    private IEnumerator RebirthForSeconds(float Sec)
    {
        IsRotationLocked = true;
        IsRespawning = true;
        IsInvincible = true;

        transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        BlinkCoroutine = StartCoroutine(BlinkEffect(Sec, 0.25f));
        yield return new WaitForSeconds(Sec);

        IsInvincible = false;
        IsRespawning = false;
        IsRotationLocked = false;
    }

    IEnumerator BlinkEffect(float Sec, float Interval)
    {
        float BlinkTimer = 0f;
        Color original = SpriteBlink.color;

        while (BlinkTimer < Sec)
        {
            SpriteBlink.color = new Color(original.r, original.g, original.b, 0.2f);
            yield return new WaitForSeconds(Interval / 2f);

            SpriteBlink.color = new Color(original.r, original.g, original.b, 1f);
            yield return new WaitForSeconds(Interval / 2f);

            BlinkTimer += Interval;
        }

        SpriteBlink.color = original;
    }
    public void LockRotation(float duration)
    {
        StartCoroutine(LockRotationCoroutine(duration));
    }

    private IEnumerator LockRotationCoroutine(float duration)
    {
        IsRotationLocked = true;
        yield return new WaitForSeconds(duration);
        IsRotationLocked = false;
    }
    void CheckOverlapWithEnemy()
    {
        Collider2D[] Hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);

        foreach (var Hit in Hits)
        {
            if (Hit.CompareTag("Enemy") ||
                Hit.CompareTag("Sub_Base") ||
                Hit.CompareTag("Main_Base") ||
                Hit.CompareTag("Turret"))
            {
                var PlayerHP = GetComponent<PlayerHealth>();
                if (PlayerHP != null)
                {
                    PlayerHP.InstantDeath();
                }

                return;
            }
        }
    }
    public void SetDead()
    {
        IsDead = true;
        enabled = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color color = sr.color;
            color.a = 0f;
            sr.color = color;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
    }
}
