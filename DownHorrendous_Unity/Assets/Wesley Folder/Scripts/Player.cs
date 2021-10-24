using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement PlayerMovement
    {
        get => _playerMovement;
        private set => _playerMovement = value;
    }
    private PlayerMovement _playerMovement;

    void Awake()
    {
        PlayerMovement = gameObject.AddComponent<PlayerMovement>();
    }
}
