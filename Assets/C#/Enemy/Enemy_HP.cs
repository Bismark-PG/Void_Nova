using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int MaxHP = 5;
    private int CurrentHP;
    private int E_Score = 2500;

    private void Start()
    {
        CurrentHP = MaxHP;
    }

    public void EnemyDamaged(int Damage)
    {
        CurrentHP -= Damage;

        if (CurrentHP <= 0)
            Die();
    }
    private void Die()
    {
        StartCoroutine(E_Destroy());
    }

    private IEnumerator E_Destroy()
    {
        Enemy_Aim aim = GetComponent<Enemy_Aim>();
        if (aim != null)
            aim.IS_Fire = false;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            Color c = sprite.color;
            c.a = 0f;
            sprite.color = c;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        if (FX_Manager.Instance != null)
            FX_Manager.Instance.SpawnExplosion(transform.position, "Enemy");

        if (Score.Instance != null)
            Score.Instance.AddScore(E_Score);

        if (Ultimate_Controller.Instance != null)
            Ultimate_Controller.Instance.AddHit(10);

        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && audio.clip != null)
        {
            audio.Play();
            yield return new WaitForSeconds(audio.clip.length);
        }

        Destroy(gameObject);
    }
}