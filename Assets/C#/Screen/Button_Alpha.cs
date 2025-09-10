using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAlphaFader : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverAlpha = 0.8f;

    private Image image;
    private Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        if (image != null)
            originalColor = image.color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image != null)
        {
            Color faded = originalColor;
            faded.a = hoverAlpha;
            image.color = faded;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (image != null)
            image.color = originalColor;
    }
}