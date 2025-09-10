using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHPUI : MonoBehaviour
{
    public Slider HpSlider;
    public Image FillImage;

    public Color NormalColor = Color.blue;
    public Color HitColor = Color.red;

    [SerializeField] private RawImage LifeImage1;
    [SerializeField] private RawImage LifeImage2;

    private Coroutine colorCoroutine;

    private void Start()
    {
        if (HpSlider != null)
            HpSlider.value = HpSlider.maxValue;

        if (FillImage != null)
            FillImage.color = NormalColor;
    }

    public void UpdateHP(float hpPercent)
    {
        if (HpSlider != null)
            HpSlider.value = hpPercent;
    }

    public void HitEffect(float duration = 0.5f)
    {
        if (colorCoroutine != null)
            StopCoroutine(colorCoroutine);

        colorCoroutine = StartCoroutine(HitEffectCoroutine(duration));
    }

    private IEnumerator HitEffectCoroutine(float duration)
    {
        if (FillImage == null)
            yield break;

        FillImage.color = HitColor;

        yield return new WaitForSeconds(duration);

        FillImage.color = NormalColor;
        colorCoroutine = null;
    }
    public void UpdateLifeUI(int Life)
    {
        if (LifeImage1 != null) LifeImage1.gameObject.SetActive(Life >= 2);
        if (LifeImage2 != null) LifeImage2.gameObject.SetActive(Life >= 3);
    }
}