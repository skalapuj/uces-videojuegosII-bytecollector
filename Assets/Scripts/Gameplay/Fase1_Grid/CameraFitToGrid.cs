using UnityEngine;

/// <summary>
/// Encuadra la arena completa (sin scroll) en cualquier relación de aspecto.
/// Deja un margen superior extra para el HUD del Canvas.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraFitToGrid : MonoBehaviour
{
    [SerializeField] private GridManager grid;
    [SerializeField] private float paddingSides = 0.5f;
    [SerializeField] private float paddingTop = 1.25f;      // espacio reservado para el HUD
    [SerializeField] private float paddingBottom = 0.5f;

    private Camera cam;
    private int lastWidth;
    private int lastHeight;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
    }

    private void LateUpdate()
    {
        // Reencuadra solo cuando cambia la resolución (Game view, Simulator, ventana).
        if (Screen.width == lastWidth && Screen.height == lastHeight) return;
        lastWidth = Screen.width;
        lastHeight = Screen.height;
        Fit();
    }

    private void Fit()
    {
        Vector2 size = grid.WorldSize;

        float halfHeight = (size.y + paddingTop + paddingBottom) * 0.5f;
        float halfWidth = (size.x + paddingSides * 2f) * 0.5f / cam.aspect;
        cam.orthographicSize = Mathf.Max(halfHeight, halfWidth);

        Vector2 center = grid.WorldCenter;
        transform.position = new Vector3(center.x, center.y + (paddingTop - paddingBottom) * 0.5f, -10f);
    }
}
