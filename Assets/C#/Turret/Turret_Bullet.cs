using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Turret_Bullet : MonoBehaviour
{
    public int Damage = 1;
    public float BulletSpeed = 5f;
    public float LifeTime = 10f;

    private void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    private void LateUpdate()
    {
        transform.position += transform.up * BulletSpeed * Time.deltaTime;
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
