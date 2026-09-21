using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/// <summary>
/// Traduce WASD / flechas a una dirección y se la pasa al GridMover.
/// Usa el Input System nuevo (Keyboard.current), coherente con el InputSystemUIInputModule de la UI.
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

        // Prioridad: tecla recién presionada; si no hay, la que se mantiene apretada.
        Vector2Int direction = bufferedDirection != Vector2Int.zero ? bufferedDirection : ReadDirection(false);
        bufferedDirection = Vector2Int.zero;

        if (direction != Vector2Int.zero) Mover.TryMove(direction);
    }

    private static Vector2Int ReadDirection(bool onlyThisFrame)
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return Vector2Int.zero;

        if (Pressed(kb.wKey, onlyThisFrame) || Pressed(kb.upArrowKey, onlyThisFrame)) return Vector2Int.up;
        if (Pressed(kb.sKey, onlyThisFrame) || Pressed(kb.downArrowKey, onlyThisFrame)) return Vector2Int.down;
        if (Pressed(kb.aKey, onlyThisFrame) || Pressed(kb.leftArrowKey, onlyThisFrame)) return Vector2Int.left;
        if (Pressed(kb.dKey, onlyThisFrame) || Pressed(kb.rightArrowKey, onlyThisFrame)) return Vector2Int.right;

        return Vector2Int.zero;
    }

    private static bool Pressed(ButtonControl key, bool onlyThisFrame)
    {
        return onlyThisFrame ? key.wasPressedThisFrame : key.isPressed;
    }
}
