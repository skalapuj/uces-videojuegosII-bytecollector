using System.Collections;
using UnityEngine;
namespace ByteCollector.Gameplay
{
    public class GlitchSpawner : MonoBehaviour
    {
        [Header("Referencias")]
        [SerializeField] private Transform player;
        [SerializeField] private GlitchEnemy leakPrefab;
        [SerializeField] private GameObject corruptedTilePrefab;

        [Header("Sector 1 (Caché)")]
        [SerializeField] private float sector1LeakInterval = 10f;
        [SerializeField] private int sector1MaxLeaks = 1;

        [Header("Sector 2 (RAM) - más presión")]
        [SerializeField] private float sector2LeakInterval = 5f;
        [SerializeField] private int sector2MaxLeaks = 3;
        [SerializeField] private float tileInterval = 3f;

        [Header("Seguridad")]
        [SerializeField] private float minSpawnDistanceFromPlayer = 3f;
        [SerializeField] private float topUiReserve = 1.0f;
        [SerializeField] private float bottomUiReserve = 1.2f;
        private Camera cam;
        private Coroutine leakRoutine;
        private Coroutine tileRoutine;

        private void Awake()
        {
            cam = Camera.main;
        }
        // Lo llama GameLoopManager al empezar cada sector
        public void SetSector(int sector)
        {
            if (leakRoutine != null) StopCoroutine(leakRoutine);
            if (tileRoutine != null) StopCoroutine(tileRoutine);
            if (sector == 1)
            {
                leakRoutine = StartCoroutine(LeakLoop(sector1LeakInterval, sector1MaxLeaks));
            }
            else
            {
                leakRoutine = StartCoroutine(LeakLoop(sector2LeakInterval, sector2MaxLeaks));
                tileRoutine = StartCoroutine(TileLoop());
            }
        }
        private IEnumerator LeakLoop(float interval, int maxLeaks)
        {
            while (true)
            {
                yield return new WaitForSeconds(interval);
                if (leakPrefab != null && player != null && GlitchEnemy.ActiveCount < maxLeaks)
                {
                    GlitchEnemy leak = Instantiate(leakPrefab, GetSpawnPosition(),
                    Quaternion.identity);
                    leak.Initialize(player);
                }
            }
        }
        private IEnumerator TileLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(tileInterval);
                if (corruptedTilePrefab != null)
                {
                    Instantiate(corruptedTilePrefab, GetSpawnPosition(), Quaternion.identity);
                }
            }
        }
        private Vector2 GetSpawnPosition()
        {
            if (cam == null) return Vector2.zero;
            float h = cam.orthographicSize;
            float w = h * cam.aspect;
            for (int i = 0; i < 10; i++)
            {
                Vector2 pos = new Vector2(
                Random.Range(-w + 0.5f, w - 0.5f),
                Random.Range(-h + bottomUiReserve, h - topUiReserve));
                if (player == null ||
                Vector2.Distance(pos, player.position) >= minSpawnDistanceFromPlayer)
                    return pos;
            }
            return new Vector2(w - 0.5f, h - topUiReserve);
        }
    }
}