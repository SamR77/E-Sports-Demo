using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // TODO: Crouch speed not yet implemented
    // TODO: Look into issue with camera not correct height when crouching  https://www.youtube.com/watch?v=b7qdx9UtMt4


    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float SprintMultiplier = 2.0f;

    [Header("Jump Settings")]
    [SerializeField] private float JumpForce = 5.0f;
    [SerializeField] private float gravity = 9.81f;

    /*
    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float crouchTransitionTime = 5f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3 (0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3 (0, 0, 0);
    [SerializeField] private Vector3 standingPosition = new Vector3(0, 0.9f, 0);
    [SerializeField] private Vector3 crouchingPosition = new Vector3(0, 0.1f, 0);
    [SerializeField] private Image crouchON;
    [SerializeField] private Image crouchOFF;

    private bool isCrouching;
    private bool duringCrouchAnimation;
    */

    [Header("Look Sensitivity")]
    [SerializeField] private float lookSensitivity = 1.0f;
    [SerializeField] private float upDownRange = 80.0f;


    public Transform player;

    private CharacterController characterController;
    private Camera mainCamera;
    private PlayerInputHandler inputHandler;
    private Vector3 currentMovement;
    private float verticalRotation;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        inputHandler = PlayerInputHandler.Instance;
    }

    



private void Update()
    {
        HandleMovement();
        HandleRotation();
    }



    void HandleMovement()
    {
        // handle basic move
        
        float speed;
        if (inputHandler.SprintValue > 0)
        {   speed = walkSpeed * SprintMultiplier;   }
        else
        {   speed = walkSpeed * 1f;                 }
        // the above expressed as a ternary operator
        // float speed = walkSpeed * (inputHandler.SprintValue > 0 ? SprintMultiplier : 1f);

        Vector3 inputDirection = new Vector3(inputHandler.MoveInput.x, 0f, inputHandler.MoveInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);

        worldDirection.Normalize();

        currentMovement.x = worldDirection.x * speed;
        currentMovement.z = worldDirection.z * speed;

        // Handle Jumping
        HandleJumping();




        // Handle Crouching
        //HandleCrouch();

        //Apply movement
        if (characterController.isGrounded == false)
        {
            currentMovement.y -= gravity * Time.deltaTime;
        }
        characterController.Move(currentMovement * Time.deltaTime);


        //Input Debug
       // if (inputHandler.JumpTriggered) { Debug.Log("Jump pressed"); }
       // if (inputHandler.CrouchTriggered) { Debug.Log("Crouch pressed"); }



    }
    void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;

            if (inputHandler.JumpTriggered)
            {
                currentMovement.y = JumpForce;
            }
        }
        //else
        //{
        //    currentMovement.y -= gravity * Time.deltaTime;
        //}
    }

    /*
    void HandleCrouch()
    {
        float timeElapsed = 0;
       

        // Crouch 
        if (inputHandler.CrouchTriggered == true && characterController.isGrounded)
        {
            
            if (characterController.height > crouchHeight)
            {

                transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, crouchingPosition.y, crouchTransitionTime), transform.localPosition.z); 

                characterController.height = Mathf.Lerp(characterController.height, crouchHeight, crouchTransitionTime);
                characterController.center = Vector3.Lerp(characterController.center, crouchingCenter, crouchTransitionTime);

                if (characterController.height - 0.05f <= crouchHeight)
                {
                    //characterController.height = crouchHeight;
                    //characterController.center = crouchingCenter;
                    //transform.position = transform.localPosition = new Vector3(transform.localPosition.x, crouchingPosition.y, transform.localPosition.z); 
                }
            }
        }
        
        // Stand
        else if(inputHandler.CrouchTriggered == false)
        {            
            if (characterController.height < standingHeight)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, standingPosition.y, crouchTransitionTime), transform.localPosition.z);
                characterController.height = Mathf.Lerp(characterController.height, standingHeight, crouchTransitionTime );
                characterController.center = Vector3.Lerp(characterController.center, standingCenter, crouchTransitionTime );
                
                if (characterController.height + 0.05f >= standingHeight)
                {
                    //characterController.height = standingHeight;
                    //characterController.center = standingCenter;
                    //transform.localPosition = new Vector3(transform.localPosition.x, standingPosition.y, transform.localPosition.z);
                }
                
            }  
        }      
    }
    */

    void HandleRotation()
    {
        float mouseXRotation = inputHandler.LookInput.x * lookSensitivity;
        transform.Rotate(0, mouseXRotation, 0);

        verticalRotation -= inputHandler.LookInput.y * lookSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

    }


}
