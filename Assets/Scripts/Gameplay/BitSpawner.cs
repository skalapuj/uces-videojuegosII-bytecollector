using UnityEngine;
using ByteCollector.UI;

namespace ByteCollector.Gameplay
{
    public class BitSpawner : MonoBehaviour
    {
        [Header("HUD (Respaldo si no hay GameLoopManager)")]
        [SerializeField] private HUDController hudController;

        [Header("Configuración de Prefab")]
        [SerializeField] private GameObject dataBitPrefab;

        [Header("Estado")]
        [SerializeField] private int totalCollected = 0;

        private GameObject currentBit;
        private Camera mainCamera;

        private const float BIT_PADDING = 0.3f;
        private const float TOP_UI_RESERVE = 1.0f;
        private const float BOTTOM_UI_RESERVE = 1.0f;

        private void Awake()
        {
            totalCollected = 0;
            mainCamera = Camera.main;
        }

        private void Start()
        {
            // Solo inicializa el HUD manualmente si se prueba la escena de manera aislada sin el GameManager principal
            if (GameLoopManager.Instance == null && hudController != null)
            {
                hudController.UpdateBitProgress(0, 8);
                hudController.UpdateScore(0);
                hudController.UpdateHealth(3);
            }

            SpawnNextBit();
        }

        public void SpawnNextBit()
        {
            if (dataBitPrefab == null) return;

            Vector2 spawnPosition = GetSafeSpawnPosition();
            currentBit = Instantiate(dataBitPrefab, spawnPosition, Quaternion.identity);

            DataBit bitComponent = currentBit.GetComponent<DataBit>();
            if (bitComponent != null)
            {
                bitComponent.Initialize(this);
            }
        }

        public void OnBitCollected()
        {
            totalCollected++;

            if (GameLoopManager.Instance != null)
            {
                GameLoopManager.Instance.RegisterBitCollected();
            }
            else if (hudController != null)
            {
                // Respaldo de seguridad si se corre la escena sin GameManager
                int currentCycleBits = totalCollected % 8;
                int displayBits = (currentCycleBits == 0 && totalCollected > 0) ? 8 : currentCycleBits;
                hudController.UpdateBitProgress(displayBits, 8);
                hudController.UpdateScore(totalCollected * 100);
            }

            SpawnNextBit();
        }

        private Vector2 GetSafeSpawnPosition()
        {
            if (mainCamera == null) return Vector2.zero;

            float cameraHeight = mainCamera.orthographicSize;
            float cameraWidth = cameraHeight * mainCamera.aspect;

            float minX = -cameraWidth + BIT_PADDING + 0.4f;
            float maxX = cameraWidth - BIT_PADDING - 0.4f;
            float minY = -cameraHeight + BOTTOM_UI_RESERVE;
            float maxY = cameraHeight - TOP_UI_RESERVE;

            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);

            return new Vector2(randomX, randomY);
        }

        public int GetTotalCollected()
        {
            return totalCollected;
        }

        private void OnDrawGizmosSelected()
        {
            Camera cam = Camera.main;
            if (cam == null || !cam.orthographic) return;

            Gizmos.color = Color.green;
            float h = cam.orthographicSize;
            float w = h * cam.aspect;

            float minX = -w + BIT_PADDING + 0.4f;
            float maxX = w - BIT_PADDING - 0.4f;
            float minY = -h + BOTTOM_UI_RESERVE;
            float maxY = h - TOP_UI_RESERVE;

            Vector3 center = new Vector3((minX + maxX) * 0.5f, (minY + maxY) * 0.5f, 0);
            Vector3 size = new Vector3(Mathf.Abs(maxX - minX), Mathf.Abs(maxY - minY), 0);
            Gizmos.DrawWireCube(center, size);
        }
    }
}