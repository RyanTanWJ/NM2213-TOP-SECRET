using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

  public delegate void StartNewWave();
  public static event StartNewWave StartNewWaveEvent;

  public enum Direction { UP, DOWN, LEFT, RIGHT};

  int waveNumber = 0; //TODO: Count the number of waves cleared

  int maxRows = 8;
  int maxCols = 8;

  //The numerical offset between each tile
  [SerializeField]
  float offset;

  //Parent holder for platforms
  [SerializeField]
  GameObject platforms;

  List<GameObject> TopIndicators = new List<GameObject>();
  List<GameObject> BotIndicators = new List<GameObject>();
  List<GameObject> LeftIndicators = new List<GameObject>();
  List<GameObject> RightIndicators = new List<GameObject>();

  //The Prefab used for the floor
  [SerializeField]
  GameObject floor;

  //The Prefab used for the floor player cannot walk on
  [SerializeField]
  GameObject badFloor;

  //The Prefab used for the arrow indicators
  [SerializeField]
  GameObject arrowIndicator;

  [SerializeField]
  GameObject playerPrefab;

  [SerializeField]
  Player player;

  [SerializeField]
  BoulderPool boulderPool;

  List<Boulder> hazards = new List<Boulder>();

  float currentWaveTime = 0;
  float currentWaveTimer = 0;

  float currentWaveDelay = 0;
  float currentWaveDelayTimer = 0;

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
    GeneratePlatforms();
    GenerateIndicators();
    PlacePlayer();
	}

  private void Update()
  {
    if (hazards.Count<=0)
    {
      StartNewWaveEvent();
    }
    else
    {
      if (currentWaveDelay > currentWaveDelayTimer)
      {
        if (currentWaveTime > currentWaveTimer)
        {
          RemoveAllHazardsAndNuisances();
          currentWaveTime = 0;
          currentWaveDelay = 0;
        }
        else
        {
          currentWaveTime += Time.deltaTime;
        }
      }
      else
      {
        currentWaveDelay += Time.deltaTime;
      }
    }
  }

  void GeneratePlatforms()
  {
    for (int i = 0; i < maxRows; i++)
    {
      for (int j = 0; j < maxCols; j++)
      {
        GameObject tile;
        if (i == 0 || j == 0 || i == maxRows - 1 || j == maxCols - 1)
        {
          tile = Instantiate(badFloor, platforms.transform);
        }
        else
        {
          tile = Instantiate(floor, platforms.transform);
        }
        Vector3 tilePosition = GetGridPosition(i, j);
        tile.transform.position = tilePosition;
      }
    }
  }

  void GenerateIndicators()
  {
    for (int i = 0; i < maxRows; i++)
    {
      GameObject indicator = Instantiate(arrowIndicator, platforms.transform);
      Vector3 tilePosition = GetGridPosition(i, 0);
      indicator.transform.position = tilePosition;
      LeftIndicators.Add(indicator);
      indicator.SetActive(false);
      GameObject indicator2 = Instantiate(arrowIndicator, platforms.transform);
      Vector3 tilePosition2 = GetGridPosition(i, maxCols - 1);
      indicator2.transform.position = tilePosition2;
      RightIndicators.Add(indicator2);
      indicator2.transform.eulerAngles = new Vector3(0, 0, 180);
      indicator2.SetActive(false);
    }
    for (int j = 0; j < maxCols; j++)
    {
      GameObject indicator = Instantiate(arrowIndicator, platforms.transform);
      Vector3 tilePosition = GetGridPosition(0, j);
      indicator.transform.position = tilePosition;
      BotIndicators.Add(indicator);
      indicator.transform.eulerAngles = new Vector3(0, 0, 90);
      indicator.SetActive(false);

      GameObject indicator2 = Instantiate(arrowIndicator, platforms.transform);
      Vector3 tilePosition2 = GetGridPosition(maxRows - 1, j);
      indicator2.transform.position = tilePosition2;
      TopIndicators.Add(indicator2);
      indicator2.transform.eulerAngles = new Vector3(0, 0, -90);
      indicator2.SetActive(false);
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
    SetPlayerGraphic();
    //grid[playerX, playerY] = playerObj;
  }

  private void SetPlayerGraphic()
  {
    player.gameObject.transform.position = GetGridPosition(player.x, player.y);
  }

  private void MovePlayerUp()
  {
    if (player.x < maxRows - 2)
    {
      player.x += 1;
    }
  }

  private void MovePlayerDown()
  {
    if (player.x > 1)
    {
      player.x -= 1;
    }
  }

  private void MovePlayerLeft()
  {
    if (player.y > 1)
    {
      player.y -= 1;
    }
  }

  private void MovePlayerRight()
  {
    if (player.y < maxCols - 2)
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
    SetPlayerGraphic();
    //PlayerSmoothMovement(player, GetGridPosition(player.x, player.y));
  }

  public void NewWave(Wave nextWave)
  {
    currentWaveTimer = nextWave.WaveTimer;
    currentWaveDelayTimer = nextWave.DelayTimer;

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
      Boulder boulder = hazardObject.GetComponent<Boulder>();
      hazards.Add(boulder);
      hazardObject.GetComponent<Boulder>().SetBoulderDirection(Direction.RIGHT);
      //int x = i + 1;
      int x = UnityEngine.Random.Range(1, 5);
      int y = -2;
      LeftIndicators[x].SetActive(true);
      SetBoulderGraphic(hazardObject, x, y);
      MoveBoulder(boulder, x, y);
    }
    //TODO: Place the boulders on a random row that does not already have something on it.
  }

  private void SetBoulderGraphic(GameObject boulder, int x, int y)
  {
    boulder.gameObject.transform.position = GetGridPosition(x, y);
  }

  private void MoveBoulder(Boulder boulder, int x, int y)
  {
    //Debug.Log("(x, y) = (" + x + ", " + y + ")");
    Debug.Log(GetGridPosition(x, y));
    int newX = x;
    int newY = y;
    switch (boulder.GetDirection())
    {
      case Direction.DOWN:
        newX = -1;
        break;
      case Direction.UP:
        newX = maxRows;
        break;
      case Direction.LEFT:
        newY = -1;
        break;
      case Direction.RIGHT:
        newY = maxCols;
        break;
    }
    //Debug.Log("(newX, newY) = (" + newX + ", " + newY + ")");
    Debug.Log(GetGridPosition(newX, newY));
    StartCoroutine(HazardSmoothMovement(boulder, GetGridPosition(newX, newY), boulder.MoveTime));
    //if (currentWaveDelay > currentWaveDelayTimer)
    //{
    //  LeftIndicators[x].SetActive(false);
    //}
  }

  //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
  IEnumerator HazardSmoothMovement(Boulder boulder, Vector3 endPos, float moveTime)
  {
    GameObject gameObj = boulder.gameObject;

    //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
    //Square magnitude is used instead of magnitude because it's computationally cheaper.
    float sqrRemainingDistance = (gameObj.transform.position - endPos).sqrMagnitude;

    //While that distance is greater than a very small amount (Epsilon, almost zero):
    while (sqrRemainingDistance > float.Epsilon)
    {
      //Find a new position proportionally closer to the end, based on the moveTime
      float inverseMoveTime = 1.0f / moveTime;
      Vector3 newPostion = Vector3.MoveTowards(gameObj.transform.position, endPos, 0.1f);
      Debug.Log("currPosition = " + gameObj.transform.position);
      Debug.Log("newPosition = " + newPostion);

      //Set the current transform's position to the new position
      gameObj.transform.position = newPostion;

      //Recalculate the remaining distance after moving.
      sqrRemainingDistance = (gameObj.transform.position - endPos).sqrMagnitude;

      //Return and loop until sqrRemainingDistance is close enough to zero to end the function
      yield return null;
    }
  }

  /*
  //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
  IEnumerator PlayerSmoothMovement(Player player, Vector3 endPos, float moveTime=0.1f)
  {
    GameObject gameObj = player.gameObject;

    //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
    //Square magnitude is used instead of magnitude because it's computationally cheaper.
    float sqrRemainingDistance = (gameObj.transform.position - endPos).sqrMagnitude;

    //While that distance is greater than a very small amount (Epsilon, almost zero):
    while (sqrRemainingDistance > float.Epsilon)
    {
      //Find a new position proportionally closer to the end, based on the moveTime
      float inverseMoveTime = 1.0f / moveTime;
      Vector3 newPostion = Vector3.MoveTowards(gameObj.transform.position, endPos, inverseMoveTime * Time.deltaTime * 10.0f);

      //Set the current transform's position to the new position
      gameObj.transform.position = newPostion;

      //Recalculate the remaining distance after moving.
      sqrRemainingDistance = (gameObj.transform.position - endPos).sqrMagnitude;

      //Return and loop until sqrRemainingDistance is close enough to zero to end the function
      yield return null;
    }
  }
  */

  private void RemoveAllHazardsAndNuisances()
  {
    foreach (Boulder hazard in hazards)
    {
      boulderPool.ReturnBoulder(hazard);
    }
    hazards.Clear();
    //TODO: Remove Nuisances
  }

}
