using UnityEngine;
namespace ByteCollector.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class GlitchEnemy : MonoBehaviour
    {
        public static int ActiveCount { get; private set; }
        [Header("Persecución")]
        [SerializeField] private float speed = 2f;
        private Transform target;
        private Rigidbody2D rb;
        public void Initialize(Transform playerTransform)
        {
            target = playerTransform;
        }
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            GetComponent<Collider2D>().isTrigger = true;
        }
        private void OnEnable()
        {
            ActiveCount++;
            GameLoopManager.MemoryFlushed += HandleMemoryFlush;
        }
        private void OnDisable()
        {
            ActiveCount--;
            GameLoopManager.MemoryFlushed -= HandleMemoryFlush;
        }
        private void FixedUpdate()
        {
            if (target == null) return;
            Vector2 next = Vector2.MoveTowards(rb.position, target.position,
            speed * Time.fixedDeltaTime);
            rb.MovePosition(next);
        }
        private void HandleMemoryFlush()
        {
            Destroy(gameObject);
        }
    }
}