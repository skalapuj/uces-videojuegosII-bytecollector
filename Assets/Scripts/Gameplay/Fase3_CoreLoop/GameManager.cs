using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Dueño del estado global: bits, puntaje, ciclo, Memory Flush y Game Over.
/// NO conoce enemigos, HUD ni salud: expone eventos y los demás se suscriben.
/// </summary>
[DefaultExecutionOrder(100)]   // corre después del resto, así el Player ya está registrado en la grilla al spawnear el 1er Bit
public class GameManager : MonoBehaviour
{
    public enum GameState { Playing, Flushing, GameOver }

    [Header("Referencias")]
    [SerializeField] private GridSpawner spawner;
    [SerializeField] private Bit bitPrefab;

    [Header("Reglas")]
    [SerializeField, Min(1)] private int bitsPerByte = 8;
    [SerializeField] private int pointsPerBit = 10;
    [SerializeField] private int flushBonus = 1000;
    [SerializeField] private float flushDuration = 1.2f;
    [SerializeField] private bool debugLogs = true;

    public GameState State { get; private set; } = GameState.Playing;
    public int Score { get; private set; }
    public int BitsInCycle { get; private set; }
    public int Cycle { get; private set; } = 1;
    public int BitsPerByte => bitsPerByte;

    // ---- Eventos (los consumen HUD, GlitchDirector, GameOverPanel) ----
    public event Action<int> ScoreChanged;
    public event Action<int, int> BitsChanged;      // (bitsActuales, bitsRequeridos)
    public event Action<int> CycleChanged;
    public event Action<int, int> BitCollected;     // (bitsEnElCiclo, ciclo). No se dispara con el 8/8: ahí va el Flush.
    public event Action MemoryFlushStarted;
    public event Action<int> GameEnded;             // puntaje final

    private void Start()
    {
        ScoreChanged?.Invoke(Score);
        BitsChanged?.Invoke(BitsInCycle, bitsPerByte);
        CycleChanged?.Invoke(Cycle);
        SpawnBit();
    }

    private void SpawnBit()
    {
        Bit bit = spawner.Spawn(bitPrefab, out _);
        if (bit == null)
        {
            Debug.LogWarning("GameManager: no hay casillas libres para spawnear el Bit.");
            return;
        }
        bit.Collected += HandleBitCollected;
    }

    private void HandleBitCollected()
    {
        if (State != GameState.Playing) return;

        BitsInCycle++;
        AddScore(pointsPerBit);
        BitsChanged?.Invoke(BitsInCycle, bitsPerByte);
        Log($"Bit {BitsInCycle}/{bitsPerByte} (ciclo {Cycle})");

        if (BitsInCycle >= bitsPerByte)
        {
            StartCoroutine(MemoryFlushRoutine());
            return;
        }

        BitCollected?.Invoke(BitsInCycle, Cycle);   // el GlitchDirector spawnea peligros antes de que aparezca el próximo Bit
        SpawnBit();
    }

    private IEnumerator MemoryFlushRoutine()
    {
        State = GameState.Flushing;
        Log("MEMORY FLUSH");
        MemoryFlushStarted?.Invoke();               // GlitchDirector limpia enemigos, HUD hace el parpadeo
        AddScore(flushBonus);

        yield return new WaitForSeconds(flushDuration);

        if (State == GameState.GameOver) yield break;

        Cycle++;
        BitsInCycle = 0;
        CycleChanged?.Invoke(Cycle);
        BitsChanged?.Invoke(BitsInCycle, bitsPerByte);

        State = GameState.Playing;
        SpawnBit();
    }

    /// <summary>Lo llama PlayerHealth cuando las vidas llegan a 0.</summary>
    public void TriggerGameOver()
    {
        if (State == GameState.GameOver) return;

        State = GameState.GameOver;
        Log($"GAME OVER - puntaje {Score}");
        GameEnded?.Invoke(Score);
    }

    private void AddScore(int amount)
    {
        Score += amount;
        ScoreChanged?.Invoke(Score);
    }

    private void Log(string message)
    {
        if (debugLogs) Debug.Log($"[GameManager] {message}");
    }
}
