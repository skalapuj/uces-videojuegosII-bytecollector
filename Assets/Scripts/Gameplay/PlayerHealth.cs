using System.Collections;
using UnityEngine;
using ByteCollector.UI;
using ByteCollector.Commands;
using ByteCollector.Events;

namespace ByteCollector.Gameplay
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Integridad")]
        [SerializeField] private int maxLives = 3;
        [SerializeField] private int currentLives;

        [Header("Invulnerabilidad")]
        [SerializeField] private float invulnerabilityDuration = 1.0f;
        [SerializeField] private float blinkInterval = 0.1f;

        [Header("Referencias")]
        [SerializeField] private HUDController hudController;

        [Header("Eventos de UI (Observer)")]
        [SerializeField] private PopupEventSO popupChannel;

        [Header("Depuración Visual (Gizmos / DrawRay)")]
        [SerializeField] private bool showDebugGizmos = true;

        private Collider2D _playerCollider;
        private bool _isTakingHazardDamage;

        private SpriteRenderer spriteRenderer;
        private PlayerController playerController;
        private bool isInvulnerable = false;
        private bool isDead = false;

        public int CurrentLives => currentLives;
        public bool IsDead => isDead;

        private void Awake()
        {
            currentLives = maxLives;
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerController = GetComponent<PlayerController>();
            _playerCollider = GetComponent<Collider2D>();

            Debug.Assert(_playerCollider != null, $"[PlayerHealth] No se encontró Collider2D en {gameObject.name}.", this);
            Debug.Assert(popupChannel != null, $"[PlayerHealth] Falta asignar el canal 'popupChannel' en {gameObject.name}.", this);
            Debug.Assert(hudController != null, $"[PlayerHealth] Falta asignar 'hudController' en {gameObject.name}.", this);
        }

        private void Start()
        {
            if (hudController != null)
            {
                hudController.UpdateHealth(currentLives);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            CheckHazardCollision(collision.gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CheckHazardCollision(collision.gameObject);
        }

        private void CheckHazardCollision(GameObject other)
        {
            if (isDead || isInvulnerable) return;

            if (other.CompareTag("Hazard") || other.CompareTag("Glitch"))
            {
                _isTakingHazardDamage = true;

                // Traza una línea roja hacia el objeto 'other' y un rayo amarillo direccional durante 0.5s
                Debug.DrawLine(transform.position, other.transform.position, Color.red, 0.5f);
                Debug.DrawRay(transform.position, (other.transform.position - transform.position).normalized * 1.5f, Color.yellow, 0.5f);

                TakeDamage(1);
            }
        }

        public void TakeDamage(int damage)
        {
            if (isDead || isInvulnerable) return;

            currentLives = Mathf.Max(0, currentLives - damage);

            if (hudController != null)
            {
                hudController.UpdateHealth(currentLives);
            }

            if (currentLives <= 0)
            {
                TriggerGameOver();
            }
            else
            {
                StartCoroutine(InvulnerabilityRoutine());
            }
        }

        private IEnumerator InvulnerabilityRoutine()
        {
            isInvulnerable = true;
            float elapsed = 0f;

            Color originalColor = spriteRenderer != null ? spriteRenderer.color : Color.white;
            Color transparentColor = originalColor;
            transparentColor.a = 0.25f;

            while (elapsed < invulnerabilityDuration)
            {
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = (spriteRenderer.color.a == originalColor.a) ? transparentColor : originalColor;
                }
                yield return new WaitForSeconds(blinkInterval);
                elapsed += blinkInterval;
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
            isInvulnerable = false;
            _isTakingHazardDamage = false;
        }

        private void TriggerGameOver()
        {
            isDead = true;

            // Desactivar movimiento y frenar el Rigidbody
            if (playerController != null)
            {
                playerController.enabled = false;
            }

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }

            if (popupChannel != null)
            {
                PopupData deathPopupData = new PopupData(
                    "MEMORY OVERFLOW",
                    "¿Deseas reintentar?",
                    new RestartLevelCommand(),
                    new ReturnToMenuCommand()
                );

                popupChannel.Raise(deathPopupData);
            }
            else
            {
                Debug.LogWarning("[PlayerHealth] No se asignó el canal PopupEventSO en el Inspector.");
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            CheckHazardCollision(collision.gameObject);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            CheckHazardCollision(collision.gameObject);
        }

        private void OnDrawGizmos()
        {
            if (!showDebugGizmos) return;

            Collider2D col = _playerCollider != null ? _playerCollider : GetComponent<Collider2D>();
            if (col == null) return;

            // Verde si esta a salvo, Rojo si esta colisionando con un peligro
            Gizmos.color = _isTakingHazardDamage ? Color.red : Color.green;

            // Dibuja el volumen exacto del Bounds del Collider2D en la vista Scene
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        }
    }
}