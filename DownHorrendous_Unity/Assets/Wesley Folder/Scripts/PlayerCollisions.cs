using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCollisions : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == CollisionManager.SlipperyLayer && player.canInput)
        {
            player.canInput = false;
            StartCoroutine(player.PlayerMovement.Slip());
        }
        if (other.gameObject.GetComponent<LightSwitch>())
        {
            other.gameObject.GetComponent<LightSwitch>().isOn = true;
            if (Array.TrueForAll(LightManager.lightSwitches, x => x.isOn))
            {
                if (LightManager.RoomAmbience == LightManager.Ambience.Normal)
                {
                    LightManager.SwitchLights(LightManager.Ambience.Love);
                }
                if (LightManager.RoomAmbience == LightManager.Ambience.Love)
                {
                    LightManager.SwitchLights(LightManager.Ambience.Normal);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<LightSwitch>())
        {
            other.gameObject.GetComponent<LightSwitch>().isOn = false;
        }
    }
}
