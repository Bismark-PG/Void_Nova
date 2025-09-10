using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UltimateGaugeUI : MonoBehaviour
{
    public Slider Ultima_Gauge;
    public Image FillImage;

    public Color NormalColor = Color.blue;
    public Color ReadyColor = Color.red;

    private float Ultima_Count = 100;

    private void Start()
    {
        if (Ultima_Gauge != null)
        {
            Ultima_Gauge.maxValue = Ultima_Count;
            Ultima_Gauge.value = 0;
        }

        if (FillImage != null)
            FillImage.color = NormalColor;
    }

    public void UpdateHitCount(float currentHitCount)
    {
        if (Ultima_Gauge != null)
        {
            Ultima_Gauge.value = currentHitCount;

            if (FillImage != null)
            {
                if (currentHitCount >= Ultima_Count)
                    FillImage.color = ReadyColor;
                else
                    FillImage.color = NormalColor;
            }
        }
    }
}
