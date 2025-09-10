using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    public int Damage = 1;
    public float BulletSpeed = 5f;
    public float LifeTime = 10f;

    private void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    private void Update()
    {
        transform.position += transform.right * BulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.CompareTag("Player"))
        {
            var PlayerHP = Collision.GetComponent<PlayerHealth>();
            var PlayerCtrl = Collision.GetComponent<Player_Control>();

            if (PlayerCtrl != null && PlayerCtrl.GetInvincible())
            {
                if (FX_Manager.Instance != null)
                    FX_Manager.Instance.SpawnExplosion(transform.position, "Enemy_Bullet");
                Destroy(gameObject);
                return;
            }
            if (PlayerHP != null)
            {
                if (FX_Manager.Instance != null)
                    FX_Manager.Instance.SpawnExplosion(transform.position, "Enemy_Bullet");
                PlayerHP.TakeDamage(Damage);
                Destroy(gameObject);
            }
        }
    }
}
