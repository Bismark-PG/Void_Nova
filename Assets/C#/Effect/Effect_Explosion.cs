using UnityEngine;

public class FX_Manager : MonoBehaviour
{
    public static FX_Manager Instance;

    public GameObject ExplosionPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SpawnExplosion(Vector3 pos, string targetTag = null)
    {
        if (ExplosionPrefab == null) return;

        GameObject explosion = Instantiate(ExplosionPrefab, pos, Quaternion.identity);

        if (!string.IsNullOrEmpty(targetTag))
        {
            float Scale = 1f;

            switch (targetTag)
            {
                case "Main_Base":
                    Scale = 15f;
                    break;
                case "Sub_Base":
                    Scale = 10f;
                    break;
                case "Enemy":
                    Scale = 3f;
                    break;   
                case "Bullet":
                    Scale = 2f;
                    break;   
                case "Enemy_Bullet":
                    Scale = 1.5f;
                    break;
                case null:
                    Scale = 5f;
                    break;
            }

            explosion.transform.localScale = Vector3.one * Scale;
        }
    }
}