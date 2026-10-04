using UnityEngine;

namespace ByteCollector.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Configuración de Movimiento")]
        [SerializeField] private float moveSpeed = 6f;

        [Header("Límites de la Arena")]
        [SerializeField] private bool useCameraBounds = true;
        [SerializeField] private Vector2 horizontalLimits = new Vector2(-8f, 8f);
        [SerializeField] private Vector2 verticalLimits = new Vector2(-4.5f, 4.5f);
        [SerializeField] private float boundaryPadding = 0.5f;

        private Rigidbody2D rb;
        private Vector2 movementInput;
        private Camera mainCamera;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            mainCamera = Camera.main;
        }

        private void Start()
        {
            if (useCameraBounds && mainCamera != null)
            {
                CalculateCameraBounds();
            }
        }

        private void Update()
        {
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

        private void CalculateCameraBounds()
        {
            if (!mainCamera.orthographic) return;

            float cameraHeight = mainCamera.orthographicSize;
            float cameraWidth = cameraHeight * mainCamera.aspect;

            horizontalLimits = new Vector2(-cameraWidth + boundaryPadding, cameraWidth - boundaryPadding);
            verticalLimits = new Vector2(-cameraHeight + boundaryPadding, cameraHeight - boundaryPadding);
        }

        public void SetSpeed(float newSpeed)
        {
            moveSpeed = newSpeed;
        }

        public float GetSpeed()
        {
            return moveSpeed;
        }
    }
}