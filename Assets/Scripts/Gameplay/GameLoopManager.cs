using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ByteCollector.UI;

namespace ByteCollector.Gameplay
{
    public class GameLoopManager : MonoBehaviour
    {
        public static GameLoopManager Instance { get; private set; }

        [Header("Referencias de Escena")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private HUDController hudController;
        [SerializeField] private Camera mainCamera;

        [Header("UI de Notificacion de Sector")]
        [SerializeField] private TextMeshProUGUI txtSectorNotification;
        [SerializeField] private Image imgScreenFlash;

        [Header("Configuracion de Sectores")]
        [SerializeField] private int sector1Target = 8;
        [SerializeField] private int sector2Target = 16;
        [SerializeField] private Color sector1BgColor = new Color(0.05f, 0.05f, 0.08f, 1f);
        [SerializeField] private Color sector2BgColor = new Color(0.1f, 0.05f, 0.12f, 1f);

        [Header("Estado")]
        [SerializeField] private int currentSector = 1;
        [SerializeField] private int currentBits = 0;
        [SerializeField] private int totalScore = 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        private void Start()
        {
            currentSector = 1;
            currentBits = 0;
            totalScore = 0;

            if (mainCamera != null)
            {
                mainCamera.backgroundColor = sector1BgColor;
            }

            if (hudController != null)
            {
                hudController.UpdateBitProgress(currentBits, sector1Target);
                hudController.UpdateScore(totalScore);
            }

            if (imgScreenFlash != null)
            {
                imgScreenFlash.color = new Color(1f, 1f, 1f, 0f);
            }

            StartCoroutine(ShowSectorNotification("SECTOR 01: CACHE"));
        }

        public void RegisterBitCollected()
        {
            currentBits++;
            totalScore += 100;

            int targetForSector = (currentSector == 1) ? sector1Target : sector2Target;

            if (hudController != null)
            {
                hudController.UpdateBitProgress(currentBits, targetForSector);
                hudController.UpdateScore(totalScore);
            }

            if (currentSector == 1 && currentBits >= sector1Target)
            {
                StartCoroutine(TriggerMemoryFlushRoutine());
            }
        }

        private IEnumerator TriggerMemoryFlushRoutine()
        {
            currentSector = 2;
            totalScore += 1000; // Bonificación Memory Flush

            if (hudController != null)
            {
                hudController.UpdateScore(totalScore);
                hudController.UpdateBitProgress(currentBits, sector2Target);
            }

            // Incrementar velocidad del jugador en un 30%
            if (playerController != null)
            {
                float newSpeed = playerController.GetSpeed() * 1.30f;
                playerController.SetSpeed(newSpeed);
            }

            // Efecto Memory Flush: Destello blanco
            if (imgScreenFlash != null)
            {
                yield return StartCoroutine(FlashScreenRoutine());
            }

            // Cambio sutil de fondo de la arena
            if (mainCamera != null)
            {
                mainCamera.backgroundColor = sector2BgColor;
            }

            yield return StartCoroutine(ShowSectorNotification("SECTOR 02: MEMORIA PRINCIPAL"));
        }

        private IEnumerator FlashScreenRoutine()
        {
            imgScreenFlash.gameObject.SetActive(true);
            float duration = 0.35f;
            float elapsed = 0f;

            // Fade in a blanco
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 0.7f, elapsed / duration);
                imgScreenFlash.color = new Color(1f, 1f, 1f, alpha);
                yield return null;
            }

            elapsed = 0f;
            // Fade out
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0.7f, 0f, elapsed / duration);
                imgScreenFlash.color = new Color(1f, 1f, 1f, alpha);
                yield return null;
            }

            imgScreenFlash.color = new Color(1f, 1f, 1f, 0f);
        }

        private IEnumerator ShowSectorNotification(string message)
        {
            if (txtSectorNotification == null) yield break;

            txtSectorNotification.gameObject.SetActive(true);
            txtSectorNotification.text = message;
            txtSectorNotification.alpha = 1f;

            yield return new WaitForSeconds(2.0f);

            float fadeDuration = 0.5f;
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                txtSectorNotification.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                yield return null;
            }

            txtSectorNotification.gameObject.SetActive(false);
        }
    }
}