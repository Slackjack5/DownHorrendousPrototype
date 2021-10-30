using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private enum InputNames { Right, Left, Forward };

    private List<Player> players = new List<Player>();

    public static bool GizmosAreDisplayed
    {
        get => _gizmosAreDisplayed;
        private set => _gizmosAreDisplayed = value;
    }
    private static bool _gizmosAreDisplayed;

    private Dictionary<InputNames, InputClass> inputs = new Dictionary<InputNames, InputClass>();

    [Header("Display Gizmos?")]
    [SerializeField] private bool gizmosAreDisplayed;

    [Header("Movement Tuning")]
    [SerializeField] [Range(0f, 360f)] private float rotationSpeed;
    [SerializeField] [Range(0.01f, 10f)] private float moveSpeed;
    public static float SlopeOffset
    {
        get => _slopeOffset;
        private set => _slopeOffset = value;
    }
    private static float _slopeOffset;
    [SerializeField] [Range(0f, 89.9f)] private float slopeOffset;
    public static float StepOffset
    {
        get => _stepOffset;
        private set => _stepOffset = value;
    }
    private static float _stepOffset;
    [SerializeField] [Range(0.01f, 1f)] private float stepOffset;

    [Header("Controls")]
    [SerializeField] private KeyCode inputRotateRight;
    [SerializeField] private KeyCode inputRotateLeft;
    [SerializeField] private KeyCode inputMoveForward;

    void Awake()
    {
        GizmosAreDisplayed = gizmosAreDisplayed;
        SlopeOffset = Mathf.Deg2Rad * slopeOffset * 0.1f;
        StepOffset = stepOffset;
    }

    void Start()
    {
        Player[] playersArray = FindObjectsOfType<Player>();
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
        #region check inputs
        if (Input.GetKey(inputRotateRight))
        {
            inputs[InputNames.Right].keyState = InputClass.KeyState.Held;
        }else if (Input.GetKey(inputRotateLeft))
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
        #endregion
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
                    players[i].PlayerMovement.Rotate(PlayerMovement.Direction.Right, rotationSpeed);
                }
            }
        }
        if (inputs[InputNames.Left].keyState == InputClass.KeyState.Held)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].canInput)
                {
                    players[i].PlayerMovement.Rotate(PlayerMovement.Direction.Left, rotationSpeed);
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
        #endregion
    }
}
