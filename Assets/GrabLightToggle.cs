using UnityEngine;

using UnityEngine.InputSystem;

public class GrabLightToggle : MonoBehaviour
{
    public Light pointLight;
    public AudioSource audioSource;
    public Renderer meshRenderer;
    public Material materialOn;
    public Material materialOff;

    // Bind this to the X button (left controller primary button)
    public InputActionProperty xButton;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        pointLight.enabled = false;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.Stop();
    }

    void OnEnable()
    {
        xButton.action.Enable();
    }

    void OnDisable()
    {
        xButton.action.Disable();
    }

    void Update()
    {
        // Only works if the object is currently held
        if (grabInteractable.isSelected)
        {
            if (xButton.action.WasPressedThisFrame())
            {
                ToggleLight();
            }
        }
    }

    void ToggleLight()
    {
        pointLight.enabled = !pointLight.enabled;

        if (meshRenderer != null)
        {
            meshRenderer.material = pointLight.enabled ? materialOn : materialOff;
        }

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}