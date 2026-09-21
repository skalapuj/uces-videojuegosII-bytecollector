using UnityEngine;

/// <summary>
/// Marca una casilla como ocupada mientras el objeto exista y la libera sola al destruirse.
/// Lo agrega GridSpawner automáticamente: nadie tiene que acordarse de llamar a Release().
/// </summary>
public class CellOccupant : MonoBehaviour
{
    private GridManager grid;
    private Vector2Int cell;

    public void Init(GridManager gridManager, Vector2Int occupiedCell)
    {
        grid = gridManager;
        cell = occupiedCell;
        grid.Occupy(cell);
    }

    private void OnDestroy()
    {
        if (grid != null) grid.Release(cell);
    }
}
