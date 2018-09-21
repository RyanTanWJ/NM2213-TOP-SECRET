using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
  
  int maxRows = 7;
  int maxCols = 7;

  int playerX = 0;
  int playerY = 0;

  [SerializeField]
  float offset;

  [SerializeField]
  GameObject floor;

  [SerializeField]
  GameObject playerPrefab;
  GameObject playerObj;

  GameObject[,] grid;

	// Use this for initialization
	void Start () {
    grid = new GameObject[maxRows, maxCols];
    GeneratePlatforms();
    CreatePlayer();
	}
	
	void GeneratePlatforms()
  {
    for (int i = 0; i < maxRows; i++)
    {
      for (int j = 0; j < maxCols; j++)
      {
        //Instantiate(floor, tilePosition, transform.rotation);
        GameObject tile = Instantiate(floor, this.transform);
        Vector3 tilePosition = GetGridPosition(i, j);
        tile.transform.position = tilePosition;
      }
    }
  }

  private Vector3 GetGridPosition(int i, int j)
  {
    Vector3 tilePosition = new Vector3(transform.position.x, transform.position.y);
    tilePosition.x += j * offset;
    tilePosition.y += i * offset;
    return tilePosition;
  }

  void CreatePlayer()
  {
    playerX = maxRows / 2;
    playerY = maxCols / 2;
    playerObj = Instantiate(playerPrefab, transform);
    playerObj.transform.position = GetGridPosition(playerX, playerY);
    grid[playerX, playerY] = playerObj;
  }

  private void MovePlayerGraphic()
  {
    playerObj.transform.position = GetGridPosition(playerX, playerY);
  }

  private void MovePlayerUp()
  {
    if (playerX < maxRows - 1)
    {
      playerX += 1;
    }
  }

  private void MovePlayerDown()
  {
    if (playerX > 0)
    {
      playerX -= 1;
    }
  }

  private void MovePlayerLeft()
  {
    if (playerY > 0)
    {
      playerY -= 1;
    }
  }

  private void MovePlayerRight()
  {
    if (playerY < maxCols - 1)
    {
      playerY += 1;
    }
  }
  
  public void MovePlayerHori(bool right)
  {
    if (right)
    {
      MovePlayerRight();
    }
    else
    {
      MovePlayerLeft();
    }
    MovePlayerGraphic();
  }

  public void MovePlayerVert(bool up)
  {
    if (up)
    {
      MovePlayerUp();
    }
    else
    {
      MovePlayerDown();
    }
    MovePlayerGraphic();
  }
}
