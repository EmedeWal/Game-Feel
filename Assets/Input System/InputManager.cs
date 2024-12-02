using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

namespace ShatterStep
{
    public class InputManager : SingletonBase
    {
        #region Singleton
        public static InputManager Instance { get; private set; }

        public override void Initialize()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(Instance);
            }
        }
        #endregion

        [Header("INPUT ACTION ASSET")]
        [SerializeField] private InputActionAsset _inputActions;

        [Header("ENABLED ACTIONS")]
        [SerializeField] private List<ActionData> _actionDataList;

        public float HorizontalInput { get; private set; }

        public event Action PauseInputPerformed;

        public void Setup()
        {
            _inputActions.FindAction("Horizontal").performed += i => HorizontalInput = i.ReadValue<float>();
            _inputActions.FindAction("Horizontal").canceled += i => HorizontalInput = i.ReadValue<float>();

            _inputActions.FindAction("Pause").performed += i => PauseInputPerformed?.Invoke();

            foreach (var action in _actionDataList)
            {
                action.Bind();
            }

            _inputActions.Enable();
        }

        public void Cleanup()
        {
            _inputActions.Disable();

            foreach (var action in _actionDataList)
            {
                action.Unbind();
            }

            _inputActions.FindAction("Horizontal").performed -= i => HorizontalInput = i.ReadValue<float>();
            _inputActions.FindAction("Horizontal").canceled -= i => HorizontalInput = i.ReadValue<float>();

            _inputActions.FindAction("Pause").performed -= i => PauseInputPerformed?.Invoke();
        }

        public bool GetAction(ActionType actionType, out ActionData actionData)
        {
            actionData = _actionDataList.Find(action => action.ActionType == actionType);
            if (actionData)
            {
                return true;
            }
            return false;
        }
    }
}