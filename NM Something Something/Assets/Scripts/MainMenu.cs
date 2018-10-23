using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

  public delegate void MenuSelect(int option);
  public static event MenuSelect MenuSelectEvent;
  
  [SerializeField]
  List<MenuItem> MenuOptions;
  
  int selectedOption;

  // Use this for initialization
  void Start () {
    selectedOption = 1;
    UpdateMainMenu(true);
	}
	
	// Update is called once per frame
	void Update ()
  {
    MenuControls();
  }

  private void MenuControls()
  {
    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
    {
      UpdateMainMenu(true);
    }
    else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
    {
      UpdateMainMenu(false);
    }
    else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
    {
      MenuSelectEvent(selectedOption);
    }
  }

  private void UpdateMainMenu(bool up)
  {
    MenuOptions[selectedOption].SwitchTextHighlight(false);
    if (up)
    {
      selectedOption = ((selectedOption - 1) + MenuOptions.Count) % MenuOptions.Count;
    }
    else
    {
      selectedOption = (selectedOption + 1) % MenuOptions.Count;
    }
    MenuOptions[selectedOption].SwitchTextHighlight(true);
  }

}
