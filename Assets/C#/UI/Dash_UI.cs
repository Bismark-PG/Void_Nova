using UnityEngine;
using UnityEngine.UI;

public class Dash_UI : MonoBehaviour
{
    public Image CooldownMask;

    private float CooldownTime = 5f;
    private float Current = 0f;
    private bool IsCooldown = false;

    public void Initialize(float dashCooldown)
    {
        CooldownTime = dashCooldown;
        Current = CooldownTime;
        SetCooldownFill(0);
    }

    public void StartCooldown()
    {
        Current = 0f;
        IsCooldown = true;
        SetCooldownFill(1);
    }

    void Update()
    {
        if (!IsCooldown) return;

        Current += Time.deltaTime;
        float Rotation = Mathf.Clamp01(Current / CooldownTime);
        SetCooldownFill(1 - Rotation);

        if (Current >= CooldownTime)
        {
            IsCooldown = false;
            SetCooldownFill(0);
        }
    }

    private void SetCooldownFill(float amount)
    {
        if (CooldownMask != null)
            CooldownMask.fillAmount = amount;
    }
}