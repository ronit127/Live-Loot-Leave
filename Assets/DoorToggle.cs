// previous version for instant disappear :
// using UnityEngine;

// public class DoorToggle : MonoBehaviour
// {
//     public GameObject door;

//     public void Toggle()
//     {
//         if (door != null)
//             door.SetActive(!door.activeSelf);
//     }
// }


using UnityEngine;
using System.Collections;

public class DoorToggle : MonoBehaviour
{
    public GameObject door;
    public float animationDuration = 2f;
    public AudioSource doorAudioSource;

    private bool isAnimating = false;
    private bool isDoorDown = false;

    private Vector3 upPosition;
    private Vector3 downPosition;

    void Start()
    {
        upPosition = door.transform.position;
        downPosition = upPosition - new Vector3(0, door.transform.localScale.y, 0);
    }

    public void Toggle()
    {
        if (door != null && !isAnimating)
            StartCoroutine(MoveDoor());
    }

    private IEnumerator MoveDoor()
    {
        isAnimating = true;

        if (doorAudioSource != null)
            doorAudioSource.Play();

        door.SetActive(true);
        Vector3 startPosition = isDoorDown ? downPosition : upPosition;
        Vector3 endPosition = isDoorDown ? upPosition : downPosition;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            door.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        door.transform.position = endPosition;
        isDoorDown = !isDoorDown;

        if (isDoorDown)
            door.SetActive(false);

        isAnimating = false;
    }
}