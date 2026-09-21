using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HUD pasivo: no calcula nada, solo refleja los eventos de GameManager y PlayerHealth.
/// Reutiliza los textos txt_Lives, txt_Bits y txt_Score que ya anclaste en 03_Gameplay.
/// </summary>
public class HudController : MonoBehaviour
{
    [Header("Fuentes de datos")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Textos")]
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text bitsText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text cycleText;      // opcional

    [Header("Efecto Memory Flush (opcional)")]
    [Tooltip("Image a pantalla completa con alpha 0. Desactivá su Raycast Target para no bloquear clics.")]
    [SerializeField] private Image flashImage;
    [SerializeField] private float flashDuration = 1f;
    [SerializeField, Min(1)] private int flashCount = 3;

    private void OnEnable()
    {
        gameManager.ScoreChanged += SetScore;
        gameManager.BitsChanged += SetBits;
        gameManager.CycleChanged += SetCycle;
        gameManager.MemoryFlushStarted += PlayFlash;
        playerHealth.LivesChanged += SetLives;
    }

    private void OnDisable()
    {
        if (gameManager != null)
        {
            gameManager.ScoreChanged -= SetScore;
            gameManager.BitsChanged -= SetBits;
            gameManager.CycleChanged -= SetCycle;
            gameManager.MemoryFlushStarted -= PlayFlash;
        }
        if (playerHealth != null) playerHealth.LivesChanged -= SetLives;
    }

    private void SetLives(int lives) { livesText.text = $"VIDAS: {lives}"; }
    private void SetBits(int current, int required) { bitsText.text = $"BITS: {current}/{required}"; }
    private void SetScore(int score) { scoreText.text = $"PUNTAJE: {score}"; }

    private void SetCycle(int cycle)
    {
        if (cycleText != null) cycleText.text = $"CICLO {cycle:00}";
    }

    private void PlayFlash()
    {
        if (flashImage != null) StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float half = flashDuration / (flashCount * 2f);

        for (int i = 0; i < flashCount; i++)
        {
            SetFlashAlpha(0.55f);
            yield return new WaitForSeconds(half);
            SetFlashAlpha(0f);
            yield return new WaitForSeconds(half);
        }
    }

    private void SetFlashAlpha(float alpha)
    {
        Color c = flashImage.color;
        c.a = alpha;
        flashImage.color = c;
    }
}
