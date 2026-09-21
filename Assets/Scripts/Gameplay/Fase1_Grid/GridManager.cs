using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fuente de verdad de la grilla lógica: tamaño, conversión celda <-> mundo y ocupación.
/// Usa el componente nativo Grid solo para la conversión de coordenadas.
/// Colocá este objeto en la esquina inferior izquierda de la arena (ej: X=-8, Y=-4.5 para 16x9).
/// </summary>
[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    [SerializeField, Min(1)] private int width = 16;
    [SerializeField, Min(1)] private int height = 9;

    private readonly HashSet<Vector2Int> staticCells = new HashSet<Vector2Int>();   // Bits, Corrupted Tiles
    private readonly List<IGridOccupant> occupants = new List<IGridOccupant>();     // Player, Leaks
    private readonly List<Vector2Int> freeBuffer = new List<Vector2Int>();
    private Grid unityGrid;

    public int Width => width;
    public int Height => height;

    private Grid UnityGrid
    {
        get
        {
            if (unityGrid == null) unityGrid = GetComponent<Grid>();
            return unityGrid;
        }
    }

    public Vector2 CellSize => UnityGrid.cellSize;
    public Vector2 WorldSize => new Vector2(width * CellSize.x, height * CellSize.y);
    public Vector2 WorldCenter => (Vector2)transform.position + WorldSize * 0.5f;

    // ---------- Coordenadas ----------
    public Vector3 CellToWorld(Vector2Int cell)
    {
        return UnityGrid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
    }

    public Vector2Int WorldToCell(Vector3 worldPosition)
    {
        Vector3Int c = UnityGrid.WorldToCell(worldPosition);
        return new Vector2Int(c.x, c.y);
    }

    public bool IsInside(Vector2Int cell)
    {
        return cell.x >= 0 && cell.x < width && cell.y >= 0 && cell.y < height;
    }

    // ---------- Ocupación ----------
    public void Occupy(Vector2Int cell) { staticCells.Add(cell); }
    public void Release(Vector2Int cell) { staticCells.Remove(cell); }

    public void Register(IGridOccupant occupant)
    {
        if (!occupants.Contains(occupant)) occupants.Add(occupant);
    }

    public void Unregister(IGridOccupant occupant) { occupants.Remove(occupant); }

    public bool IsFree(Vector2Int cell)
    {
        if (!IsInside(cell) || staticCells.Contains(cell)) return false;

        for (int i = 0; i < occupants.Count; i++)
        {
            if (occupants[i].Occupies(cell)) return false;
        }
        return true;
    }

    /// <summary>Elige una casilla libre al azar. Opcionalmente lejos de un punto (distancia Manhattan).</summary>
    public bool TryGetRandomFreeCell(out Vector2Int cell, Vector2Int? awayFrom = null, int minDistance = 0)
    {
        freeBuffer.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int candidate = new Vector2Int(x, y);
                if (!IsFree(candidate)) continue;
                if (awayFrom.HasValue && Manhattan(candidate, awayFrom.Value) < minDistance) continue;
                freeBuffer.Add(candidate);
            }
        }

        if (freeBuffer.Count == 0)
        {
            cell = default(Vector2Int);
            return false;
        }

        cell = freeBuffer[Random.Range(0, freeBuffer.Count)];
        return true;
    }

    private static int Manhattan(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    // ---------- Debug visual en Scene / Game (activar Gizmos) ----------
    private void OnDrawGizmos()
    {
        Vector2 cs = CellSize;
        Vector3 origin = transform.position;
        Gizmos.color = new Color(0f, 1f, 0.3f, 0.35f);

        for (int x = 0; x <= width; x++)
            Gizmos.DrawLine(origin + new Vector3(x * cs.x, 0f, 0f), origin + new Vector3(x * cs.x, height * cs.y, 0f));

        for (int y = 0; y <= height; y++)
            Gizmos.DrawLine(origin + new Vector3(0f, y * cs.y, 0f), origin + new Vector3(width * cs.x, y * cs.y, 0f));
    }
}
