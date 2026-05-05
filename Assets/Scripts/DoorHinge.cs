using UnityEngine;
using System.Collections;

public class DoorHinge : MonoBehaviour
{
    public GameObject door;
    public float openAngle = 90f;
    public float animationDuration = 1f;
    public AudioSource doorAudioSource;
    public string handTag = "PokePoint";

    private bool isAnimating = false;
    private bool isDoorOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = door.transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag) && !isAnimating)
            StartCoroutine(ToggleDoor());
    }

    private IEnumerator ToggleDoor()
    {
        isAnimating = true;

        if (doorAudioSource != null)
            doorAudioSource.Play();

        Quaternion startRotation = isDoorOpen ? openRotation : closedRotation;
        Quaternion endRotation = isDoorOpen ? closedRotation : openRotation;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            door.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        door.transform.localRotation = endRotation;
        isDoorOpen = !isDoorOpen;
        isAnimating = false;
    }
}