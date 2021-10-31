using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool gameStarted;
    public static bool gameFinished;
    [SerializeField] private TextMeshProUGUI interfaceText;
    private int currentTime = 3;
    private bool timerWait;

    // Update is called once per frame
    void Update()
    {
      //If the game hasn't started, turn on the timer to start it
      if(!gameStarted && !timerWait) { StartCoroutine(Timer()); timerWait = true; }

      //Starting timer
      interfaceText.text = currentTime.ToString();
      //If timer is 0 say "Go"
      if (currentTime==0) { interfaceText.text = "Go!"; }
      //Empty Text Field on Game Start
      if (gameStarted) { interfaceText.text = ""; }
      //Empty Text Field on Game Start
      if (gameFinished) { interfaceText.text = "Finished!"; }
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
