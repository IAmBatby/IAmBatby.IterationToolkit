#if INPUTSYSTEM_PRESENT

using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public enum ActionType { Idle, Started, Performed, Canceled }
namespace IterationToolkit.InputSystem
{
    [SerializeField] //Don't need in future tbh
    public class ExtendedInputAction
    {
        public InputAction InputAction { get; private set; }
        [field: SerializeField] public CallbackContext PreviousContext { get; private set; }
        [field: SerializeField] public ActionType PreviousAction { get; private set; }
        public ExtendedEvent<CallbackContext> OnContextChanged = new ExtendedEvent<CallbackContext>();

        private bool hasConsumableInput;


        public ExtendedInputAction(string displayName, InputType inputType, string inputIdentifier)
        {
            InputAction = InputUtilities.CreateInputAction(displayName, inputType, inputIdentifier);
            ListenToEvents(InputAction);
        }

        public ExtendedInputAction(InputAction newInputAction)
        {
            InputAction = newInputAction;
            if (InputAction.enabled == false)
                InputAction.Enable();
            ListenToEvents(InputAction);
        }

        private void ListenToEvents(InputAction action)
        {
            action.started += OnActionEvent;
            action.performed += OnActionEvent;
            action.canceled += OnActionEvent;
        }

        public bool TryConsumeInput()
        {
            if (hasConsumableInput == true)
            {
                hasConsumableInput = false;
                return (true);
            }
            return (false);
        }

        private void OnActionEvent(CallbackContext context)
        {
            CallbackContext previousContext = PreviousContext;
            PreviousContext = context;

            if (previousContext.performed == true)
                PreviousAction = ActionType.Performed;
            else if (previousContext.started == true)
                PreviousAction = ActionType.Started;
            else if (previousContext.canceled == true)
                PreviousAction = ActionType.Canceled;
            else
                PreviousAction = ActionType.Idle;

            if (PreviousAction == ActionType.Performed)
                hasConsumableInput = true;

            if (PreviousAction == ActionType.Idle || PreviousAction == ActionType.Canceled)
                hasConsumableInput = false;

            OnContextChanged.Invoke(context);
        }
    }
}

#endif
