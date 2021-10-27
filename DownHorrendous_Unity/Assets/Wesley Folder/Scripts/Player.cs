using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.NonSerialized] public bool canInput;

    public PlayerCollisions PlayerCollisions
    {
        get => _playerCollisions;
        private set => _playerCollisions = value;
    }
    private PlayerCollisions _playerCollisions;

    public PlayerMovement PlayerMovement
    {
        get => _playerMovement;
        private set => _playerMovement = value;
    }
    private PlayerMovement _playerMovement;

    private Rigidbody rb;

    void Awake()
    {
        PlayerCollisions = gameObject.AddComponent<PlayerCollisions>();
        PlayerMovement = gameObject.AddComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        canInput = true;
        rb.centerOfMass = new Vector3(0f, PlayerPhysics.CenterOfMass, 0f);
    }
}
