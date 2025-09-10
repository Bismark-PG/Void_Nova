using UnityEngine;
using System.Collections;

public class Make_Bullet : MonoBehaviour
{
    public AudioClip FireSound;
    private AudioSource Source;
    public float VolumeScale = 0.3f;
    public float VolumePitch = 1.3f;

    public GameObject BulletPre;

    public float BulletSpeed = 15.0f;
    private float SpawnOffset = 0.5f;

    public float ReloadingTime = 0.25f;
    private float LastShotTime = 0.0f;

    public bool Fire = true;

    private void Start()
    {
        Source = GetComponent<AudioSource>();
        if (Source == null)
            Source = gameObject.AddComponent<AudioSource>();

        Source.pitch = VolumePitch;
        Source.playOnAwake = false;
    }

    private void Update()
    {
        if (!Fire) return;

        if (Input.GetKeyDown(KeyCode.Space) && Time.time - LastShotTime >= ReloadingTime)
        {
            LastShotTime = Time.time;
            Shoot();
        }
    }

    void Shoot()
    {
        StartCoroutine(ShootSound());
    }

    IEnumerator ShootSound()
    {
        if (FireSound != null && Source != null)
            Source.PlayOneShot(FireSound, VolumeScale);

        Vector3 SpawnPOS = transform.position + transform.right * SpawnOffset;
        GameObject Bullet = Instantiate(BulletPre, SpawnPOS, Quaternion.identity);

        Vector2 Shoot = transform.right.normalized;
        Rigidbody2D RB = Bullet.GetComponent<Rigidbody2D>();
        RB.linearVelocity = Shoot * BulletSpeed;

        float Angle = Mathf.Atan2(Shoot.y, Shoot.x) * Mathf.Rad2Deg;
        Bullet.transform.rotation = Quaternion.Euler(0, 0, Angle);

        yield return null;
    }
}
