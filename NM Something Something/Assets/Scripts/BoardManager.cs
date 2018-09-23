using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

  public delegate void StartNewWave();
  public static event StartNewWave StartNewWaveEvent;

  public enum Direction { UP, DOWN, LEFT, RIGHT};

  int waveNumber = 0;

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

  [SerializeField]
  BoulderPool boulderPool;

  GameObject[,] grid;

  private bool waveEnded;

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
    StartNewWaveEvent();
	}

  private void Update()
  {
    waveEnded = MoveHazards();
    if (waveEnded)
    {
      StartNewWaveEvent();
    }

  }

  private bool MoveHazards()
  {
    bool waveOver = true;
    for (int i = 0; i < maxRows; i++)
    {
      for (int j = 0; j < maxCols; j++)
      {
        if (grid[i, j] == null)
        {
          continue;
        }
        Boulder boulder = grid[i, j].GetComponent<Boulder>();
        if (boulder != null)
        {
          if (boulder.ReadyToMove())
          {
            MoveBoulder(boulder, i, j);
          }
          else
          {
            boulder.UpdateLastMove(Time.deltaTime);
          }
          waveOver = false;
        }
      }
    }
    return waveOver;
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

  public void NewWave(Wave nextWave)
  {
    for (int i = 0; i < nextWave.HazardNum; i++)
    {
      GameObject hazardObject = boulderPool.RetrieveBoulder();
      //TODO: Implement Switch Case for different Hazards
      /*
      switch (nextWave.Hazard)
      {
        default:
          hazardObject = boulderPool.RetrieveBoulder();
          break;
      }
      */
      hazardObject.GetComponent<Boulder>().SetBoulderDirection(Direction.RIGHT);
      grid[i + 1, 0] = hazardObject;
      MoveBoulderGraphic(hazardObject, i+1, 0);
    }
    //TODO: Place the boulders on a random row that does not already have something on it.
  }

  private void MoveBoulderGraphic(GameObject boulder, int x, int y)
  {
    boulder.gameObject.transform.position = GetGridPosition(x, y);
  }

  private void MoveBoulder(Boulder boulder, int x, int y)
  {
    int newX = x;
    int newY = y;
    bool remove = false;
    switch (boulder.GetDirection())
    {
      case Direction.DOWN:
        newX = x - 1;
        //if (newX < 0)
        //{
        //  remove = true;
        //}
        break;
      case Direction.UP:
        newX = x + 1;
        //if (newX >= maxRows)
        //{
        //  remove = true;
        //}
        break;
      case Direction.LEFT:
        newY = y - 1;
        //if (newY < 0)
        //{
        //  remove = true;
        //}
        break;
      case Direction.RIGHT:
        newY = y + 1;
        //if (newY >= maxCols)
        //{
        //  remove = true;
        //}
        break;
    }
    grid[x, y] = null;
    if (remove)
    {
      boulderPool.ReturnBoulder(boulder);
    }
    else
    {
      grid[newX, newY] = boulder.gameObject;
      MoveBoulderGraphic(boulder.gameObject, newX, newY);
    }
  }
}
