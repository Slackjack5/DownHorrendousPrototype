using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    public bool isLit = false;

    private CandleFlames flames;

    void Start()
    {
        flames = GetComponentInChildren<CandleFlames>();
        flames.gameObject.SetActive(false);
    }

    public void LightCandle()
    {
        flames.gameObject.SetActive(true);
    }
}
