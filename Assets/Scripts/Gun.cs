using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public Transform muzzleTransform;
    public GameObject muzzleFlashPrefab;
    public AudioSource gunAudioSource;

    [Header("Settings")]
    public float muzzleFlashDuration = 0.05f;
    public float fireCooldown = 0.2f;

    private bool _triggerWasPressed;
    private float _cooldownTimer;

    void Awake()
    {
    }

    void Update()
    {
        _cooldownTimer -= Time.deltaTime;

        var rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        rightHand.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        bool triggerPressed = triggerValue > 0.8f;

        if (triggerPressed && !_triggerWasPressed && _cooldownTimer <= 0f)
            Fire();

        _triggerWasPressed = triggerPressed;
    }

    void Fire()
    {
        _cooldownTimer = fireCooldown;

        if (gunAudioSource != null)
            gunAudioSource.Play();

        if (muzzleFlashPrefab != null && muzzleTransform != null)
            StartCoroutine(ShowMuzzleFlash());
    }

    IEnumerator ShowMuzzleFlash()
    {
        var flash = Instantiate(muzzleFlashPrefab, muzzleTransform.position, muzzleTransform.rotation, muzzleTransform);
        yield return new WaitForSeconds(muzzleFlashDuration);
        Destroy(flash);
    }
}
