using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool gameStarted;
    [SerializeField] private bool gameFinished;
    private int currentTime = 3;
    private bool timerWait;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameStarted && !timerWait) { StartCoroutine(Timer()); timerWait = true; }
    }

    public void SubTractTime()
    {
        if(currentTime>0)
        {
            currentTime -= 1;
            timerWait = false;
            Debug.Log("-1 Tick");
        }
        else
        {
            gameStarted = true;
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        SubTractTime(); 
        
    }

    }
