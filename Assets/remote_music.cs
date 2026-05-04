using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class remote_music : MonoBehaviour
{
    public AudioSource audioSource;
    public InputActionProperty buttonAction;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isGrabbed;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.Stop();

        if (buttonAction.action != null && buttonAction.action.enabled == false)
            buttonAction.action.Enable();
    }

    void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnSelectEntered);
            grabInteractable.selectExited.AddListener(OnSelectExited);
        }

        if (buttonAction.action != null)
            buttonAction.action.Enable();
    }

    void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }

        if (buttonAction.action != null)
            buttonAction.action.Disable();
    }

    void Update()
    {
        if (isGrabbed && buttonAction.action != null)
        {
            if (buttonAction.action.WasPressedThisFrame())
            {
                ToggleMusic();
            }
        }
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    void ToggleMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
        else
            audioSource.Play();
    }
}
