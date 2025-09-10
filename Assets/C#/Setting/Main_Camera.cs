using UnityEngine;
using System.Collections;

public class Effect_2D_Camera_Movement : MonoBehaviour 
{
    [SerializeField] private SpriteRenderer BackgroundRenderer;
    public GameObject Target;
    public float follow_speed = 4.0f;

    private float z = -10.0f;

    private Transform Target_Transform;
    private Bounds MapBounds;
    private float Cam_HalfWidth, Cam_HalfHeight;

    public float Default_CameraSize = 8f;
    public float Dash_ZoomOut_CameraSize = 12f;
    public float Ultima_ZoomOut_CameraSize = 50f;
    public float ZoomSpeed = 5f;

    public bool IsDashing = false;
    public bool IsUltima = false;

    void Start()
    {
        if (Target != null)
            Target_Transform = Target.GetComponent<Transform>();

        if (BackgroundRenderer != null)
        {
            MapBounds = BackgroundRenderer.bounds;
        }
        else
        {
            Debug.LogError("Can`t Find BackgroundRenderer.");
        }

        Cam_HalfHeight = Camera.main.orthographicSize;
        Cam_HalfWidth = Cam_HalfHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        float TargetSize = Default_CameraSize;

        if (IsUltima)
            TargetSize = Ultima_ZoomOut_CameraSize;
        else if (IsDashing)
            TargetSize = Dash_ZoomOut_CameraSize;

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, TargetSize, ZoomSpeed * Time.deltaTime);

        Cam_HalfWidth = Camera.main.orthographicSize;
        Cam_HalfWidth = Cam_HalfHeight * Camera.main.aspect;

        Vector3 TargetPos = Vector2.Lerp(transform.position,
            Target_Transform.position, follow_speed * Time.deltaTime);
        TargetPos.z = z;

        float MinX = MapBounds.min.x + Cam_HalfWidth;
        float MaxX = MapBounds.max.x - Cam_HalfWidth;
        float MinY = MapBounds.min.y + Cam_HalfHeight;
        float MaxY = MapBounds.max.y - Cam_HalfHeight;

        TargetPos.x = Mathf.Clamp(TargetPos.x, MinX, MaxX);
        TargetPos.y = Mathf.Clamp(TargetPos.y, MinY, MaxY);

        transform.position = TargetPos;

    }

    public void ShakeCamera(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }
    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        Vector3 OriginalPos = transform.position;

        float Elapsed = 0.0f;

        while (Elapsed < duration)
        {
            float OffsetX = Random.Range(-1f, 1f) * magnitude;
            float OffsetY = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(OriginalPos.x + OffsetX, OriginalPos.y + OffsetY, OriginalPos.z);

            Elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = OriginalPos;
    }
}