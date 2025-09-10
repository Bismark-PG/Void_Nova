using UnityEngine;

public class TurretAiming : MonoBehaviour
{
    public Transform Player;

    public float RotateSensitivity = 2.0f;

    private void LateUpdate()
    {
        if (Player != null)
        {
            Vector2 Direction = Player.position - transform.position;
            float Angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg - 90f; ;
            Quaternion TargetRotation = Quaternion.Euler(0f, 0f, Angle);

            transform.rotation = Quaternion.Slerp(
                 transform.rotation,
                 TargetRotation,
                 RotateSensitivity * Time.deltaTime);
        }
        else
        {
            Debug.Log("Turret Aiming Failed");
            return;
        }
    }
}