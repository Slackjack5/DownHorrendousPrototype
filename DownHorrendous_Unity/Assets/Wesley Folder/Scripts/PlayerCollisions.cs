using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == CollisionManager.SlipperyMask && player.canInput)
        {
            player.canInput = false;
            StartCoroutine(player.PlayerMovement.Slip());
        }
    }
}
