using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HoverPrompt : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI priceText;

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
        var item = GetComponent<grababble>();
        if (item != null && priceText != null)
            priceText.text = $"{item.itemName} worth {item.itemValue}";

        HidePrompt();
    }

    public void ShowPrompt()
    {
        if (promptText != null) promptText.gameObject.SetActive(true);
        if (priceText != null)  priceText.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        if (promptText != null) promptText.gameObject.SetActive(false);
        if (priceText != null)  priceText.gameObject.SetActive(false);
    }
}
