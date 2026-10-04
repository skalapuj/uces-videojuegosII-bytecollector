using UnityEngine;

namespace ByteCollector.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class DataBit : MonoBehaviour
    {
        private BitSpawner spawner;

        public void Initialize(BitSpawner spawnerReference)
        {
            spawner = spawnerReference;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Collect();
            }
        }

        private void Collect()
        {
            if (spawner != null)
            {
                spawner.OnBitCollected();
            }

            Destroy(gameObject);
        }
    }
}