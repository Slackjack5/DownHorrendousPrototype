using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool gameStarted;
    public static bool gameFinished;
    [SerializeField] private TextMeshProUGUI interfaceText;
    private int currentTime = 3;
    private bool timerWait;
  private bool tickTime;

    public static bool JukeBox;
    public static bool Candles;
    public static bool Lights;
    public Animator myAnimator;
  private bool waitingForPlayer=true;
    public bool player1;
    public bool player2;
  public GameObject audioManager;
  public GameObject ourFont;
  public GameObject ourClipboard;
  private float fontSize;

    [SerializeField] private RawImage[] checkmarks;

  private void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
    {
        //If the game hasn't started, turn on the timer to start it
        if(!gameStarted && !timerWait && !waitingForPlayer) { StartCoroutine(Timer()); timerWait = true; tickTime = true;}

    //Starting timer
    interfaceText.text = currentTime.ToString();
        //If timer is 0 say "Go"
        if (currentTime==0) { interfaceText.text = "Go!"; }
        //Empty Text Field on Game Start
        if (gameStarted) { interfaceText.text = ""; }
        //Empty Text Field on Game Start
        if (gameFinished) { interfaceText.text = "Finished!";}
        if ((Input.GetKeyDown("space")) && waitingForPlayer) { waitingForPlayer = false; myAnimator.SetFloat("GameState", 1); AkSoundEngine.SetSwitch("GameStarted", "GameActive", audioManager); ourFont.SetActive(true); ourClipboard.SetActive(true); }
        if (Candles && JukeBox && player1 && player2 && Lights)
        {
            GameManager.gameFinished=true;
        }

        //turn on UI checkmarks
        if (JukeBox && !checkmarks[0].enabled)
        {
            checkmarks[0].enabled = true;
            AkSoundEngine.PostEvent("Play_Ding", gameObject);
            Bounce.startBouncing = true;
        }
        if (Candles && !checkmarks[1].enabled)
        {
            checkmarks[1].enabled = true;
            AkSoundEngine.PostEvent("Play_Ding", gameObject);
        }
        if (Lights && !checkmarks[2].enabled)
        {
            checkmarks[2].enabled = true;
            AkSoundEngine.PostEvent("Play_Ding", gameObject);
        }
    }



    public void SubTractTime()
    {
    if (!tickTime) { return; }
        if (currentTime>0)
        {
            currentTime -= 1;
            AkSoundEngine.PostEvent("Play_Ding", gameObject);
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
    //SubTractTime(); 
    
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

  public void startTimer()
  {

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
