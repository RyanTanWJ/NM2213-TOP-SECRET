using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

  void OnEnable()
  {
    MainMenu.MenuSelectEvent += OnMenuSelect;
    GoBack.GoBackEvent += GoBackToMainMenu;
    Player.GameOverEvent += OnGameOver;
    GameOverMenu.RestartGameEvent += RestartGame;
  }

  void OnDisable()
  {
    MainMenu.MenuSelectEvent -= OnMenuSelect;
    GoBack.GoBackEvent -= GoBackToMainMenu;
    Player.GameOverEvent -= OnGameOver;
    GameOverMenu.RestartGameEvent -= RestartGame;
  }

  // Use this for initialization
  void Start () {
    
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  private void OnMenuSelect(int option)
  {
    switch (option)
    {
      case 0:
        StartGame();
        break;
      case 1:
        Instructions();
        break;
      case 2:
        Credits();
        break;
      case 3:
        Quit();
        break;
      default:
        Debug.LogError("Selected Menu Option, " + option + ", is not a valid option.");
        break;
    }
  }

  private void StartGame()
  {
    //TODO: Go to Game Scene
    SceneManager.LoadScene(1);
  }

  private void Instructions()
  {
    //TODO: Go to Instructions scene
    SceneManager.LoadScene(2);
  }

  private void Credits()
  {
    //TODO: Go to Credits Scene
    SceneManager.LoadScene(3);
  }

  private void GoBackToMainMenu()
  {
    SceneManager.LoadScene(0);
  }

  private void Quit()
  {
    Application.Quit();
  }

  private void OnGameOver()
  {
    //TODO: Game Over Stuff (Animation and whatnot)
    SceneManager.LoadScene(4);
  }

  private void RestartGame()
  {
    SceneManager.LoadScene(0);
  }
}
