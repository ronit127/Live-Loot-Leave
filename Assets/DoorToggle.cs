using UnityEngine;

public class DoorToggle : MonoBehaviour
{
    public GameObject door;

    public void Toggle()
    {
        if (door != null)
            door.SetActive(!door.activeSelf);
    }
}
