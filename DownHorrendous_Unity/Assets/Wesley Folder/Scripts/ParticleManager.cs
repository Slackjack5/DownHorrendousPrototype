using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static GameObject FireParticles;
    [SerializeField] private GameObject fireParticles;
    public static GameObject JukeboxParticles;
    [SerializeField] private GameObject jukeboxParticles;
    public static GameObject WalkParticles;
    [SerializeField] private GameObject walkParticles;

    void Start()
    {
        FireParticles = fireParticles;
        JukeboxParticles = jukeboxParticles;
        WalkParticles = walkParticles;
    }
}
