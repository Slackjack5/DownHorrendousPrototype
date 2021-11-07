using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    public bool isLit = false;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void LightCandle()
    {
        gameObject.SetActive(true);
    }
}
