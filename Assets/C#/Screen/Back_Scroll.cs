using UnityEngine;

public class BackgroundScroll_Sprite : MonoBehaviour
{
    public float scrollSpeedX = 0.05f;
    public float scrollSpeedY = 0f;

    private Material mat;
    private Vector2 offset;

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        offset.x += scrollSpeedX * Time.deltaTime;
        offset.y += scrollSpeedY * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}