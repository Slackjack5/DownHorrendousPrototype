using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCollisions : MonoBehaviour
{
    private bool isMoody;
    private static int playersTouchingBedCount = 0;

    private static bool jukeboxIsTouched;

    private ScreenShake cameraShake;

    private IEnumerator slipCoroutine;
    private IEnumerator fireCoroutine;
    private static IEnumerator screenShakeCoroutine;

    private Player player;
    public bool player1;
    public bool player2;

    void Start()
    {
        player = GetComponent<Player>();
        slipCoroutine = player.PlayerMovement.Slip();
        fireCoroutine = player.PlayerMovement.OnFire();
        cameraShake = Camera.main.GetComponent<ScreenShake>();
        screenShakeCoroutine = cameraShake.Shake();
    }

    private void Update()
    {
        //Debug.Log(playersTouchingBedCount);
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    StopCoroutine(screenShakeCoroutine);
        //    screenShakeCoroutine = cameraShake.Shake();
        //    StartCoroutine(screenShakeCoroutine);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == CollisionManager.SlipperyLayer && player.canInput)
        {
            player.canInput = false;
            slipCoroutine = player.PlayerMovement.Slip();
            AkSoundEngine.PostEvent("Play_SlipSound", gameObject);
            StartCoroutine(slipCoroutine);
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
                    GameManager.Lights = true;
        }
                if (LightManager.RoomAmbience == LightManager.Ambience.Love)
                {
                    LightManager.SwitchLights(LightManager.Ambience.Normal);
                    isMoody = false;
                }
            }
        }
        if (other.gameObject.CompareTag("Fire"))
        {
            player.isOnFire = true;
            AkSoundEngine.PostEvent("Play_SetOnFire", gameObject);
            player.playerRenderer.material.color = Color.red;
        }

    if (other.gameObject.GetComponent<Candle>())
        {
            if (player.isOnFire)
            {
                Candle candle = other.gameObject.GetComponent<Candle>();
                candle.LightCandle();
                candle.isLit = true;
                if (Array.TrueForAll(LightManager.candles, x => x.isLit))
                {
                    LightManager.AllCandlesLit = true;
                    GameManager.Candles = true;
                }
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bed") /*&& LightManager.RoomAmbience == LightManager.Ambience.Love && LightManager.AllCandlesLit && jukeboxIsTouched*/)
        {
            //playersTouchingBedCount++;
            if (playersTouchingBedCount >= 2)
            {
                GameManager.gameFinished = true;
            }
        }

        //Sound
        if (collision.relativeVelocity.magnitude>5)
        {
            AkSoundEngine.PostEvent("Play_CollisionSound", gameObject);
            StopCoroutine(screenShakeCoroutine);
            screenShakeCoroutine = cameraShake.Shake();
            StartCoroutine(screenShakeCoroutine);
        }
        //if (collision.gameObject.CompareTag("Fire"))
        //{
        //    player.isOnFire = true;
        //    player.playerRenderer.material.color = Color.red;
        //}
        //if (collision.gameObject.GetComponentInChildren<Candle>())
        //{
        //    if (player.isOnFire)
        //    {
        //        Candle candle = collision.gameObject.GetComponentInChildren<Candle>();
        //        candle.LightCandle();
        //        candle.isLit = true;
        //        if(Array.TrueForAll(LightManager.candles, x => x.isLit))
        //        {
        //            LightManager.AllCandlesLit = true;
        //        }
        //    }
        //}
        if (collision.gameObject.CompareTag("Jukebox"))
        {
            jukeboxIsTouched = true;
            GameManager.JukeBox = true;
            Debug.Log("JukeBoxTouched");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
    /*
        if (collision.gameObject.CompareTag("Bed") && LightManager.RoomAmbience == LightManager.Ambience.Love && LightManager.AllCandlesLit && jukeboxIsTouched)
        {
            playersTouchingBedCount--;
        }
    */
        //if (collision.gameObject.CompareTag("Fire"))
        //{
        //    StopCoroutine(fireCoroutine);
        //    fireCoroutine = player.PlayerMovement.OnFire();
        //    StartCoroutine(fireCoroutine);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<LightSwitch>())
        {
            other.gameObject.GetComponent<LightSwitch>().isOn = false;
            if (Array.TrueForAll(LightManager.lightSwitches, x => !x.isOn))
            {
                if (isMoody)
                {
                    LightManager.RoomAmbience = LightManager.Ambience.Love;
                }
                else
                {
                LightManager.RoomAmbience = LightManager.Ambience.Normal;
                }
            }
        }
        if (other.gameObject.CompareTag("Fire"))
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = player.PlayerMovement.OnFire();
            StartCoroutine(fireCoroutine);
        }

    }
}
