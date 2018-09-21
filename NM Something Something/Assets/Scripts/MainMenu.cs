using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

  public delegate void MenuSelect(int option);
  public static event MenuSelect MenuSelectEvent;

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
      MenuSelectEvent(selectedOption);
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

}
