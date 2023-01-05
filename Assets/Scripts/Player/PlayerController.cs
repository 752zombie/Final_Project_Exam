using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, ISaveable
{
    [Tooltip("The cinemachine camera will follow this target")]
    public Transform CinemachineFollowTarget;

    [Tooltip("What layers should be considered ground for the player")]
    [SerializeField]
    private LayerMask GroundLayers;

    [Tooltip("Offset to use for ground detection from players pivot point")]
    [SerializeField]
    private float GroundSphereOffsetY = 0.24f;

    [Tooltip("Offset to use for feet ground detection from feets position")]
    [SerializeField]
    private float FeetSphereOffsetY = 0.08f;
    
    [Tooltip("How strongly should the character be affected by gravity. Lower values means stronger gravity")]
    public float PlayerGravity = -15;
    
    [Tooltip("How high in meters should the character be able to jump")]
    public float JumpHeight = 1.5f;
    
    [Tooltip("Position of character's left foot. Used for more precise ground detection")]
    [SerializeField]
    private Transform LeftFoot;

    [Tooltip("Position of character's right foot. Used for more precise ground detection")]
    [SerializeField]
    private Transform RightFoot;
    
    [Tooltip("How fast the character rotates")]
    [SerializeField]
    private float RotationSpeed = 400;

    [Tooltip("The base movement speed of the character")]
    [SerializeField]
    private float MoveSpeed = 7;

    [Tooltip("The walk speed relative to the base movement speed")]
    [Range(0, 1)]
    [SerializeField]
    private float WalkSpeedModifier = 0.3f;

    [Tooltip("How many seconds should pass after jumping before the character is able to jump again")]
    public float JumpTimeout = 0;

    [Tooltip("How many seconds should pass before the character begins fall animation")]
    [SerializeField]
    private float FallTimeut = 0.3f;

    [Range(-180, 180)]
    [SerializeField]
    private float MinCameraAngleX = -40;

    [Range(-180, 180)]
    [SerializeField]
    private float MaxCameraAngleX = 60;

    // references to other components on the same GameObject    
    private CharacterController characterController;
    private Animator animator;
    private Health health;

    // movement and jumping related fields
    private float verticalVelocity = 0;
    private Quaternion targetRotation;
    private float moveSpeedModifier = 1;
    private bool isWalking = false;
    private bool isGrounded = true;
    private bool feetIsGrounded = true;
    private float lastGroundedTime;

    private float jumpTimeoutDelta = 0;
    private float fallTimeoutDelta = 0;
    private float terminalVelocity = -53;
    
    // camera fields
    private float cameraRotationX = 0;
    private float cameraRotationY = 0;

    // animation related
    private int PlayerSpeedAnimId;
    private int IsFallingAnimId;
    private int JumpStartAnimId;
    private int IsGroundedAnimId;
    private int FeetGroundedAnimId;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        // Unity's built in CharacterController component will be used to perform the actual movement of the player
        characterController = GetComponent<CharacterController>();
        
        // cache animation ids for better performance
        PlayerSpeedAnimId = Animator.StringToHash("PlayerSpeed");
        IsFallingAnimId = Animator.StringToHash("IsFalling");
        JumpStartAnimId = Animator.StringToHash("JumpStart");
        IsGroundedAnimId = Animator.StringToHash("IsGrounded");
        FeetGroundedAnimId = Animator.StringToHash("FeetGrounded");
    }

    void Start()
    {
        lastGroundedTime = Time.time;
    }

    void Update()
    {
        if (GameStateManager.IsPaused)
        {
            return;
        }
        JumpAndGravity();
        GroundedCheck();
        Move();
    }

    private void LateUpdate()
    {
        if (GameStateManager.IsPaused)
        {
            return;
        }

        RotateCamera();
    }

    private void Move()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput);

        // used for the idle, walk, run animation blend, as negative values should have the same weight
        float absoluteMovement = Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleWalk();
        }

        // The movement direction is relative to the camera
        Vector3 moveDirection = Quaternion.Euler(0, cameraRotationY, 0) * (Time.deltaTime * MoveSpeed * moveSpeedModifier * inputVector.normalized);
        characterController.Move(moveDirection + (new Vector3(0, verticalVelocity, 0) * Time.deltaTime));

        if (absoluteMovement - 0.01 > 0)
        {
            targetRotation = Quaternion.LookRotation(moveDirection);
        }

        // Perform target rotation over time
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);

        // This will control the idle, walk, run animation blend
        animator.SetFloat(PlayerSpeedAnimId, Mathf.Clamp01(absoluteMovement) * moveSpeedModifier);
        
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        cameraRotationY += mouseX * ControlSettings.CameraSensitivityXNormal;
        cameraRotationX -= mouseY;
        
        // We want to prevent overflow and underflow of float value
        if (cameraRotationY < 0)
        {
            cameraRotationY += 360;
        }

        else if (cameraRotationY > 360)
        {
            cameraRotationY -= 360; 
        }

        // Prevent camera from rotating all the way around the x-axis
        cameraRotationX = Mathf.Clamp(cameraRotationX, MinCameraAngleX, MaxCameraAngleX);

        CinemachineFollowTarget.transform.rotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0);
    }

    private void JumpAndGravity()
    {
        if (isGrounded)
        {
            animator.SetBool(IsFallingAnimId, false);
            animator.SetBool(JumpStartAnimId, false);
            animator.SetBool(IsGroundedAnimId, isGrounded);

            fallTimeoutDelta = FallTimeut;

            lastGroundedTime = Time.time;
            
            // Helps to keep grounded when walking on slopes
            // CharacterController will prevent character from going through ground anyway
            if (verticalVelocity < 0)
            {
                verticalVelocity = -2;
            }
            
            // Jump
            if ((Input.GetButtonDown("Jump")) && feetIsGrounded && jumpTimeoutDelta <= 0)
            {
                // This formula should make the character jump to the desired height
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2 * PlayerGravity);

                animator.SetBool(JumpStartAnimId, true);
                
            }

            if (jumpTimeoutDelta > 0)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
             
        }

        else
        {
            jumpTimeoutDelta = JumpTimeout;

            if (Time.time - lastGroundedTime <= 1f)
            {
                animator.SetBool(IsGroundedAnimId, true);
            }
 
            if (verticalVelocity < 0 && fallTimeoutDelta <= 0)
            {
                animator.SetBool(IsFallingAnimId, true);
            }

            if (fallTimeoutDelta >= 0)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
        }
        
        // terminal velocity
        if (verticalVelocity >= terminalVelocity)
        {
            verticalVelocity = Mathf.Clamp(PlayerGravity * Time.deltaTime + verticalVelocity, terminalVelocity, float.PositiveInfinity);
        }
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + GroundSphereOffsetY, transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, 0.28f, GroundLayers);

        animator.SetBool(IsGroundedAnimId, isGrounded || Time.time - lastGroundedTime <= 0.15f);

        Vector3 leftFootSpherePosition = new Vector3(LeftFoot.position.x, LeftFoot.position.y + FeetSphereOffsetY, LeftFoot.position.z);
        Vector3 rightFootSpherePosition = new Vector3(RightFoot.position.x, RightFoot.position.y + FeetSphereOffsetY, RightFoot.position.z);
        feetIsGrounded = Physics.CheckSphere(leftFootSpherePosition, 0.28f, GroundLayers) || Physics.CheckSphere(rightFootSpherePosition, 0.28f, GroundLayers);

        animator.SetBool(FeetGroundedAnimId, feetIsGrounded || Time.time - lastGroundedTime <= 0.15f);
    }


    private void ToggleWalk()
    {
        isWalking = !isWalking;
        moveSpeedModifier = isWalking ? WalkSpeedModifier : 1;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (verticalVelocity < -10 && health != null)
        {
            health.TakeDamage(Mathf.RoundToInt(-verticalVelocity));
        }
    }

    // This is just to get a visual indication of whether the player is considered grounded in the editor
    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        Gizmos.color = isGrounded ? transparentGreen : transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y + GroundSphereOffsetY, transform.position.z),
            0.28f);

        Gizmos.color = feetIsGrounded ? transparentGreen : transparentRed;

        Gizmos.DrawSphere(
            new Vector3(LeftFoot.position.x, LeftFoot.transform.position.y + FeetSphereOffsetY, LeftFoot.transform.position.z),
            0.28f);
    }

    public void Save(GameData gameData)
    {
        gameData.PlayerData.Position = transform.position;
        gameData.PlayerData.Rotation = transform.rotation;
    }

    public void Load(GameData gameData)
    {
        characterController.enabled = false;
        transform.SetPositionAndRotation(gameData.PlayerData.Position, gameData.PlayerData.Rotation);
        characterController.enabled = true;
        targetRotation = gameData.PlayerData.Rotation;
        cameraRotationX = targetRotation.eulerAngles.x;
        cameraRotationY = targetRotation.eulerAngles.y;
    }
}
