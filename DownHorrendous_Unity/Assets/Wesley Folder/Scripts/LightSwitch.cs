using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public bool isOn;
    

    private void Update()
    {
        if (isOn)
        {
            gameObject.GetComponent<Animator>().SetBool("isPressed", true);
        }
        else {
            gameObject.GetComponent<Animator>().SetBool("isPressed", false);
        }
    }
}
