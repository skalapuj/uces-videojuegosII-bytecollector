using System.Collections;
using UnityEngine;
using ByteCollector.UI;

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
        [SerializeField] private GameObject gameOverPanel;

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
        }

        private void Start()
        {
            if (hudController != null)
            {
                hudController.UpdateHealth(currentLives);
            }

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(false);
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

            // Desplegar modal de Game Over
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

            Time.timeScale = 0f; // Pausa física del juego
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            CheckHazardCollision(collision.gameObject);
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            CheckHazardCollision(collision.gameObject);
        }
    }
}