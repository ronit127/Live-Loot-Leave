using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public Transform muzzleTransform;
    public GameObject muzzleFlash;
    public GameObject bulletPrefab;
    public AudioSource gunAudioSource;

    [Header("Settings")]
    // public float muzzleFlashDuration = 0.5f;
    public float fireCooldown = 0.2f;
    public float bulletSpeed = 20f;

    private bool _triggerWasPressed;
    private float _cooldownTimer;

    public float flashDuration = 0.05f;

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

        // copier
        if (bulletPrefab != null && muzzleTransform != null)
        {
            // create copy
            GameObject newBullet = Instantiate(bulletPrefab, muzzleTransform.position, muzzleTransform.rotation);
            
            // move with rigidbody physics
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = muzzleTransform.right * bulletSpeed;
            }
        }

        if (muzzleFlash != null && muzzleTransform != null) {
            // StartCoroutine(ShowMuzzleFlash());
            StartCoroutine(TriggerFlash());
        }
    }

    /* IEnumerator ShowMuzzleFlash()
    {
        var flash = Instantiate(muzzleFlashPrefab, muzzleTransform.position, muzzleTransform.rotation, muzzleTransform);
        yield return new WaitForSeconds(muzzleFlashDuration);
        Destroy(flash);
    } */

    IEnumerator TriggerFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        muzzleFlash.SetActive(false);
    }
}
