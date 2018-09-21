using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

  public enum Direction { UP, DOWN, LEFT, RIGHT};

  int maxRows = 7;
  int maxCols = 7;

  //The numerical offset between each tile
  [SerializeField]
  float offset;

  //The Prefab used for the floor
  [SerializeField]
  GameObject floor;

  [SerializeField]
  GameObject playerPrefab;

  [SerializeField]
  Player player;

  GameObject[,] grid;

  private void OnEnable()
  {
    Player.PlayerMoveEvent += MovePlayer;
  }

  private void OnDisable()
  {
    Player.PlayerMoveEvent -= MovePlayer;
  }

  // Use this for initialization
  void Start () {
    grid = new GameObject[maxRows, maxCols];
    GeneratePlatforms();
    PlacePlayer();
	}
	
	void GeneratePlatforms()
  {
    for (int i = 0; i < maxRows; i++)
    {
      for (int j = 0; j < maxCols; j++)
      {
        //Instantiate(floor, tilePosition, transform.rotation);
        GameObject tile = Instantiate(floor, transform);
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

  void PlacePlayer()
  {
    player.x = maxRows / 2;
    player.y = maxCols / 2;
    MovePlayerGraphic();
    //grid[playerX, playerY] = playerObj;
  }

  private void MovePlayerGraphic()
  {
    player.gameObject.transform.position = GetGridPosition(player.x, player.y);
  }

  private void MovePlayerUp()
  {
    if (player.x < maxRows - 1)
    {
      player.x += 1;
    }
  }

  private void MovePlayerDown()
  {
    if (player.x > 0)
    {
      player.x -= 1;
    }
  }

  private void MovePlayerLeft()
  {
    if (player.y > 0)
    {
      player.y -= 1;
    }
  }

  private void MovePlayerRight()
  {
    if (player.y < maxCols - 1)
    {
      player.y += 1;
    }
  }

  private void MovePlayer(Direction dir)
  {
    switch (dir)
    {
      case Direction.UP:
        MovePlayerUp();
        break;
      case Direction.DOWN:
        MovePlayerDown();
        break;
      case Direction.LEFT:
        MovePlayerLeft();
        break;
      case Direction.RIGHT:
        MovePlayerRight();
        break;
      default:
        break;
    }
    MovePlayerGraphic();
  }
}
