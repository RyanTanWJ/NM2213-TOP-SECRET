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
  List<MenuItem> MenuOptions;

  int selectedOption = 0;
  private const float countdown = 3.1f;
  
  [SerializeField]
  private Image countdownImage;
  [SerializeField]
  private List<Sprite> cdSprites;

  //Ensure that the player doesn't break the game by start and stopping during countdown
  private bool disableInput = false;

  private void Start()
  {
    MenuOptions[selectedOption].SwitchTextHighlight(true);
  }

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
      }else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
      {
        UpdatePauseMenu(true);
      }
      else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
      {
        UpdatePauseMenu(false);
      }
      else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
      {
        ProcessSelection(selectedOption);
      }
    }
	}

  private void ProcessSelection(int selectedOption)
  {
    switch (selectedOption)
    {
      case 0:
        StartCoroutine(CountdownToResume(countdown));
        break;
      default:
        Application.Quit();
        break;
    }
  }

  

  private void UpdatePauseMenu(bool up)
  {
    MenuOptions[selectedOption].SwitchTextHighlight(false);
    selectedOption = (up ? selectedOption + (MenuOptions.Count - 1) : selectedOption + 1) % MenuOptions.Count;
    MenuOptions[selectedOption].SwitchTextHighlight(true);
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
    pauseMenuUI.SetActive(false);
    disableInput = true;
    countdownImage.enabled = true;
    while (cd > 0)
    {
      countdownImage.sprite = cdSprites[Mathf.CeilToInt(cd-0.2f)];
      yield return new WaitForEndOfFrame();
      cd -= Time.unscaledDeltaTime;
    }
    countdownImage.enabled = false;
    pauseMenuUI.SetActive(true);
    Resume();
  }

  public bool IsPaused
  {
    get { return GameIsPaused; }
  }
}
