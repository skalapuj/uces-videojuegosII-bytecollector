using UnityEngine;

/// <summary>
/// Servicio genérico: instancia cualquier prefab en una casilla libre de la grilla.
/// No conoce Bits, Leaks ni Tiles; quien lo usa decide qué spawnear y cómo inicializarlo.
/// </summary>
public class GridSpawner : MonoBehaviour
{
    [SerializeField] private GridManager grid;
    [Tooltip("Padre opcional para mantener ordenada la Hierarchy")]
    [SerializeField] private Transform container;

    /// <param name="occupiesCell">true para objetos estáticos (Bit, Tile). false para los que se mueven (Leak): esos se registran solos vía GridMover.</param>
    /// <param name="awayFrom">Punto del que hay que alejarse (normalmente la casilla del jugador).</param>
    /// <returns>La instancia, o null si no hay casillas libres.</returns>
    public T Spawn<T>(T prefab, out Vector2Int cell, bool occupiesCell = true,
                      Vector2Int? awayFrom = null, int minDistance = 0) where T : Component
    {
        if (!grid.TryGetRandomFreeCell(out cell, awayFrom, minDistance))
        {
            return null;
        }

        T instance = Instantiate(prefab, grid.CellToWorld(cell), Quaternion.identity, container);

        if (occupiesCell)
        {
            instance.gameObject.AddComponent<CellOccupant>().Init(grid, cell);
        }

        return instance;
    }
}
