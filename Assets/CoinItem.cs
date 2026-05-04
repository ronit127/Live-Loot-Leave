using UnityEngine;
using Unity.XR.CoreUtils;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class CoinItem : MonoBehaviour
{
    public int coinValue = 1;

    void Awake()
    {
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        GetComponent<BoxCollider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<CharacterController>() == null &&
            other.GetComponent<XROrigin>() == null) return;

        ResourceManager.Instance.AddCoins(coinValue);
        gameObject.SetActive(false);
    }
}
