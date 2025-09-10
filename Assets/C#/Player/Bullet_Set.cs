using UnityEngine;

public class Bullet_Set : MonoBehaviour
{
    public int Damage = 1;
    public float LifeTime = 10f;

    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy_Bullet"))
        {
            Ultimate_Controller.Instance.AddHit(0.5f);
        }
        else if (collision.CompareTag("Enemy") ||
                 collision.CompareTag("Sub_Base") ||
                 collision.CompareTag("Main_Base"))
        {
            Ultimate_Controller.Instance.AddHit();
        }

        if (collision.CompareTag("Enemy_Bullet"))
        {
            Destroy(collision.gameObject);

            if (FX_Manager.Instance != null)
                FX_Manager.Instance.SpawnExplosion(transform.position, "Bullet");
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Enemy")
            || collision.CompareTag("Sub_Base")
            || collision.CompareTag("Main_Base"))
        {
            EnemyHealth enemyHP = collision.GetComponent<EnemyHealth>();
            Sub_Base_HP sbHP = collision.GetComponent<Sub_Base_HP>();
            Main_Base_HP mbHP = collision.GetComponent<Main_Base_HP>();

            if (FX_Manager.Instance != null)
                FX_Manager.Instance.SpawnExplosion(transform.position, "Bullet");

            if (enemyHP != null)
                enemyHP.EnemyDamaged(Damage);
            else if (sbHP != null)
                sbHP.SubBase_Damaged(Damage);
            else if (mbHP != null)
                mbHP.MainBase_Damaged(Damage);

            Destroy(gameObject);
        }
    }
}