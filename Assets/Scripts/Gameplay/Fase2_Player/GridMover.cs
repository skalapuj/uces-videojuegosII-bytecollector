using UnityEngine;

/// <summary>
/// Motor de movimiento por casillas, reutilizable por Player y Leak.
/// No sabe QUIÉN lo controla: solo expone TryMove(dirección).
/// Usa Rigidbody2D Kinematic + MovePosition para que los triggers se detecten bien.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class GridMover : MonoBehaviour, IGridOccupant
{
    [Tooltip("Segundos que tarda en cruzar UNA casilla (menor = más rápido)")]
    [SerializeField, Min(0.02f)] private float stepDuration = 0.12f;

    private Rigidbody2D rb;
    private GridManager grid;
    private Vector2 from;
    private Vector2 to;
    private float progress;

    public Vector2Int Cell { get; private set; }          // casilla donde está (o de donde salió)
    public Vector2Int TargetCell { get; private set; }    // casilla a la que va
    public bool IsMoving { get; private set; }

    public float StepDuration
    {
        get { return stepDuration; }
        set { stepDuration = Mathf.Max(0.02f, value); }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;   // necesario para que OnTriggerStay2D siga llegando
    }

    public void Init(GridManager gridManager, Vector2Int startCell)
    {
        grid = gridManager;
        Cell = startCell;
        TargetCell = startCell;
        IsMoving = false;

        Vector3 worldPos = grid.CellToWorld(startCell);
        transform.position = worldPos;
        rb.position = worldPos;

        grid.Register(this);
    }

    public bool Occupies(Vector2Int cell)
    {
        return cell == Cell || cell == TargetCell;
    }

    /// <summary>Inicia un paso hacia la dirección dada. Devuelve false si está en movimiento o se sale de la grilla.</summary>
    public bool TryMove(Vector2Int direction)
    {
        if (grid == null || IsMoving || direction == Vector2Int.zero) return false;

        Vector2Int target = Cell + direction;
        if (!grid.IsInside(target)) return false;

        TargetCell = target;
        from = rb.position;
        to = grid.CellToWorld(target);
        progress = 0f;
        IsMoving = true;
        return true;
    }

    private void FixedUpdate()
    {
        if (!IsMoving) return;

        progress += Time.fixedDeltaTime / stepDuration;

        if (progress >= 1f)
        {
            rb.MovePosition(to);
            Cell = TargetCell;
            IsMoving = false;
            return;
        }

        rb.MovePosition(Vector2.Lerp(from, to, progress));
    }

    private void OnDestroy()
    {
        if (grid != null) grid.Unregister(this);
    }
}
