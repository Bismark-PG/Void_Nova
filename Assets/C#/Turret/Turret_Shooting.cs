using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    public GameObject BulletPre;
    public Transform Player;

    public float FireRate = 1.0f;
    public float FireRange = 15.0f;
    private float FireCooldown = 0f;
    void Start()
    {
        if (Player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                Player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player object not found in scene!");
            }
        }
    }

    private void LateUpdate()
    {
        if (Player == null)
        {
            Debug.LogWarning("Player is null");
            return;
        }

        float Distance = Vector3.Distance(transform.position, Player.position);
        FireCooldown -= Time.deltaTime;

        if (Distance <= FireRange && FireCooldown <= 0f)
        {
            Fire();
            FireCooldown = FireRate;
        }
    }

    void Fire()
    {
        Vector3 spawnPos = transform.position + transform.up * 1.0f;
        Instantiate(BulletPre, spawnPos, transform.rotation);
    }
}
