using UnityEngine;

/// <summary>
/// Traduce WASD / flechas a una dirección y se la pasa al GridMover.
/// Buffer de 1 dirección: si tocás una tecla mientras se mueve, el próximo paso no se pierde.
/// </summary>
[RequireComponent(typeof(GridMover))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GridManager grid;
    [SerializeField] private Vector2Int startCell = new Vector2Int(8, 4);

    private Vector2Int bufferedDirection;

    public GridMover Mover { get; private set; }

    private void Awake()
    {
        Mover = GetComponent<GridMover>();
    }

    private void Start()
    {
        Mover.Init(grid, startCell);
    }

    private void Update()
    {
        Vector2Int pressed = ReadDirection(true);
        if (pressed != Vector2Int.zero) bufferedDirection = pressed;

        if (Mover.IsMoving) return;

        Vector2Int direction = bufferedDirection != Vector2Int.zero ? bufferedDirection : ReadDirection(false);
        bufferedDirection = Vector2Int.zero;

        if (direction != Vector2Int.zero) Mover.TryMove(direction);
    }

    private static Vector2Int ReadDirection(bool onlyThisFrame)
    {
        if (Pressed(KeyCode.W, onlyThisFrame) || Pressed(KeyCode.UpArrow, onlyThisFrame)) return Vector2Int.up;
        if (Pressed(KeyCode.S, onlyThisFrame) || Pressed(KeyCode.DownArrow, onlyThisFrame)) return Vector2Int.down;
        if (Pressed(KeyCode.A, onlyThisFrame) || Pressed(KeyCode.LeftArrow, onlyThisFrame)) return Vector2Int.left;
        if (Pressed(KeyCode.D, onlyThisFrame) || Pressed(KeyCode.RightArrow, onlyThisFrame)) return Vector2Int.right;

        return Vector2Int.zero;
    }

    private static bool Pressed(KeyCode key, bool onlyThisFrame)
    {
        return onlyThisFrame ? Input.GetKeyDown(key) : Input.GetKey(key);
    }
}