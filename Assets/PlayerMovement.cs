using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2.5f;
    public float sprintSpeed = 5f;
    public float sneakSpeed = 1.2f;
    public float gravity = 9.81f;

    [Header("Comfort Settings")]
    public Volume postProcessVolume; 
    public float normalVignette = 0f;
    public float sprintVignette = 0.75f;
    public float vignetteSpeed = 5f;

    [Header("Required References")]
    public XROrigin xrOrigin;
    public AudioSource audioSource;
    public ParticleSystem sprintParticles;
    public AudioClip footstepSprint;

    [Header("Input Settings")]
    public XRNode movementHand = XRNode.LeftHand;
    public float inputThreshold = 0.1f;

    private CharacterController controller;
    private Camera vrCamera;
    private Vignette vignette;
    private bool isSprinting;
    private bool isSneaking;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (xrOrigin != null) vrCamera = xrOrigin.Camera;

        if (postProcessVolume != null && postProcessVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = normalVignette;
        }
    }

    void Update()
    {
        if (vrCamera == null) return;

        HandleInput();
        MovePlayer();
        HandleVignette();
        HandleFeedback();
    }

    void HandleInput()
    {
        float leftGrip = 0f;
        bool shiftPressed = false;
        bool ctrlPressed = false;

        // get left hand grip
        var leftDevice = InputDevices.GetDeviceAtXRNode(movementHand);
        if (leftDevice.isValid)
        {
            leftDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out leftGrip);
        }

        // keyboard input
        if (Keyboard.current != null)
        {
            shiftPressed = Keyboard.current.leftShiftKey.isPressed;
            ctrlPressed = Keyboard.current.leftCtrlKey.isPressed;
        }

        // final calculation (using left hand for both)
        isSprinting = (leftGrip > 0.8f) || shiftPressed; 
        isSneaking = ctrlPressed; 
    }

    void MovePlayer()
    {
        // get VR input
        InputDevices.GetDeviceAtXRNode(movementHand).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 joystickInput);
        
        float x = joystickInput.x;
        float z = joystickInput.y;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) z += 1f;
            if (Keyboard.current.sKey.isPressed) z -= 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;
        }

        // calculate direction relative to camera
        Vector3 forward = Vector3.ProjectOnPlane(vrCamera.transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(vrCamera.transform.right, Vector3.up).normalized;
        
        if (forward.sqrMagnitude < 0.01f) forward = Vector3.forward;

        Vector3 move = (forward * z + right * x);

        // speed
        // float currentSpeed = isSprinting ? sprintSpeed : (isSneaking ? sneakSpeed : walkSpeed);
        // speed — pull base speeds from AgilityManager if available
        float agilityWalk = AgilityManager.Instance != null ? AgilityManager.Instance.CurrentSpeed : walkSpeed;
        float agilitySprint = agilityWalk + (sprintSpeed - walkSpeed); // preserve the sprint offset
        float agilitySneakSpeed = sneakSpeed; // sneak stays fixed

        float currentSpeed = isSprinting ? agilitySprint : (isSneaking ? agilitySneakSpeed : agilityWalk);



        if (controller.isGrounded) 
        {
            verticalVelocity = -0.5f;
        }
        else 
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // final velocity
        Vector3 finalVelocity = (move * currentSpeed) + (Vector3.up * verticalVelocity);
        controller.Move(finalVelocity * Time.deltaTime);
    }

    void HandleVignette()
    {
        if (vignette == null) return;

        float target = isSprinting ? sprintVignette : normalVignette;
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, target, Time.deltaTime * vignetteSpeed);
    }

    void HandleFeedback()
    {
        // particles
        if (sprintParticles != null)
        {
            var emission = sprintParticles.emission;
            emission.enabled = isSprinting && controller.velocity.magnitude > 0.1f;
        }

        // sound
        if (audioSource != null && footstepSprint != null)
        {
            if (isSprinting && controller.velocity.magnitude > 0.1f)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = footstepSprint;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
            else
            {
                audioSource.Stop();
            }
        }
    }
}
