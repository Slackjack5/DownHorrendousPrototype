using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public enum Ambience { Love, Normal };
    public static Ambience RoomAmbience;

    public static LightSwitch[] lightSwitches;
    public static Candle[] candles;
    public static bool AllCandlesLit;

    public static GameObject LoveLightParent { get; private set; }
    [SerializeField] private GameObject loveLightParent;

    public static GameObject NormalLightParent { get; private set; }
    [SerializeField] private GameObject normalLightParent;

    void Start()
    {
        lightSwitches = FindObjectsOfType<LightSwitch>();
        candles = FindObjectsOfType<Candle>();
        LoveLightParent = loveLightParent;
        NormalLightParent = normalLightParent;
        RoomAmbience = Ambience.Normal;
    }

    public static void SwitchLights(Ambience newAmbience)
    {
        if (newAmbience == Ambience.Normal)
        {
            Light[] loveLights = LoveLightParent.GetComponentsInChildren<Light>();
            foreach (Light light in loveLights)
            {
                light.enabled = false;
            }
            Light[] normalLights = NormalLightParent.GetComponentsInChildren<Light>();
            foreach (Light light in normalLights)
            {
                light.enabled = true;
            }
        }
        if (newAmbience == Ambience.Love)
        {
            Light[] normalLights = NormalLightParent.GetComponentsInChildren<Light>();
            foreach (Light light in normalLights)
            {
                light.enabled = false;
            }
            Light[] loveLights = LoveLightParent.GetComponentsInChildren<Light>();
            foreach (Light light in loveLights)
            {
                light.enabled = true;
            }
        }
    }
}
