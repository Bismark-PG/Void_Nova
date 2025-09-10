using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private GameOver Over;
    public AudioClip DeadSound;
    private AudioSource Source;
    public float VolumeScale = 0.8f;

    public int MaxHP = 10;
    private int CurrentHP;
    public int Life = 3;
    private float RebirthTime = 2.0f;

    public PlayerHPUI HP_UI;
    private Player_Control PlayerCtrl;

    private void Start()
    {
        CurrentHP = MaxHP;
        PlayerCtrl = GetComponent<Player_Control>();
        Over = FindAnyObjectByType<GameOver>();
        Source = GetComponent<AudioSource>();

        if (HP_UI != null)
        {
            HP_UI.HpSlider.maxValue = MaxHP;
            HP_UI.HpSlider.value = CurrentHP;
            HP_UI.FillImage.color = HP_UI.NormalColor;

            HP_UI.UpdateLifeUI(Life);
        }
        if (Source == null)
            Source = gameObject.AddComponent<AudioSource>();
    }

    public void TakeDamage(int Damage)
    {
        if (PlayerCtrl != null && PlayerCtrl.GetInvincible())
            return;

        CurrentHP -= Damage;
        if (CurrentHP < 0) CurrentHP = 0;

        if (HP_UI != null)
        {
            HP_UI.HpSlider.value = CurrentHP;
            HP_UI.HitEffect();
        }

        if (CurrentHP <= 0)
        {
            Life--;

            if (HP_UI != null)
                HP_UI.UpdateLifeUI(Life);

            if (Life > 0)
            {
                StartCoroutine(Dead());

                transform.position = Vector3.zero;
                CurrentHP = MaxHP;

                if (HP_UI != null)
                {
                    HP_UI.HpSlider.value = CurrentHP;
                    HP_UI.FillImage.color = HP_UI.NormalColor;
                }

                PlayerCtrl.StartRebirth(RebirthTime);
            }
            else
            {
                Die();
            }
        }
    }

    public void InstantDeath()
    {
        if (PlayerCtrl != null && PlayerCtrl.GetInvincible())
            return;

        Life--;

        if (HP_UI != null)
            HP_UI.UpdateLifeUI(Life);

        if (Life > 0)
        {
            StartCoroutine(Dead());

            transform.position = Vector3.zero;
            CurrentHP = MaxHP;

            if (HP_UI != null)
            {
                HP_UI.HpSlider.value = CurrentHP;
                HP_UI.FillImage.color = HP_UI.NormalColor;
            }

            PlayerCtrl.StartRebirth(RebirthTime);
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        StartCoroutine(Dead());

        if (PlayerCtrl != null)
            PlayerCtrl.SetDead();

        Over.Game_Over();
    }

    private IEnumerator Dead()
    {

        if (FX_Manager.Instance != null)
            FX_Manager.Instance.SpawnExplosion(transform.position);

        if (DeadSound != null && Source != null)
        {
            Source.PlayOneShot(DeadSound, VolumeScale);
            yield return new WaitForSeconds(DeadSound.length);
        }
    }
}