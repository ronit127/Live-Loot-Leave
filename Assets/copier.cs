using UnityEngine;
using UnityEngine.InputSystem;

public class copier : MonoBehaviour
{
    public Object[] vendingItems = new Object[3];
    public int[] itemWeights = new int[3] { 60, 30, 10 };
    public Transform spawnPoint;
    public InputActionProperty buttonAction;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabObject;
    public AudioSource audioSource;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        if (grabObject == null)
            grabObject = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        grabInteractable = grabObject;

        if (spawnPoint == null)
            spawnPoint = transform;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.Stop();
    }

    void Start()
    {
        if (buttonAction.action != null)
            buttonAction.action.Enable();
    }

    void Update()
    {
        if (grabInteractable == null || buttonAction.action == null)
            return;

        if (grabInteractable.isSelected && buttonAction.action.WasPressedThisFrame())
        {
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.time = 1.5f; 
                audioSource.PlayOneShot(audioSource.clip);
            }
            SpawnRandomItem();
        }
    }

    void SpawnRandomItem()
    {
        if (vendingItems == null || vendingItems.Length == 0)
            return;

        int totalWeight = 0;
        for (int i = 0; i < Mathf.Min(vendingItems.Length, itemWeights.Length); i++)
        {
            if (vendingItems[i] != null)
                totalWeight += itemWeights[i];
        }

        if (totalWeight == 0)
            return;

        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;
        int selectedIndex = -1;

        for (int i = 0; i < Mathf.Min(vendingItems.Length, itemWeights.Length); i++)
        {
            if (vendingItems[i] != null)
            {
                cumulativeWeight += itemWeights[i];
                if (randomValue < cumulativeWeight)
                {
                    selectedIndex = i;
                    break;
                }
            }
        }

        if (selectedIndex == -1)
            return;

        Object selectedItem = vendingItems[selectedIndex];

        if (selectedItem is GameObject gameObjectItem)
        {
            Instantiate(gameObjectItem, spawnPoint.position, spawnPoint.rotation);
        }
        else if (selectedItem is Component componentItem)
        {
            Instantiate(componentItem, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Instantiate(selectedItem);
        }
    }
}
