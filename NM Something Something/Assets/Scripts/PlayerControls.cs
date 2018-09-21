using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

  [SerializeField]
  BoardManager boardManager;
  
	// Update is called once per frame
	void Update ()
  {
    CheckForPlayerInput();
  }

  private void CheckForPlayerInput()
  {
    if (Input.GetKeyDown(KeyCode.UpArrow))
    {
      boardManager.MovePlayerVert(true);
    }
    else if (Input.GetKeyDown(KeyCode.DownArrow))
    {
      boardManager.MovePlayerVert(false);
    }
    else if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
      boardManager.MovePlayerHori(false);
    }
    else if (Input.GetKeyDown(KeyCode.RightArrow))
    {
      boardManager.MovePlayerHori(true);
    }
  }
}
