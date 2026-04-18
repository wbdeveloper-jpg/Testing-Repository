using UnityEngine;

/// <summary>
/// Applies device safe area to the attached RectTransform.
/// Designed for production use (orientation change + resolution change safe).
/// </summary>
[RequireComponent(typeof(RectTransform))]
public sealed class SafeArea : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect lastSafeArea = Rect.zero;
    private Vector2Int lastScreenSize = Vector2Int.zero;
    private ScreenOrientation lastOrientation = ScreenOrientation.AutoRotation;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    private void Update()
    {
        // Re-apply if something important changed
        if (Screen.safeArea != lastSafeArea ||
            Screen.width != lastScreenSize.x ||
            Screen.height != lastScreenSize.y ||
            Screen.orientation != lastOrientation)
        {
            ApplySafeArea();
        }
    }

    private void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        lastSafeArea = safeArea;
        lastScreenSize = new Vector2Int(Screen.width, Screen.height);
        lastOrientation = Screen.orientation;

        // Convert safe area rectangle from absolute pixels to normalized anchor values
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;

        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        // Ensure full stretch inside anchors
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}
