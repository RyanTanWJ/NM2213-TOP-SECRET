using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

  public static bool GameIsPaused = false;

  [SerializeField]
  private GameObject pauseMenuUI;

  [SerializeField]
  private TMPro.TextMeshProUGUI displayText;

  [SerializeField]
  private Image background;

  private const float countdown = 3.1f;

  //Ensure that the player doesn't break the game by start and stopping during countdown
  private bool disableInput = false;
	
	// Update is called once per frame
	void Update () {
    if (!disableInput)
    {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
        if (GameIsPaused)
        {
          StartCoroutine(CountdownToResume(countdown));
        }
        else
        {
          Pause();
        }
      }
    }
	}

  private void Resume()
  {
    disableInput = false;
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

  private IEnumerator CountdownToResume(float cd)
  {
    while (cd > 0)
    {
      displayText.text = Mathf.CeilToInt(cd-0.2f).ToString();
      yield return new WaitForEndOfFrame();
      cd -= Time.unscaledDeltaTime;
    }
    Resume();
  }
}
