using UnityEngine;

namespace ByteCollector.Gameplay
{
    public class BitSpawner : MonoBehaviour
    {
        [Header("Configuración de Prefab")]
        [SerializeField] private GameObject dataBitPrefab;

        [Header("Área de Spawn (Coordenadas de Arena)")]
        [SerializeField] private Vector2 xBounds = new Vector2(-7.5f, 7.5f);
        [SerializeField] private Vector2 yBounds = new Vector2(-4f, 4f);

        [Header("Estado")]
        [SerializeField] private int totalCollected = 0;

        private GameObject currentBit;

        private void Start()
        {
            SpawnNextBit();
        }

        public void SpawnNextBit()
        {
            if (dataBitPrefab == null) return;

            Vector2 spawnPosition = GetRandomSpawnPosition();
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
            Debug.Log($"[BitSpawner] Bit recolectado. Total acumulado: {totalCollected}");
            SpawnNextBit();
        }

        private Vector2 GetRandomSpawnPosition()
        {
            float randomX = Random.Range(xBounds.x, xBounds.y);
            float randomY = Random.Range(yBounds.x, yBounds.y);
            return new Vector2(randomX, randomY);
        }

        public int GetTotalCollected()
        {
            return totalCollected;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Vector3 center = new Vector3((xBounds.x + xBounds.y) * 0.5f, (yBounds.x + yBounds.y) * 0.5f, 0);
            Vector3 size = new Vector3(Mathf.Abs(xBounds.y - xBounds.x), Mathf.Abs(yBounds.y - yBounds.y), 0);
            Gizmos.DrawWireCube(center, size);
        }
    }
}