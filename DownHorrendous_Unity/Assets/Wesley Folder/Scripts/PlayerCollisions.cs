using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EZCameraShake;

public class PlayerCollisions : MonoBehaviour
{
    private bool isMoody;
    private static int playersTouchingBedCount = 0;

    private static bool jukeboxIsTouched;
    private static bool jukeboxParticleExists;

    [NonSerialized] public bool fireParticleExists;
    public GameObject FireParticles { get; private set; }

    private ScreenShake cameraShake;

    private IEnumerator slipCoroutine;
    private IEnumerator fireCoroutine;
    private static IEnumerator screenShakeCoroutine;

    private Lover player;
    public bool player1;
    public bool player2;

    void Start()
    {
        player = GetComponent<Lover>();
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
            if (!player.isOnFire)
            {
                
                foreach (Renderer renderer in player.playerRenderers)
                {
                    renderer.material.color = Color.red;
                }
                player.isOnFire = true;
                AkSoundEngine.PostEvent("Play_SetOnFire", gameObject);
            }
            if (!fireParticleExists)
            {
                FireParticles = Instantiate(ParticleManager.FireParticles, transform);
                fireParticleExists = true;
            }
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

        if (collision.gameObject.CompareTag("Jukebox"))
        {
            jukeboxIsTouched = true;
            GameManager.JukeBox = true;
            //Debug.Log("JukeBoxTouched");

            GameObject jukebox = collision.gameObject;
            if (!jukeboxParticleExists)
            {
                Vector3 particlePosition = new Vector3(jukebox.transform.position.x, jukebox.transform.position.y + 2f, jukebox.transform.position.z);
                Instantiate(ParticleManager.JukeboxParticles, particlePosition, Quaternion.Euler(-90f, 0f, 0f));
                jukeboxParticleExists = true;
            }
        }
    }

  public void screenBump()
  {
    if (GameManager.gameStarted && GameManager.JukeBox) { CameraShaker.Instance.ShakeOnce(3, 0, .1f, .1f); }
   
  }

    private void OnCollisionExit(Collision collision)
    {
    /*
        if (collision.gameObject.CompareTag("Bed") && LightManager.RoomAmbience == LightManager.Ambience.Love && LightManager.AllCandlesLit && jukeboxIsTouched)
        {
            playersTouchingBedCount--;
        }
    */
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
