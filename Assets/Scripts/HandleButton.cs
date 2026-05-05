using UnityEngine;
using System.Collections;

public class HandleButton : MonoBehaviour
{
    public GameObject door;
    public float openAngle = 90f;
    public float animationDuration = 1f;
    public AudioSource doorAudioSource;

    private bool isAnimating = false;
    private bool isDoorOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = door.transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    public void Toggle()
    {
        if (!isAnimating)
            StartCoroutine(MoveDoor());
    }

    private IEnumerator MoveDoor()
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