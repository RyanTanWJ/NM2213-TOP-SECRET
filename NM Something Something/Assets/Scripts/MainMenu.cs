using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

  [SerializeField]
  GameObject SplashText;
  [SerializeField]
  List<GameObject> MenuOptions;
  
  int selectedOption;

  bool splash = true;

  // Use this for initialization
  void Start () {
    selectedOption = 1;
    UpdateMainMenu(true);
	}
	
	// Update is called once per frame
	void Update ()
  {
    if (splash)
    {
      if (Input.anyKeyDown)
      {
        OpenMainMenu();
        splash = false;
      }
    }
    else
    {
      MenuControls();
    }
  }

  private void MenuControls()
  {
    if (Input.GetKeyDown(KeyCode.UpArrow))
    {
      UpdateMainMenu(true);
    }
    else if (Input.GetKeyDown(KeyCode.DownArrow))
    {
      UpdateMainMenu(false);
    }
    else if (Input.GetKeyDown(KeyCode.Return))
    {
      MenuSelect();
    }
  }

  private void OpenMainMenu()
  {
    SplashText.SetActive(false);
    foreach (GameObject option in MenuOptions)
    {
      option.SetActive(true);
    }
  }

  private void MenuSelect()
  {
    switch (selectedOption)
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
        Debug.LogError("Selected Menu Option: " + selectedOption + ", is not a valid option.");
        break;
    }
  }

  private void UpdateMainMenu(bool up)
  {
    MenuOptions[selectedOption].GetComponent<TMPro.TextMeshProUGUI>().color = Color.black;
    if (up)
    {
      selectedOption = ((selectedOption - 1) + MenuOptions.Count) % MenuOptions.Count;
    }
    else
    {
      selectedOption = (selectedOption + 1) % MenuOptions.Count;
    }
    MenuOptions[selectedOption].GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
  }

  private void StartGame()
  {
    Debug.Log("StartGame() not yet implemented");
    //TODO: Go to Game Scene
  }

  private void Instructions()
  {
    Debug.Log("Instructions() not yet implemented");
    //TODO: Go to Instructions scene
  }

  private void Credits()
  {
    Debug.Log("Credits() not yet implemented");
    //TODO: Go to Credits Scene
  }

  private void Quit()
  {
    Application.Quit();
  }
}
