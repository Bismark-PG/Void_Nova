using UnityEngine;

public class AimUIController : MonoBehaviour
{
    public RectTransform Aim;
    public Camera MainCamera;
    public Canvas Canvas;

    void Update()
    {
        if (MainCamera == null || Canvas == null || Aim == null)
        {
            Debug.LogWarning("MainCamera, Canvas or Aim is not assigned!");
            return;
        }

        Vector2 mousePos = Input.mousePosition;

        Vector2 localPoint;
        RectTransform canvasRect = Canvas.GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle
            (canvasRect, mousePos, MainCamera, out localPoint))
            Aim.localPosition = localPoint;
    }
}