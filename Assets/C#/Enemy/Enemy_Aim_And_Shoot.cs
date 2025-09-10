using UnityEngine;

public class Enemy_Aim : MonoBehaviour
{
    public GameObject BulletPre;
    public Transform Player;

    public float FireRate = 1f;
    public float FireRange = 10f;
    private float FireCooldown = 0f;

    public bool IS_Fire = false;

    void Start()
    {
        if (Player == null)
        {
            GameObject obj = GameObject.FindWithTag("Player");

            if (obj != null)
                Player = obj.transform;

        }

        IS_Fire = true;
    }

    private void Update()
    {
        if (Player == null) return;

        float Distance = Vector3.Distance(transform.position, Player.position);
        FireCooldown -= Time.deltaTime;

        if (Distance <= FireRange && FireCooldown <= 0f && IS_Fire)
        {
            Shoot();
            FireCooldown = FireRate;
        }
    }
    void Shoot()
    {
        GameObject Bullet = Instantiate(BulletPre, transform.position,transform.rotation);
    }
}