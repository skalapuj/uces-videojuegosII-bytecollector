using UnityEngine;

namespace ByteCollector.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Configuración de Movimiento")]
        [SerializeField] private float moveSpeed = 6f;

        [Header("Márgenes de Seguridad (UI Safe Area)")]
        [SerializeField] private float topPadding = 1.0f;
        [SerializeField] private float bottomPadding = 1.2f;
        [SerializeField] private float sidePadding = 0.5f;

        private Rigidbody2D rb;
        private Vector2 movementInput;
        private Camera mainCamera;
        private Vector2 horizontalLimits;
        private Vector2 verticalLimits;
        private float lastAspect;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            mainCamera = Camera.main;
        }

        private void Start()
        {
            UpdateCameraBounds();
        }

        private void Update()
        {
            if (mainCamera != null && !Mathf.Approximately(mainCamera.aspect, lastAspect))
            {
                UpdateCameraBounds();
            }

            CaptureInput();
        }

        private void FixedUpdate()
        {
            MovePlayer();
            ClampPosition();
        }

        private void CaptureInput()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            movementInput = new Vector2(horizontal, vertical).normalized;
        }

        private void MovePlayer()
        {
            rb.velocity = movementInput * moveSpeed;
        }

        private void ClampPosition()
        {
            Vector3 currentPos = rb.position;
            float clampedX = Mathf.Clamp(currentPos.x, horizontalLimits.x, horizontalLimits.y);
            float clampedY = Mathf.Clamp(currentPos.y, verticalLimits.x, verticalLimits.y);

            rb.position = new Vector2(clampedX, clampedY);
        }

        public void UpdateCameraBounds()
        {
            if (mainCamera == null || !mainCamera.orthographic) return;

            lastAspect = mainCamera.aspect;
            float cameraHeight = mainCamera.orthographicSize;
            float cameraWidth = cameraHeight * mainCamera.aspect;

            horizontalLimits = new Vector2(-cameraWidth + sidePadding, cameraWidth - sidePadding);
            verticalLimits = new Vector2(-cameraHeight + bottomPadding, cameraHeight - topPadding);
        }

        public void SetSpeed(float newSpeed)
        {
            moveSpeed = newSpeed;
        }

        public float GetSpeed()
        {
            return moveSpeed;
        }

        public Vector2 GetHorizontalLimits() => horizontalLimits;
        public Vector2 GetVerticalLimits() => verticalLimits;
    }
}