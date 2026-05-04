using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class CoinItem : MonoBehaviour
{
    public int coinValue = 1;
    public Mesh coinMesh;

    private bool _isHeld;
    private bool _xWasPressed;

    void OnValidate() => ApplyMesh();

    void Awake()
    {
        ApplyMesh();
        var grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(_ => _isHeld = true);
        grab.selectExited.AddListener(_ => _isHeld = false);
    }

    void ApplyMesh()
    {
        if (coinMesh == null) return;
        var col = GetComponent<MeshCollider>();
        col.sharedMesh = coinMesh;
        col.convex = true;
        GetComponent<MeshFilter>().sharedMesh = coinMesh;
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
        ResourceManager.Instance.AddCoins(coinValue);
        gameObject.SetActive(false);
    }
}
