using UnityEngine;

/// <summary>
/// Daña al jugador mientras lo toca. Se reutiliza tal cual en Leak y en Corrupted Tile.
/// Usa OnTriggerStay2D (no Enter) para que, si el jugador queda quieto encima cuando termina
/// la invulnerabilidad, vuelva a recibir daño. PlayerHealth se encarga de ignorar los golpes
/// mientras es invulnerable.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    [SerializeField, Min(1)] private int damage = 1;

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
        if (health != null) health.TakeDamage(damage);
    }
}
