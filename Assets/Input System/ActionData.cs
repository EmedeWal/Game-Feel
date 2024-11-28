using UnityEngine.InputSystem;
using UnityEngine;
using System;

namespace ShatterStep
{
    [CreateAssetMenu(fileName = "New Action", menuName = "Scriptable Object/Data/Action")]
    public class ActionData : ScriptableObject
    {
        [Header("SETTINGS")]
        public InputActionReference InputActionReference;
        public ActionType ActionType;

        public event Action Performed;
        public event Action Canceled;

        public void Bind()
        {
            if (InputActionReference != null)
            {
                InputActionReference.action.performed += i => Performed?.Invoke();
                InputActionReference.action.canceled += i => Canceled?.Invoke();
            }
        }

        public void Unbind()
        {
            if (InputActionReference != null)
            {
                InputActionReference.action.performed -= i => Performed?.Invoke();
                InputActionReference.action.canceled -= i => Canceled?.Invoke();
            }
        }
    }
}