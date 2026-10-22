using UnityEngine;
using ByteCollector.Events;

namespace ByteCollector.UI
{
    public class PopupManager : MonoBehaviour
    {
        [Header("Canal de Comunicación (Observer)")]
        [SerializeField] private PopupEventSO popupChannel;

        [Header("Vista del Pop-up")]
        [SerializeField] private GenericPopupView popupViewInstance;

        private void OnEnable()
        {
            if (popupChannel != null)
            {
                popupChannel.OnPopupRequested += ShowPopup;
            }

            if (popupViewInstance != null)
            {
                popupViewInstance.OnPopupClosed += HandlePopupClosed;
            }
        }

        private void OnDisable()
        {
            if (popupChannel != null)
            {
                popupChannel.OnPopupRequested -= ShowPopup;
            }

            if (popupViewInstance != null)
            {
                popupViewInstance.OnPopupClosed -= HandlePopupClosed;
            }
        }

        private void ShowPopup(PopupData data)
        {
            if (popupViewInstance == null || data == null) return;

            // Inyectamos la información y comandos a la vista
            popupViewInstance.Configure(data);

            // Activamos el pop-up asegurando que quede al frente del Canvas
            popupViewInstance.gameObject.SetActive(true);
            popupViewInstance.transform.SetAsLastSibling();

            // Congelamos la simulación física del juego
            Time.timeScale = 0f;
        }

        private void HandlePopupClosed()
        {
            // Reanudamos la escala temporal estándar
            Time.timeScale = 1f;
        }
    }
}