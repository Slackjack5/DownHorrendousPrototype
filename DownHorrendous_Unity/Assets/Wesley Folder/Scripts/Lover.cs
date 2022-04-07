using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lover : MonoBehaviour
{
    [System.NonSerialized] public bool canInput;
    [System.NonSerialized] public bool isOnFire;

    public LoverValuesScriptableObject LoverValues
    {
        get => _loverValues;
        private set => _loverValues = value;
    }
    [SerializeField] private LoverValuesScriptableObject _loverValues;

    public PlayerCollisions PlayerCollisions { get; private set; }

    public PlayerMovement PlayerMovement { get; private set; }

    private Rigidbody rb;
    //[System.NonSerialized] public Renderer playerRenderer;
    [System.NonSerialized] public Renderer[] playerRenderers = new Renderer[3];
    [System.NonSerialized] public Color baseColor;

    void Awake()
    {
        PlayerCollisions = gameObject.AddComponent<PlayerCollisions>();
        PlayerMovement = gameObject.AddComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        playerRenderers[0] = GetComponent<Renderer>();
        GameObject leftCheek = gameObject.transform.Find("Eye (2)").gameObject;
        GameObject rightCheek = gameObject.transform.Find("Eye (3)").gameObject;
        playerRenderers[1] = leftCheek.GetComponent<Renderer>();
        playerRenderers[2] = rightCheek.GetComponent<Renderer>();
        baseColor = playerRenderers[0].material.color;

        Services.lovers.Add(this);
    }

    void Start()
    {
        canInput = true;
        rb.centerOfMass = new Vector3(0f, PlayerPhysics.CenterOfMass, 0f);
    }
}
