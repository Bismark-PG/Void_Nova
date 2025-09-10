using UnityEngine;

public class Ultima_Set : MonoBehaviour
{
    public int Damage = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy_Bullet"))
        {

            if (FX_Manager.Instance != null)
                FX_Manager.Instance.SpawnExplosion(transform.position, "Enemy_Bullet");
            Destroy(collision.gameObject);
        }

        if(collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHP = collision.GetComponent<EnemyHealth>();

            if (enemyHP != null)
                enemyHP.EnemyDamaged(Damage);
        }

        if (collision.CompareTag("Sub_Base")
            || collision.CompareTag("Main_Base"))
        {
            Sub_Base_HP sbHP = collision.GetComponent<Sub_Base_HP>();
            Main_Base_HP mbHP = collision.GetComponent<Main_Base_HP>();

            if (sbHP != null)
                sbHP.SubBase_Damaged(Damage);
            else if (mbHP != null)
                mbHP.MainBase_Damaged(Damage);
        }
    }
}