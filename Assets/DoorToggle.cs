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

    private bool isAnimating = false;

    public void Toggle()
    {
        if (door != null && !isAnimating)
            StartCoroutine(SinkDoor());
        
    }

    private IEnumerator SinkDoor()
    {
        isAnimating = true;

        Vector3 startPosition = door.transform.position;
        Vector3 endPosition = startPosition - new Vector3(0, door.transform.localScale.y, 0);

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;

            door.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        door.transform.position = endPosition;
        door.SetActive(false); 
        isAnimating = false;
    }
}

