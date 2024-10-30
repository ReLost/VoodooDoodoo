using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VoodooDoodoo.Utils.Input
{
    public sealed class InputController : MonoBehaviour
    {
        public static event Action<bool> OnPrimaryActionClickEvent;
        
        public static Vector2 PrimaryActionPosition { get; private set; }

        public void OnPrimaryClickAction (InputAction.CallbackContext context)
        {
            if (context.phase is InputActionPhase.Started or InputActionPhase.Canceled)
            {
                bool newValue = context.phase == InputActionPhase.Started;
                OnPrimaryActionClickEvent?.Invoke(newValue);
            }
        }

        public void OnPrimaryPositionAction (InputAction.CallbackContext context)
        {
            if (context.performed == true)
            {
                PrimaryActionPosition = context.ReadValue<Vector2>();
            }
        }
    }
}