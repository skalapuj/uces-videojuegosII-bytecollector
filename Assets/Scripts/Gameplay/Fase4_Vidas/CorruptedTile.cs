using System.Collections;
using UnityEngine;

/// <summary>
/// Casilla corrupta estática. Nace "desarmada" y parpadea en rojo (telegrafía el peligro),
/// después se arma: recién ahí su collider empieza a dañar. Evita que el jugador reciba daño injusto.
/// </summary>
[RequireComponent(typeof(Collider2D), typeof(Hazard))]
public class CorruptedTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField, Min(0f)] private float warningTime = 1.5f;
    [SerializeField, Min(0.05f)] private float blinkInterval = 0.15f;
    [SerializeField] private Color armedColor = new Color(1f, 0.23f, 0.23f, 1f);
    [SerializeField] private Color warningColor = new Color(1f, 0.23f, 0.23f, 0.25f);

    private Collider2D hitbox;

    private void Awake()
    {
        hitbox = GetComponent<Collider2D>();
        hitbox.enabled = false;
        if (sprite == null) sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private IEnumerator Start()
    {
        float elapsed = 0f;
        bool bright = false;

        while (elapsed < warningTime)
        {
            bright = !bright;
            sprite.color = bright ? armedColor : warningColor;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        sprite.color = armedColor;
        hitbox.enabled = true;
    }
}
