using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    private enum InputNames { Right, Left, Forward };
    //Declare GameManager
    private GameObject gameManager;

    private List<Lover> players = new List<Lover>();

    public static bool GizmosAreDisplayed { get; private set; }

    private Dictionary<InputNames, InputClass> inputs = new Dictionary<InputNames, InputClass>();

    [Header("Display Gizmos?")]
    [SerializeField] private bool gizmosAreDisplayed;

    public static float RotationSpeed { get; private set; }
    [Header("Movement Tuning")]
    [SerializeField] [Range(0f, 360f)] private float rotationSpeed;
    [SerializeField] [Range(0.01f, 10f)] private float moveSpeed;
    public static float SlopeOffset { get; private set; }
    [SerializeField] [Range(0f, 89.9f)] private float slopeOffset;
    public static float StepOffset { get; private set; }
    [SerializeField] [Range(0.01f, 1f)] private float stepOffset;

    [Header("Controls")]
    [SerializeField] private KeyCode inputRotateRight;
    [SerializeField] private KeyCode inputRotateLeft;
    [SerializeField] private KeyCode inputMoveForward;

    public static float FireDuration { get; private set; }
    [Header("Durations (in seconds)")]
    [SerializeField] private float fireDuration;

    public static float TimeUntilEyesMeet { get; private set; } //how long with no input until the lovers turn to face each other
    [SerializeField] private float timeUntilEyesMeet;

    private IEnumerator noInputCoroutine;
    private bool isInputting;

    void Awake()
    {
        #region assigning static variables
        GizmosAreDisplayed = gizmosAreDisplayed;
        RotationSpeed = rotationSpeed;
        SlopeOffset = Mathf.Deg2Rad * slopeOffset * 0.1f;
        StepOffset = stepOffset;
        FireDuration = fireDuration;
        TimeUntilEyesMeet = timeUntilEyesMeet;
        #endregion

        noInputCoroutine = WaitForNoInputs();

        //Assign GameManager
        gameManager = GameObject.Find("GameManager");
    }

    void Start()
    {
        //Debug.Log(SlopeOffset);
        Lover[] playersArray = FindObjectsOfType<Lover>();
        for (int i = 0; i < playersArray.Length; i++)
        {
            players.Add(playersArray[i]);
        }

        #region assigning inputs
        inputs.Add(InputNames.Right, new InputClass());
        inputs.Add(InputNames.Left, new InputClass());
        inputs.Add(InputNames.Forward, new InputClass());
        inputs[InputNames.Right].keyCode = inputRotateRight;
        inputs[InputNames.Left].keyCode = inputRotateLeft;
        inputs[InputNames.Forward].keyCode = inputMoveForward;
        #endregion
    }

    void Update()
    {
        //If the game started, turn on inputs
        if (GameManager.gameStarted == true && GameManager.gameFinished == false)
        {
            #region check inputs
            if (Input.GetKey(inputRotateRight))
            {
                inputs[InputNames.Right].keyState = InputClass.KeyState.Held;
            }
            else if (Input.GetKey(inputRotateLeft))
            {
                inputs[InputNames.Left].keyState = InputClass.KeyState.Held;
            }
            if (Input.GetKey(inputMoveForward))
            {
                inputs[InputNames.Forward].keyState = InputClass.KeyState.Held;
            }
            if (Input.GetKeyUp(inputRotateRight))
            {
                inputs[InputNames.Right].keyState = InputClass.KeyState.Untouched;
            }
            if (Input.GetKeyUp(inputRotateLeft))
            {
                inputs[InputNames.Left].keyState = InputClass.KeyState.Untouched;
            }
            if (Input.GetKeyUp(inputMoveForward))
            {
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].PlayerMovement.UseGravity();
                }
                inputs[InputNames.Forward].keyState = InputClass.KeyState.Untouched;
            }
            if (!Input.anyKey) //turn the lovers to look at each other when nothing is pressed for TimeUntilEyesMeet
            {
                if (isInputting)
                {
                    noInputCoroutine = WaitForNoInputs();
                    StartCoroutine(noInputCoroutine);
                }
            }
            else
            {
                if (!isInputting)
                {
                    StopCoroutine(noInputCoroutine);
                    for (int i = 0; i < players.Count; i++)
                    {
                        StopCoroutine(players[i].PlayerMovement.meetEyesCoroutine);
                    }
                    isInputting = true;
                }
            }
            #endregion
        }
    }

    private void FixedUpdate()
    {
        #region move players based on inputs
        if (inputs[InputNames.Right].keyState == InputClass.KeyState.Held)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].canInput)
                {
                    players[i].PlayerMovement.Rotate(PlayerMovement.Direction.Right, RotationSpeed);
                }
            }
        }
        if (inputs[InputNames.Left].keyState == InputClass.KeyState.Held)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].canInput)
                {
                    players[i].PlayerMovement.Rotate(PlayerMovement.Direction.Left, RotationSpeed);
                }
            }
        }
        if (inputs[InputNames.Forward].keyState == InputClass.KeyState.Held)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].canInput)
                {
                    players[i].PlayerMovement.MoveForward(moveSpeed);
                }
            }
        }
        if (inputs[InputNames.Forward].keyState == InputClass.KeyState.Untouched)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].PlayerMovement.walkParticleExists)
                {
                    Destroy(players[i].PlayerMovement.WalkParticles);
                    players[i].PlayerMovement.walkParticleExists = false;
                }
            }
        }
        #endregion
    }

    private IEnumerator WaitForNoInputs()
    {
        isInputting = false;
        yield return new WaitForSeconds(TimeUntilEyesMeet);
        for (int i = 0; i < players.Count; i++)
        {
            players[i].PlayerMovement.meetEyesCoroutine = players[i].PlayerMovement.MeetEyes();
            StartCoroutine(players[i].PlayerMovement.meetEyesCoroutine);
        }
        yield break;
    }
}
