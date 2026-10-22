using System;
using UnityEngine;

namespace ByteCollector.Events
{
    [CreateAssetMenu(fileName = "PopupEventChannel", menuName = "ByteCollector/Events/Popup Event Channel")]
    public class PopupEventSO : ScriptableObject
    {
        public event Action<PopupData> OnPopupRequested;

        public void Raise(PopupData data)
        {
            if (data == null)
            {
                Debug.LogWarning("[PopupEventSO] Se intentó disparar un evento de pop-up con datos nulos.");
                return;
            }

            OnPopupRequested?.Invoke(data);
        }
    }
}