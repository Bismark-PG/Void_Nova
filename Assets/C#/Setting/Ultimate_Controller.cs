using UnityEngine;

public class Ultimate_Controller : MonoBehaviour
{
    public static Ultimate_Controller Instance;

    public float HitCount = 0;
    public float RequiredHits = 100;
    public bool CanUseUltimate = false;

    public UltimateGaugeUI ultimateGaugeUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        if (ultimateGaugeUI != null)
        {
            ultimateGaugeUI.UpdateHitCount((int)HitCount);
        }
    }

    public void AddHit(float amount = 1f)
    {
        if (CanUseUltimate) return;

        HitCount += amount;

        if (HitCount >= RequiredHits)
        {
            HitCount = RequiredHits;
            CanUseUltimate = true;
        }

        if (ultimateGaugeUI != null)
        {
            ultimateGaugeUI.UpdateHitCount((int)HitCount);
        }
    }

    public void ConsumeUltimate()
    {
        HitCount = 0;
        CanUseUltimate = false;

        if (ultimateGaugeUI != null)
        {
            ultimateGaugeUI.UpdateHitCount((int)HitCount);
        }
    }
}
