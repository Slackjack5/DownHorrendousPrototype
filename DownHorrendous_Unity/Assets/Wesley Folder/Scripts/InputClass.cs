using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputClass
{
    public enum KeyState { Pressed, Held, Released, Untouched };
    public KeyState keyState;

    public KeyCode keyCode;
}