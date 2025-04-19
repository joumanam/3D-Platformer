using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float jumpForce = 5f;
    public float rotationSpeed = 10f;
    public float footstepInterval = 0.5f;
    private float footstepTimer = 0f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Camera Settings")]
    public Camera playerCamera;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    [Header("Camera Zoom Settings")]
    public bool enableZoom = true;
    public float zoomFOV = 30f;

    [Header("Head Bobbing Settings")]
    public Transform cameraHolder;
    public float bobFrequency = 10f;
    public float bobAmplitude = 0.05f;

    private float bobTimer = 0f;
    private Vector3 originalCameraLocalPos;

    //private variables;
    private bool isGrounded;
    private Rigidbody rb;
    private bool isWalking;
    private bool isSprinting;
    private float leftRightMovement;
    private float upDownMovement;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private bool isZoomed = false;
    private float fov;
    private float moveSpeed;
    private ObjectPickup objectPickup;
    private Shooter shooter;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        objectPickup = GetComponent<ObjectPickup>();
        shooter = GetComponent<Shooter>();
        fov = playerCamera.fieldOfView;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        enableZoom = !isWalking && !objectPickup.heldObject && !shooter.isShooting;
        moveSpeed = isSprinting ? sprintSpeed : walkSpeed;
        shooter.enableShooting = !isZoomed && !objectPickup.heldObject;
        HandleHeadBobbing();
        if (isWalking && isGrounded)
        {

            float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
            float stepInterval = footstepInterval * (walkSpeed / currentSpeed);

            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                SoundManager.PlaySound(SoundType.FOOTSTEP, 0.3f);
                footstepTimer = stepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (!isZoomed)
        {
            Vector3 targetVelocity = new Vector3(leftRightMovement, 0, upDownMovement);
            targetVelocity = transform.TransformDirection(targetVelocity) * moveSpeed;
            Vector3 velocity = rb.linearVelocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -10, 10);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -10, 10);
            velocityChange.y = 0;
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }

    }

    public void Move(InputAction.CallbackContext context)
    {
        leftRightMovement = context.ReadValue<Vector2>().x;
        upDownMovement = context.ReadValue<Vector2>().y;
        isWalking = !isZoomed && (leftRightMovement != 0 || upDownMovement != 0);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    public void Sprint(InputAction.CallbackContext context)
    {
        isSprinting = context.performed && isGrounded && isWalking;
    }

    public void Look(InputAction.CallbackContext context)
    {
        if (cameraCanMove)
        {
            yaw = transform.localEulerAngles.y + context.ReadValue<Vector2>().x * mouseSensitivity;
            if (!invertCamera)
            {
                pitch -= mouseSensitivity * context.ReadValue<Vector2>().y;
            }
            else
            {
                // Inverted Y
                pitch += mouseSensitivity * context.ReadValue<Vector2>().y;
            }

            // Clamp pitch between lookAngle
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                playerCamera.transform.localPosition.y,
                0
            );
        }
    }

    public void Zoom(InputAction.CallbackContext context)
    {
        if (enableZoom)
        {
            isZoomed = context.performed || context.started;
            playerCamera.fieldOfView = isZoomed ? zoomFOV : fov;
        }
    }

    private void HandleHeadBobbing()
    {
        if (isWalking && isGrounded)
        {
            bobTimer += Time.deltaTime * bobFrequency;
            float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;

            Vector3 newPos = originalCameraLocalPos;
            newPos.y += bobOffset;

            cameraHolder.localPosition = newPos;
        }
        else
        {
            bobTimer = 0f;
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, originalCameraLocalPos, Time.deltaTime * 5f);
        }
    }

}
