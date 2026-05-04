using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HoverPrompt : MonoBehaviour
{
    public Canvas promptCanvas;
    public TextMeshProUGUI priceText; // assign a TMP text object in the inspector

    private XRBaseInteractable _interactable;

    void Awake()
    {
        _interactable = GetComponent<XRBaseInteractable>();
        if (_interactable != null)
        {
            _interactable.firstHoverEntered.AddListener(_ => ShowPrompt());
            _interactable.lastHoverExited.AddListener(_ => { if (!_interactable.isSelected) HidePrompt(); });
            _interactable.selectExited.AddListener(_ => HidePrompt());
        }
    }

    void Start()
    {
        if (priceText != null)
        {
            var item = GetComponent<grababble>();
            if (item != null)
                priceText.text = $"{item.itemName} worth {item.itemValue}";
        }

        if (promptCanvas != null)
            promptCanvas.gameObject.SetActive(false);
    }

    public void ShowPrompt()
    {
        if (promptCanvas != null)
            promptCanvas.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        if (promptCanvas != null)
            promptCanvas.gameObject.SetActive(false);
    }
}
