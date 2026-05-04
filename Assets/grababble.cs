using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class grababble : MonoBehaviour
{
    [Header("Item Properties")]
    public string itemName = "Item";
    public Mesh itemMesh;
    public int itemValue = 1;

    private bool _isHeld;
    private bool _xWasPressed;

    void ApplyMesh()
    {
        if (itemMesh == null) return;
        var col = GetComponent<MeshCollider>();
        col.sharedMesh = itemMesh;
        col.convex = true;
        GetComponent<MeshFilter>().sharedMesh = itemMesh;
    }

    void OnValidate() => ApplyMesh();

    void Awake()
    {
        ApplyMesh();
        var grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(_ => _isHeld = true);
        grab.selectExited.AddListener(_ => _isHeld = false);
    }

    void Update()
    {
        if (!_isHeld) return;

        var leftHands = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHands);

        bool xPressed = false;
        foreach (var device in leftHands)
            device.TryGetFeatureValue(CommonUsages.primaryButton, out xPressed);

        if (xPressed && !_xWasPressed)
            Collect();

        _xWasPressed = xPressed;
    }

    void Collect()
    {
        InventoryManager.Instance.AddItem(new InventoryItem
        {
            name  = itemName,
            mesh  = itemMesh,
            value = itemValue
        });
        gameObject.SetActive(false);
    }
}
