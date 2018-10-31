using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

  public static bool GameIsPaused = false;

  [SerializeField]
  private GameObject pauseMenuUI;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
    {
      if (GameIsPaused)
      {
        Resume();
      }
      else
      {
        Pause();
      }
    }
	}

  private void Resume()
  {
    GameIsPaused = false;
    pauseMenuUI.SetActive(GameIsPaused);
    Time.timeScale = 1.0f;
  }

  private void Pause()
  {
    GameIsPaused = true;
    pauseMenuUI.SetActive(GameIsPaused);
    Time.timeScale = 0;
  }
}
