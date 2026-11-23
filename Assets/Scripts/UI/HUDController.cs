using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ByteCollector.Events;

namespace ByteCollector.UI
{
    public class HUDController : MonoBehaviour
    {
        [Header("Canales de Eventos (Observer)")]
        [SerializeField] private IntEventSO onHealthChangedChannel;

        [Header("Integridad / Vidas")]
        [SerializeField] private Image[] healthIcons;
        [SerializeField] private Color healthActiveColor = new Color(0.886f, 0.047f, 0.047f, 1.0f); // Rojo fuerte
        [SerializeField] private Color healthInactiveColor = new Color(0.2f, 0.2f, 0.2f, 0.4f); // Apagado

        [Header("Progreso de Bits (Buffer)")]
        [SerializeField] private TextMeshProUGUI bitProgressText;
        [SerializeField] private Slider bitProgressBar;

        [Header("Puntuación")]
        [SerializeField] private TextMeshProUGUI scoreText;

        private void Awake()
        {
            Debug.Assert(onHealthChangedChannel != null, $"[HUDController] Falta asignar 'onHealthChangedChannel' (IntEventSO) en {gameObject.name}.", this);
        }

        private void OnEnable()
        {
            if (onHealthChangedChannel != null)
            {
                onHealthChangedChannel.OnEventRaised += UpdateHealth;
            }
        }

        private void OnDisable()
        {
            if (onHealthChangedChannel != null)
            {
                onHealthChangedChannel.OnEventRaised -= UpdateHealth;
            }
        }

        public void UpdateHealth(int currentLives)
        {
            if (healthIcons == null) return;

            for (int i = 0; i < healthIcons.Length; i++)
            {
                if (healthIcons[i] != null)
                {
                    healthIcons[i].color = (i < currentLives) ? healthActiveColor : healthInactiveColor;
                }
            }
        }

        public void UpdateBitProgress(int currentBits, int targetBits)
        {
            if (bitProgressText != null)
            {
                bitProgressText.text = $"BITS: {currentBits}/{targetBits}";
            }

            if (bitProgressBar != null && targetBits > 0)
            {
                bitProgressBar.value = (float)currentBits / targetBits;
            }
        }

        public void UpdateScore(int newScore)
        {
            if (scoreText != null)
            {
                scoreText.text = $"SCORE: {newScore:D6}";
            }
        }
    }
}