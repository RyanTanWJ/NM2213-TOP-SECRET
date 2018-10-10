using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

  public enum Direction { UP, DOWN, LEFT, RIGHT};

  int maxRows = 8;
  int maxCols = 8;

  //The numerical offset between each tile
  [SerializeField]
  float offset;

  //Parent holder for platforms
  [SerializeField]
  GameObject platforms;

  [SerializeField]
  IndicatorHandler indicatorHandler;

  //The Prefab used for the floor
  [SerializeField]
  GameObject floor;

  //The Prefab used to show the floor tiles the player cannot walk on
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

  [SerializeField]
  HazardSpawner hazSpawner;

  float spawnDelay = 2.0f;
  float currDelay = 2.0f;


  private void OnEnable()
  {
    Player.PlayerMoveEvent += MovePlayer;
    IndicatorHandler.SpawnHazardsEvent += SpawnHazard;
  }

  private void OnDisable()
  {
    Player.PlayerMoveEvent -= MovePlayer;
    IndicatorHandler.SpawnHazardsEvent -= SpawnHazard;
  }

  // Use this for initialization
  void Start () {
    GeneratePlatforms();
    GenerateIndicators();
    PlacePlayer();
	}

  private void Update()
  {
    if (currDelay >= spawnDelay)
    {
      //TODO: Flash Indicators for Indicator Time then spawn the hazard
      int hazards;
      float delay;
      List<int> rows;
      List<int> cols;
      hazSpawner.GetHazards(out hazards, out delay, out rows, out cols);

      for (int i = 0; i < hazards; i++)
      {
        int index = 0;
        switch (UnityEngine.Random.Range(0, 2))
        {
          //Do left or right
          case 0:
            switch (UnityEngine.Random.Range(0, 2))
            {
              //Do left
              case 0:
                index = UnityEngine.Random.Range(1, rows.Count);
                rows.RemoveAt(index);
                indicatorHandler.AcitvateIndicator(IndicatorHandler.IndicatorSet.LEFT, index, delay);
                break;
              //Do right
              case 1:
                index = UnityEngine.Random.Range(1, rows.Count);
                rows.RemoveAt(index);
                indicatorHandler.AcitvateIndicator(IndicatorHandler.IndicatorSet.RIGHT, index, delay);
                break;
              default:
                Debug.LogError("Random Range exceeded in BoardManager Update()");
                break;
            }
            break;
          //Do top or bot
          case 1:
            switch (UnityEngine.Random.Range(0, 2))
            {
              //Do top
              case 0:
                index = UnityEngine.Random.Range(1, cols.Count);
                cols.RemoveAt(index);
                indicatorHandler.AcitvateIndicator(IndicatorHandler.IndicatorSet.TOP, index, delay);
                break;
              //Do bot
              case 1:
                index = UnityEngine.Random.Range(1, cols.Count);
                cols.RemoveAt(index);
                indicatorHandler.AcitvateIndicator(IndicatorHandler.IndicatorSet.BOT, index, delay);
                break;
              default:
                Debug.LogError("Random Range exceeded in BoardManager Update()");
                break;
            }
            break;
          default:
            Debug.LogError("Random Range exceeded in BoardManager Update()");
            break;
        }
        /*
      switch (UnityEngine.Random.Range(0, 4))
      {
        case 0:
          indicatorHandler.AcitvateIndicator(IndicatorHandler.IndicatorSet.TOP, UnityEngine.Random.Range(1, maxRows - 1), 1.0f);
          break;
        case 1:
          indicatorHandler.AcitvateIndicator(IndicatorHandler.IndicatorSet.BOT, UnityEngine.Random.Range(1, maxRows - 1), 1.0f);
          break;
        case 2:
          indicatorHandler.AcitvateIndicator(IndicatorHandler.IndicatorSet.LEFT, UnityEngine.Random.Range(1, maxCols - 1), 1.0f);
          break;
        case 3:
          indicatorHandler.AcitvateIndicator(IndicatorHandler.IndicatorSet.RIGHT, UnityEngine.Random.Range(1, maxCols - 1), 1.0f);
          break;
        default:
          Debug.LogError("Random Range exceeded in BoardManager Update()");
          break;
      }
      */
      }
      currDelay = 0;
    }
    else
    {
      currDelay += Time.deltaTime;
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
    GameObject indicatorObj;
    Indicator indicator;
    Vector3 tilePosition;
    for (int i = 0; i < maxRows; i++)
    {
      indicatorObj = Instantiate(arrowIndicator, indicatorHandler.transform);
      tilePosition = GetGridPosition(i, 0);
      indicatorObj.transform.position = tilePosition;
      indicator = indicatorObj.GetComponent<Indicator>();
      indicatorHandler.AddIndicatorToSet(IndicatorHandler.IndicatorSet.LEFT, indicator);

      indicatorObj = Instantiate(arrowIndicator, indicatorHandler.transform);
      tilePosition = GetGridPosition(i, maxCols - 1);
      indicatorObj.transform.position = tilePosition;
      indicator = indicatorObj.GetComponent<Indicator>();
      indicatorHandler.AddIndicatorToSet(IndicatorHandler.IndicatorSet.RIGHT, indicator);
    }
    for (int j = 0; j < maxCols; j++)
    {
      indicatorObj = Instantiate(arrowIndicator, indicatorHandler.transform);
      tilePosition = GetGridPosition(0, j);
      indicatorObj.transform.position = tilePosition;
      indicator = indicatorObj.GetComponent<Indicator>();
      indicatorHandler.AddIndicatorToSet(IndicatorHandler.IndicatorSet.BOT, indicator);

      indicatorObj = Instantiate(arrowIndicator, indicatorHandler.transform);
      tilePosition = GetGridPosition(maxRows - 1, j);
      indicatorObj.transform.position = tilePosition;
      indicator = indicatorObj.GetComponent<Indicator>();
      indicatorHandler.AddIndicatorToSet(IndicatorHandler.IndicatorSet.TOP, indicator);
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
    player.BoardPosition.x = maxRows / 2;
    player.BoardPosition.y = maxCols / 2;
    SetPlayerGraphic();
  }

  private void SetPlayerGraphic()
  {
    player.gameObject.transform.position = GetGridPosition(player.BoardPosition.x, player.BoardPosition.y);
  }

  private void MovePlayerUp()
  {
    if (player.BoardPosition.x < maxRows - 2)
    {
      player.BoardPosition.x += 1;
    }
  }

  private void MovePlayerDown()
  {
    if (player.BoardPosition.x > 1)
    {
      player.BoardPosition.x -= 1;
    }
  }

  private void MovePlayerLeft()
  {
    if (player.BoardPosition.y > 1)
    {
      player.BoardPosition.y -= 1;
    }
  }

  private void MovePlayerRight()
  {
    if (player.BoardPosition.y < maxCols - 2)
    {
      player.BoardPosition.y += 1;
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
  }

  private void SetBoulderGraphic(GameObject boulder, int x, int y)
  {
    boulder.gameObject.transform.position = GetGridPosition(x, y);
  }

  private void MoveBoulder(Boulder boulder)
  {
    Vector3 direction = new Vector3();

    switch (boulder.GetDirection())
    {
      case Direction.UP:
        direction.y = 0.1f;
        break;
      case Direction.DOWN:
        direction.y = -0.1f;
        break;
      case Direction.RIGHT:
        direction.x = 0.1f;
        break;
      case Direction.LEFT:
        direction.x = -0.1f;
        break;
      default:
        break;
    }

    boulder.ShouldMove = true;
    StartCoroutine(HazardSmoothMovement(boulder, direction, boulder.Speed));
  }

  //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
  IEnumerator HazardSmoothMovement(Boulder boulder, Vector3 direction, float speed)
  {
    GameObject gameObj = boulder.gameObject;
    while (boulder.ShouldMove)
    {
      gameObj.transform.position = gameObj.transform.position + direction * speed * Time.deltaTime;
      yield return null;
    }
  }

  private void RemoveAllHazardsAndNuisances()
  {
    boulderPool.ReturnAllBoulders();
    //TODO: Remove Nuisances
  }
  
  private void SpawnHazard(Direction direction, Vector2Int spawnPos)
  {
    GameObject boulderObj = boulderPool.RetrieveBoulder();
    Boulder boulder = boulderObj.GetComponent<Boulder>();
    SetBoulderGraphic(boulderObj, spawnPos.x, spawnPos.y);
    switch (direction)
    {
      case Direction.RIGHT:
        boulder.SetBoulderDirection(Direction.RIGHT);
        MoveBoulder(boulder);
        break;
      case Direction.UP:
        boulder.SetBoulderDirection(Direction.UP);
        MoveBoulder(boulder);
        break;
      case Direction.LEFT:
        boulder.SetBoulderDirection(Direction.LEFT);
        MoveBoulder(boulder);
        break;
      default:
        boulder.SetBoulderDirection(Direction.DOWN);
        MoveBoulder(boulder);
        break;
    }
  }
}
