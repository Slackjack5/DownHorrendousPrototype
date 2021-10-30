using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public enum Ambience { Love, Normal };
    public static Ambience RoomAmbience;

    public static LightSwitch[] lightSwitches;

    public static GameObject LoveLightParent
    {
        get => _loveLightParent;
        private set => _loveLightParent = value;
    }
    private static GameObject _loveLightParent;
    [SerializeField] private GameObject loveLightParent;

    public static GameObject NormalLightParent
    {
        get => _normalLightParent;
        private set => _normalLightParent = value;
    }
    private static GameObject _normalLightParent;
    [SerializeField] private GameObject normalLightParent;

    void Start()
    {
        lightSwitches = FindObjectsOfType<LightSwitch>();
        LoveLightParent = loveLightParent;
        NormalLightParent = normalLightParent;
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
