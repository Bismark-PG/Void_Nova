using UnityEngine;

public class Player_Rotate : MonoBehaviour
{
    public float RotateSensitivity = 5.0f;
    private Player_Control Control;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        Control = GetComponent<Player_Control>();
    }

    void Update()
    {
        if (Control != null && Control.IsRotationLockedPublic)
            return;

        Vector3 MousePOS = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePOS.z = 0;

        Vector2 Direction = (MousePOS - transform.position).normalized;
        float Angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;

        // Follow MousePOS Quickly
        transform.rotation = Quaternion.Euler(0, 0, Angle);

        // Follow MousePOS Slowly
        /*
        Quaternion TargetRotation = Quaternion.Euler(0, 0, Angle);
        
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            TargetRotation,
            RotateSensitivity * Time.deltaTime
            );
        */
    }
}
