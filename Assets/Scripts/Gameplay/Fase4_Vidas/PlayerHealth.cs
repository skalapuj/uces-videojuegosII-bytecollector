using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Vidas ("Integridad") del jugador. Recibe daño, da invulnerabilidad temporal con parpadeo
/// y avisa al GameManager cuando llega a 0.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxLives = 3;
    [SerializeField, Min(0f)] private float invulnerabilityTime = 1f;
    [SerializeField, Min(0.02f)] private float blinkInterval = 0.1f;

    [Header("Referencias")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private PlayerController controller;
    [SerializeField] private GameManager gameManager;

    public int Lives { get; private set; }
    public bool IsInvulnerable { get; private set; }

    public event Action<int> LivesChanged;
    public event Action Died;

    private void Awake()
    {
        Lives = maxLives;
        if (sprite == null) sprite = GetComponentInChildren<SpriteRenderer>();
        if (controller == null) controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        LivesChanged?.Invoke(Lives);   // valor inicial para el HUD
    }

    public void TakeDamage(int amount = 1)
    {
        if (IsInvulnerable || Lives <= 0) return;

        Lives = Mathf.Max(0, Lives - amount);
        LivesChanged?.Invoke(Lives);

        if (Lives == 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvulnerabilityRoutine());
    }

    private void Die()
    {
        SetAlpha(1f);
        if (controller != null) controller.enabled = false;   // corta el input
        Died?.Invoke();
        if (gameManager != null) gameManager.TriggerGameOver();
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        IsInvulnerable = true;
        float endTime = Time.time + invulnerabilityTime;
        bool faded = false;

        while (Time.time < endTime)
        {
            faded = !faded;
            SetAlpha(faded ? 0.25f : 1f);
            yield return new WaitForSeconds(blinkInterval);
        }

        SetAlpha(1f);
        IsInvulnerable = false;
    }

    private void SetAlpha(float alpha)
    {
        if (sprite == null) return;
        Color c = sprite.color;
        c.a = alpha;
        sprite.color = c;
    }
}
