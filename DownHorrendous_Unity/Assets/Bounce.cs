using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{

  public static bool startBouncing;
  private bool shrinking=false;
  private float size;
    // Start is called before the first frame update
    void Start()
    {
    //size = 1;
    }

    // Update is called once per frame
    void Update()
    {
      if(size>=0)
      {
      size -= 0.001f;
      }

      if(startBouncing)
      {
      gameObject.transform.localScale = new Vector3(size, size, size);
      }
      
    }

  public void inscreaseSize()
  {
    size = .75f;
  }
}
