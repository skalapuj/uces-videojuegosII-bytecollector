using System;
using UnityEngine;

/// <summary>
/// Coleccionable. Solo detecta al jugador y AVISA por evento; no sabe nada de puntajes ni del GameManager.
/// Requiere un Collider2D con Is Trigger activado.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Bit : MonoBehaviour
{
    public event Action Collected;

    private bool collected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || other.GetComponentInParent<PlayerController>() == null) return;

        collected = true;
        Collected?.Invoke();
        Destroy(gameObject);
    }
}
