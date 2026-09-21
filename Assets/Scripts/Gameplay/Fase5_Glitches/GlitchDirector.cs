using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Director de dificultad: escucha al GameManager y decide qué glitches aparecen.
/// - Cada N bits aparece una Corrupted Tile.
/// - Cada M bits aparece un Leak; M baja con cada ciclo y los Leaks se aceleran.
/// - Al Memory Flush destruye todo; al Game Over congela a los Leaks.
/// Toda la "receta" de dificultad vive acá, expuesta en el Inspector.
/// </summary>
public class GlitchDirector : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GridManager grid;
    [SerializeField] private GridSpawner spawner;
    [SerializeField] private PlayerController player;
    [SerializeField] private LeakEnemy leakPrefab;
    [SerializeField] private CorruptedTile tilePrefab;

    [Header("Spawn")]
    [SerializeField, Min(0)] private int safeDistanceFromPlayer = 3;
    [SerializeField, Min(1)] private int tileEveryNBits = 2;
    [SerializeField, Min(1)] private int leakEveryNBits = 3;     // en el ciclo 1; baja 1 por ciclo hasta 1
    [SerializeField, Min(1)] private int maxLeaks = 4;

    [Header("Velocidad del Leak (segundos por casilla)")]
    [SerializeField, Min(0.05f)] private float baseLeakStep = 0.55f;
    [SerializeField, Range(0f, 0.5f)] private float speedUpPerCycle = 0.08f;   // 8% más rápido por ciclo
    [SerializeField, Min(0.05f)] private float minLeakStep = 0.2f;

    private readonly List<LeakEnemy> leaks = new List<LeakEnemy>();
    private readonly List<CorruptedTile> tiles = new List<CorruptedTile>();

    private void OnEnable()
    {
        gameManager.BitCollected += HandleBitCollected;
        gameManager.MemoryFlushStarted += ClearAll;
        gameManager.GameEnded += HandleGameEnded;
    }

    private void OnDisable()
    {
        if (gameManager == null) return;
        gameManager.BitCollected -= HandleBitCollected;
        gameManager.MemoryFlushStarted -= ClearAll;
        gameManager.GameEnded -= HandleGameEnded;
    }

    private void HandleBitCollected(int bitsInCycle, int cycle)
    {
        if (bitsInCycle % tileEveryNBits == 0) SpawnTile();

        int leakInterval = Mathf.Max(1, leakEveryNBits - (cycle - 1));
        if (bitsInCycle % leakInterval == 0 && leaks.Count < maxLeaks) SpawnLeak(cycle);
    }

    private void SpawnTile()
    {
        CorruptedTile tile = spawner.Spawn(tilePrefab, out _, true,
                                           player.Mover.Cell, safeDistanceFromPlayer);
        if (tile != null) tiles.Add(tile);
    }

    private void SpawnLeak(int cycle)
    {
        Vector2Int cell;
        LeakEnemy leak = spawner.Spawn(leakPrefab, out cell, false,
                                       player.Mover.Cell, safeDistanceFromPlayer + 1);
        if (leak == null) return;

        leak.Init(grid, cell, player.Mover, GetLeakStep(cycle));
        leaks.Add(leak);
    }

    private float GetLeakStep(int cycle)
    {
        float step = baseLeakStep * Mathf.Pow(1f - speedUpPerCycle, cycle - 1);
        return Mathf.Max(minLeakStep, step);
    }

    // Memory Flush: se destruyen TODOS los glitches (Leaks y Tiles).
    private void ClearAll()
    {
        for (int i = 0; i < leaks.Count; i++) if (leaks[i] != null) Destroy(leaks[i].gameObject);
        for (int i = 0; i < tiles.Count; i++) if (tiles[i] != null) Destroy(tiles[i].gameObject);
        leaks.Clear();
        tiles.Clear();
    }

    // Game Over: los Leaks dejan de perseguir, pero quedan visibles en pantalla.
    private void HandleGameEnded(int finalScore)
    {
        for (int i = 0; i < leaks.Count; i++) if (leaks[i] != null) leaks[i].enabled = false;
    }
}
