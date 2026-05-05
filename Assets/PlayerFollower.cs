using Unity.XR.CoreUtils;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public float followSpeed = 0.8f;
    public float stopDistance = 0.5f;
    public float detectionRange = 6f;
    public int damage = 10;

    private Transform _player;
    private MeshRenderer _renderer;

    void Start()
    {
        var origin = FindFirstObjectByType<XROrigin>();
        if (origin != null) _player = origin.transform;

        _renderer = GetComponent<MeshRenderer>();
        SetColor(Color.red);
    }

    void Update()
    {
        if (_player == null) return;

        float dist = Vector3.Distance(transform.position, _player.position);
        if (dist <= detectionRange && dist > stopDistance)
            transform.position = Vector3.MoveTowards(transform.position, _player.position, followSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() == null &&
            other.GetComponent<XROrigin>() == null) return;

        SetColor(Color.green);
        PlayerHealth.Instance?.TakeDamage(damage);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>() == null &&
            other.GetComponent<XROrigin>() == null) return;

        SetColor(Color.red);
    }

    void SetColor(Color color)
    {
        if (_renderer != null)
            _renderer.material.color = color;
    }
}
