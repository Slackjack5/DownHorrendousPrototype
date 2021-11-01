using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCollisions : MonoBehaviour
{
    private bool isMoody;
    private static int playersTouchingBedCount = 0;

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
                    isMoody = true;
                }
                if (LightManager.RoomAmbience == LightManager.Ambience.Love)
                {
                    LightManager.SwitchLights(LightManager.Ambience.Normal);
                    isMoody = false;
                }
            }
        }
        /*
        if (other.gameObject.CompareTag("Bed") && LightManager.RoomAmbience == LightManager.Ambience.Love)
        {
          playersTouchingBedCount++;
          if (playersTouchingBedCount >= 2)
          {
            GameManager.gameFinished = true;
          }
        }
        */
    }

    private void OnCollisionEnter(Collision collision)
    {
    
        if (collision.gameObject.CompareTag("Bed") && LightManager.RoomAmbience == LightManager.Ambience.Love)
        {
            playersTouchingBedCount++;
            if (playersTouchingBedCount >= 2)
            {
                GameManager.gameFinished = true;
            }
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Bed") && LightManager.RoomAmbience == LightManager.Ambience.Love)
        {
            playersTouchingBedCount--;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<LightSwitch>())
        {
            other.gameObject.GetComponent<LightSwitch>().isOn = false;
            if (isMoody)
            {
                LightManager.RoomAmbience = LightManager.Ambience.Love;
            }
            else
            {
                LightManager.RoomAmbience = LightManager.Ambience.Normal;
            }
        }
        /*
        if (other.gameObject.CompareTag("Bed") && LightManager.RoomAmbience == LightManager.Ambience.Love)
        {
          playersTouchingBedCount--;
        }
        */
  }
}
