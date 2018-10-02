using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour {

  public delegate void RestartGame();
  public static event RestartGame RestartGameEvent;
	
	// Update is called once per frame
	void Update () {
    if (Input.anyKeyDown)
    {
      RestartGameEvent();
    }
  }
}
