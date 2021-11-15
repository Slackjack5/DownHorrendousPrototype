using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool gameStarted;
    public static bool gameFinished;
    [SerializeField] private TextMeshProUGUI interfaceText;
    private int currentTime = 3;
    private bool timerWait;

    public static bool JukeBox;
    public static bool Candles;
    public static bool Lights;
    public bool player1;
    public bool player2;

    [SerializeField] private RawImage[] checkmarks;

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


        if (Candles && JukeBox && player1 && player2 && Lights)
        {
            GameManager.gameFinished=true;
        }

        //turn on UI checkmarks
        if (JukeBox && !checkmarks[0].enabled)
        {
            checkmarks[0].enabled = true;
        }
        if (Candles && !checkmarks[1].enabled)
        {
            checkmarks[1].enabled = true;
        }
        if (Lights && !checkmarks[2].enabled)
        {
            checkmarks[2].enabled = true;
        }
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

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Player1") )
    {
      player1 = true;
    }
    if (other.gameObject.CompareTag("Player2"))
    {
      player2 = true;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.gameObject.CompareTag("Player1"))
    {
      player1 = false;
    }
    if (other.gameObject.CompareTag("Player2"))
    {
      player2 = false;
    }
  }

}
