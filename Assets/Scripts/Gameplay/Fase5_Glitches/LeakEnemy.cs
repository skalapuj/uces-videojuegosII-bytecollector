using UnityEngine;

/// <summary>
/// Leak: persigue al jugador casilla por casilla a velocidad constante.
/// Solo decide DÓNDE ir; el GridMover ejecuta el movimiento y el Hazard hace el daño.
/// </summary>
[RequireComponent(typeof(GridMover), typeof(Hazard))]
public class LeakEnemy : MonoBehaviour
{
    private GridMover mover;
    private GridMover target;

    private void Awake()
    {
        mover = GetComponent<GridMover>();
    }

    /// <summary>Lo llama el GlitchDirector justo después de instanciarlo.</summary>
    public void Init(GridManager grid, Vector2Int startCell, GridMover chaseTarget, float stepDuration)
    {
        target = chaseTarget;
        mover.StepDuration = stepDuration;
        mover.Init(grid, startCell);
    }

    private void Update()
    {
        if (target == null || mover.IsMoving) return;

        Vector2Int direction = GetChaseDirection();
        if (direction != Vector2Int.zero) mover.TryMove(direction);
    }

    // Se acerca por el eje con mayor distancia (en empate, horizontal).
    private Vector2Int GetChaseDirection()
    {
        Vector2Int delta = target.Cell - mover.Cell;
        if (delta == Vector2Int.zero) return Vector2Int.zero;

        bool horizontal = Mathf.Abs(delta.x) >= Mathf.Abs(delta.y);

        return horizontal
            ? new Vector2Int((int)Mathf.Sign(delta.x), 0)
            : new Vector2Int(0, (int)Mathf.Sign(delta.y));
    }
}
