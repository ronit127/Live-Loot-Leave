using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CoinGoalZone : MonoBehaviour
{
    private readonly HashSet<HeavyCoin> _coinsInZone = new();

    void OnTriggerEnter(Collider other)
    {
        var coin = other.GetComponent<HeavyCoin>();
        if (coin == null || !coin.HasBeenGrabbed) return;

        _coinsInZone.Add(coin);
        coin.GetComponent<XRGrabInteractable>().selectExited.AddListener(OnCoinReleased);
    }

    void OnTriggerExit(Collider other)
    {
        var coin = other.GetComponent<HeavyCoin>();
        if (coin == null) return;

        _coinsInZone.Remove(coin);
        coin.GetComponent<XRGrabInteractable>().selectExited.RemoveListener(OnCoinReleased);
    }

    void OnCoinReleased(SelectExitEventArgs args)
    {
        var coin = args.interactableObject.transform.GetComponent<HeavyCoin>();
        if (coin == null || !_coinsInZone.Contains(coin)) return;

        _coinsInZone.Remove(coin);
        coin.GetComponent<XRGrabInteractable>().selectExited.RemoveListener(OnCoinReleased);
        ResourceManager.Instance.AddCoins(coin.coinValue);
        coin.gameObject.SetActive(false);
    }
}
