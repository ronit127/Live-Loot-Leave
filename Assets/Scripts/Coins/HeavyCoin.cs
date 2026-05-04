using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class HeavyCoin : MonoBehaviour
{
    public int coinValue = 10;
    public Mesh coinMesh;

    void OnValidate() => ApplyMesh();

    public bool HasBeenGrabbed { get; private set; }

    void Awake()
    {
        ApplyMesh();

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = 5f;
            rb.isKinematic = true; // stays put until grabbed; XRI toggles this during grab/release
        }

        var grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(_ => HasBeenGrabbed = true);
    }

    void ApplyMesh()
    {
        if (coinMesh == null) return;
        var col = GetComponent<MeshCollider>();
        col.sharedMesh = coinMesh;
        col.convex = true;
        GetComponent<MeshFilter>().sharedMesh = coinMesh;
    }
}
