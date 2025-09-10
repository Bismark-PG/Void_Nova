using UnityEngine;
using System.Collections;

public class Main_Base_HP : MonoBehaviour
{
    public int MaxHP = 50;
    private int CurrentHP;
    private int MB_Score = 10000;

    public GameObject HP_UI_Prefab;

    private void Start()
    {
        CurrentHP = MaxHP;
    }

    public void MainBase_Damaged(int Damage)
    {
        CurrentHP -= Damage;

        if (CurrentHP <= 0)
            Die();
    }

    private void Die()
    {
        StartCoroutine(MB_Destroy());
    }

    private IEnumerator MB_Destroy()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            Color c = sprite.color;
            c.a = 0f;
            sprite.color = c;
        }

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        if (FX_Manager.Instance != null)
            FX_Manager.Instance.SpawnExplosion(transform.position, "Main_Base");

        if (Score.Instance != null)
            Score.Instance.AddScore(MB_Score);

        if (Ultimate_Controller.Instance != null)
            Ultimate_Controller.Instance.AddHit(50);

        MiniMap Map = Object.FindAnyObjectByType<MiniMap>();
        if (Map != null)
            Map.Remove_Main_Base(transform.position);

        Spawn_Build.Base_Count--;

        if (Spawn_Build.Base_Count <= 0)
        {
            GameOver Win = FindAnyObjectByType<GameOver>();
            if (Win != null)
                Win.Game_Over();
        }

        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && audio.clip != null)
        {
            audio.Play();
            yield return new WaitForSeconds(audio.clip.length);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.CompareTag("Player"))
        {
            var PlayerHP = Other.GetComponent<PlayerHealth>();
            if (PlayerHP != null)
            {
                PlayerHP.InstantDeath();
            }
        }
    }
}