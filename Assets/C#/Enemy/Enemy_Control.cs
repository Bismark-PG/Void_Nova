using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public Transform Player;
    public Sprite EnemySprite;

    public float RotateSensitivity = 2.0f;

    void Start()
    {
        var RB = gameObject.AddComponent<Rigidbody2D>();
        RB.gravityScale = 0;
        RB.bodyType = RigidbodyType2D.Kinematic;

        var SR = gameObject.AddComponent<SpriteRenderer>();
        SR.sprite = EnemySprite;
        SR.sortingOrder = 1;

        var Poly = gameObject.AddComponent<PolygonCollider2D>();
        Poly.isTrigger = true;

        transform.localScale = new Vector3(-0.7f, 0.7f, 1);
    }

    void Update()
    {
        if (Player != null)
        {
            Vector2 Direction = (Player.position - transform.position).normalized;
            float Angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            Quaternion TargetRotation = Quaternion.Euler(0, 0, Angle);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                TargetRotation,
                RotateSensitivity * Time.deltaTime);
        }
        else
        {
            Debug.Log("Enemy Control Failed");
            return;
        }
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