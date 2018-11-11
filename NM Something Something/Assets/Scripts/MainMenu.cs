using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

  public delegate void MenuSelect(int option);
  public static event MenuSelect MenuSelectEvent;
  
  [SerializeField]
  private List<MenuItem> MenuOptions;

  int selectedOption;

  private void OnEnable()
  {
    MenuItem.MenuItemButtonClickedEvent += MenuItemButtonClicked;
  }

  private void OnDisable()
  {
    MenuItem.MenuItemButtonClickedEvent -= MenuItemButtonClicked;
  }

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
    selectedOption = (up ? selectedOption + (MenuOptions.Count - 1) : selectedOption + 1) % MenuOptions.Count;
    MenuOptions[selectedOption].SwitchTextHighlight(true);
  }

  private void UpdateMainMenu(int newIdx)
  {
    MenuOptions[selectedOption].SwitchTextHighlight(false);
    selectedOption = newIdx;
    MenuOptions[selectedOption].SwitchTextHighlight(true);
  }

  private void MenuItemButtonClicked(MenuItem menuItem)
  {
    int newIdx = MenuOptions.IndexOf(menuItem);
    UpdateMainMenu(newIdx);
    MenuSelectEvent(selectedOption);
  }
}
