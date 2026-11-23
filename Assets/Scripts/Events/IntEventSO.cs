using System;
using UnityEngine;

namespace ByteCollector.Events
{
    [CreateAssetMenu(fileName = "NewIntEvent", menuName = "ByteCollector/Events/Int Event")]
    public class IntEventSO : ScriptableObject
    {
        public event Action<int> OnEventRaised;

        public void Raise(int value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}