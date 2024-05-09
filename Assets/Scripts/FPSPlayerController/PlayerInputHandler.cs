using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    //TODO: Will need to expand with Input for Interaction and ESC key for pause 
    
    
    // This script acts as a central manager for input control, streamlining reference handling. 
    // It eliminates the need for lengthy references in individual scripts, allowing for more straightforward shorthand references.

    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string look = "Look";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string crouch = "Crouch";

    [Header("Action Name References")]
    [SerializeField] private float leftStickDeadzoneValue = 0.15f;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction crouchAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public float SprintValue { get; private set; }
    public bool CrouchTriggered { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);
        jumpAction = playerControls.FindActionMap(actionMapName).FindAction(jump);
        sprintAction = playerControls.FindActionMap(actionMapName).FindAction(sprint);
        crouchAction = playerControls.FindActionMap(actionMapName).FindAction(crouch);

        RegisterInputActions();

        InputSystem.settings.defaultDeadzoneMin = leftStickDeadzoneValue;
    }

    void RegisterInputActions()
    {
        // When the move action is performed, update MoveInput with the current Vector2 value.
        // Reset MoveInput to zero when the action is canceled.
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        // Update LookInput with the current Vector2 value when the look action is performed.
        // Reset LookInput to zero when the action is canceled.
        lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => LookInput = Vector2.zero;

        // Set JumpTriggered to true when the jump action is performed, and to false when the action is canceled.
        jumpAction.performed += context => JumpTriggered = true;
        jumpAction.canceled += context => JumpTriggered = false;
        
        // Update SprintValue with the current float value when the sprint action is performed.
        // Reset SprintValue to 0 when the action is canceled.
        sprintAction.performed += context => SprintValue = context.ReadValue<float>();
        sprintAction.canceled += context => SprintValue = 0f;

        //Set CrouchTriggered to true when the crouch action is performed, and to false when the action is canceled.
        crouchAction.performed += context => CrouchTriggered = true;
        crouchAction.canceled += context => CrouchTriggered = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
        crouchAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        sprintAction.Disable();
        crouchAction.Disable();
    }



}
