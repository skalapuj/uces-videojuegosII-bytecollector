using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ByteCollector.Commands;
using ByteCollector.Events;
using System;

namespace ByteCollector.UI
{
    public class GenericPopupView : MonoBehaviour
    {
        [Header("Textos")]
        [SerializeField] private TextMeshProUGUI txtTitle;
        [SerializeField] private TextMeshProUGUI txtMessage;

        [Header("Botones de Acción")]
        [SerializeField] private Button btnConfirm;
        [SerializeField] private Button btnCancel;

        private ICommand confirmCommand;
        private ICommand cancelCommand;

        public event Action OnPopupClosed;

        private void Awake()
        {
            if (btnConfirm != null)
            {
                btnConfirm.onClick.AddListener(HandleConfirmClicked);
            }

            if (btnCancel != null)
            {
                btnCancel.onClick.AddListener(HandleCancelClicked);
            }
        }

        private void OnDestroy()
        {
            if (btnConfirm != null)
            {
                btnConfirm.onClick.RemoveListener(HandleConfirmClicked);
            }

            if (btnCancel != null)
            {
                btnCancel.onClick.RemoveListener(HandleCancelClicked);
            }
        }

        public void Configure(PopupData data)
        {
            if (data == null) return;

            if (txtTitle != null) txtTitle.text = data.Title;
            if (txtMessage != null) txtMessage.text = data.Message;

            confirmCommand = data.ConfirmCommand;
            cancelCommand = data.CancelCommand;

            if (btnCancel != null)
            {
                btnCancel.gameObject.SetActive(cancelCommand != null);
            }
        }

        private void HandleConfirmClicked()
        {
            confirmCommand?.Execute();
            Close();
        }

        private void HandleCancelClicked()
        {
            cancelCommand?.Execute();
            Close();
        }

        public void Close()
        {
            OnPopupClosed?.Invoke();
            gameObject.SetActive(false);
        }
    }
}