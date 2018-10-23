using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

  /// <summary>
  /// Represents a movement Direction
  /// </summary>
  public enum Direction { UP, DOWN, LEFT, RIGHT};

  public enum Hazard { BOULDER, LASER, CLAW, PUFFERFISH };

  /// <summary>
  /// Represents the border on which certain objects such as Laser and Indicators are located.
  /// </summary>
  public enum BorderSet { TOP, BOT, LEFT, RIGHT };

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
  Player player;

  [SerializeField]
  BoulderPool boulderPool;

  [SerializeField]
  ClawPool clawPool;

  [SerializeField]
  PufferfishPool pufferfishPool;

  [SerializeField]
  HazardSpawner hazSpawner;

  //The Prefab used for lasers
  [SerializeField]
  GameObject laser;

  [SerializeField]
  LaserHandler laserHandler;

  DangerBoard dangerBoard;

  float spawnDelay = 2.5f;
  float currDelay = 0.0f;

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
    dangerBoard = new DangerBoard(maxRows, maxCols);
    GeneratePlatforms();
    GenerateIndicators();
    GenerateLasers();
    PlacePlayer();
  }

  private void Update()
  {
    if (currDelay >= spawnDelay)
    {
      //Flash Indicators for Indicator Time (delay) then spawn the hazard
      int hazards;
      float delay;
      List<int> rows;
      List<int> cols;

      hazSpawner.GetHazards(player.BoardPosition, dangerBoard.GetDangerBoard(), out hazards, out delay, out rows, out cols);

      Hazard hazardToSpawn = UnityEngine.Random.Range(0,1f)<0.5 ? Hazard.BOULDER : Hazard.LASER;
      
      for (int i = 0; i < hazards; i++)
      {
        int rowColIndex = 0;
        switch (UnityEngine.Random.Range(0, 4))
        {
          //Do left
          case 0:
            rowColIndex = rows[UnityEngine.Random.Range(0, rows.Count)];
            indicatorHandler.AcitvateIndicator(hazardToSpawn, BorderSet.LEFT, rowColIndex, delay);
            break;
          //Do right
          case 1:
            rowColIndex = rows[UnityEngine.Random.Range(0, rows.Count)];
            indicatorHandler.AcitvateIndicator(hazardToSpawn, BorderSet.RIGHT, rowColIndex, delay);
            break;
          //Do top
          case 2:
            rowColIndex = cols[UnityEngine.Random.Range(0, cols.Count)];
            indicatorHandler.AcitvateIndicator(hazardToSpawn, BorderSet.TOP, rowColIndex, delay);
            break;
          //Do bot
          case 3:
            rowColIndex = cols[UnityEngine.Random.Range(0, cols.Count)];
            indicatorHandler.AcitvateIndicator(hazardToSpawn, BorderSet.BOT, rowColIndex, delay);
            break;
          default:
            Debug.LogError("Random Range exceeded in BoardManager Update()");
            break;
        }
      }
      dangerBoard.Print();
      currDelay = 0;
    }
    else
    {
      currDelay += Time.deltaTime;
    }

    dangerBoard.UpdateDangerBoard(Time.deltaTime);
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
    for (int i = 0; i < maxRows-1; i++)
    {
      indicatorObj = Instantiate(arrowIndicator, indicatorHandler.transform);
      tilePosition = GetGridPosition(i, 0);
      indicatorObj.transform.position = tilePosition;
      indicator = indicatorObj.GetComponent<Indicator>();
      indicatorHandler.AddIndicatorToSet(BorderSet.LEFT, indicator);

      indicatorObj = Instantiate(arrowIndicator, indicatorHandler.transform);
      tilePosition = GetGridPosition(i, maxCols - 1);
      indicatorObj.transform.position = tilePosition;
      indicator = indicatorObj.GetComponent<Indicator>();
      indicatorHandler.AddIndicatorToSet(BorderSet.RIGHT, indicator);
    }
    for (int j = 0; j < maxCols-1; j++)
    {
      indicatorObj = Instantiate(arrowIndicator, indicatorHandler.transform);
      tilePosition = GetGridPosition(0, j);
      indicatorObj.transform.position = tilePosition;
      indicator = indicatorObj.GetComponent<Indicator>();
      indicatorHandler.AddIndicatorToSet(BorderSet.BOT, indicator);

      indicatorObj = Instantiate(arrowIndicator, indicatorHandler.transform);
      tilePosition = GetGridPosition(maxRows - 1, j);
      indicatorObj.transform.position = tilePosition;
      indicator = indicatorObj.GetComponent<Indicator>();
      indicatorHandler.AddIndicatorToSet(BorderSet.TOP, indicator);
    }
  }

  void GenerateLasers()
  {
    GameObject laserObj;
    Laser laserRef;
    Vector3 tilePosition;
    for (int i = 0; i < maxRows; i++)
    {
      laserObj = Instantiate(laser, laserHandler.transform);
      tilePosition = GetGridPosition(i, 0);
      laserObj.transform.position = tilePosition;
      laserRef = laserObj.GetComponent<Laser>();
      laserHandler.AddLaserToSet(BorderSet.LEFT, laserRef);

      laserObj = Instantiate(laser, laserHandler.transform);
      tilePosition = GetGridPosition(i, maxCols - 1);
      laserObj.transform.position = tilePosition;
      laserRef = laserObj.GetComponent<Laser>();
      laserHandler.AddLaserToSet(BorderSet.RIGHT, laserRef);
    }
    for (int j = 0; j < maxCols; j++)
    {
      laserObj = Instantiate(laser, laserHandler.transform);
      tilePosition = GetGridPosition(0, j);
      laserObj.transform.position = tilePosition;
      laserRef = laserObj.GetComponent<Laser>();
      laserHandler.AddLaserToSet(BorderSet.BOT, laserRef);

      laserObj = Instantiate(laser, laserHandler.transform);
      tilePosition = GetGridPosition(maxRows - 1, j);
      laserObj.transform.position = tilePosition;
      laserRef = laserObj.GetComponent<Laser>();
      laserHandler.AddLaserToSet(BorderSet.TOP, laserRef);
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
  
  private void SpawnHazard(Hazard hazardType, Direction direction, Vector2Int spawnPos)
  {
    switch (hazardType)
    {
      case Hazard.BOULDER:
        TriggerBoulderHazard(direction, spawnPos);
        break;
      case Hazard.LASER:
        TriggerLaserHazard(direction, spawnPos);
        break;
      default:
        Debug.LogError("No such Hazard type");
        break;
    }
  }

  private void TriggerBoulderHazard(Direction direction, Vector2Int spawnPos)
  {
    GameObject boulderObj = boulderPool.RetrieveBoulder();
    Boulder boulder = boulderObj.GetComponent<Boulder>();
    SetBoulderGraphic(boulderObj, spawnPos.x, spawnPos.y);
    switch (direction)
    {
      case Direction.RIGHT:
        for (int i=0; i<maxCols; i++)
        {
          dangerBoard.AddDangerBoard(new Vector2Int(spawnPos.x, i), 1.0f);
        }
        boulder.SetBoulderDirection(Direction.RIGHT);
        MoveBoulder(boulder);
        break;
      case Direction.UP:
        for (int i = 0; i < maxRows; i++)
        {
          dangerBoard.AddDangerBoard(new Vector2Int(i, spawnPos.y), 1.0f);
        }
        boulder.SetBoulderDirection(Direction.UP);
        MoveBoulder(boulder);
        break;
      case Direction.LEFT:
        for (int i = 0; i < maxCols; i++)
        {
          dangerBoard.AddDangerBoard(new Vector2Int(spawnPos.x, i), 1.0f);
        }
        boulder.SetBoulderDirection(Direction.LEFT);
        MoveBoulder(boulder);
        break;
      default:
        for (int i = 0; i < maxRows; i++)
        {
          dangerBoard.AddDangerBoard(new Vector2Int(i, spawnPos.y), 1.0f);
        }
        boulder.SetBoulderDirection(Direction.DOWN);
        MoveBoulder(boulder);
        break;
    }
  }

  private void TriggerLaserHazard(Direction direction, Vector2Int spawnPos)
  {
    switch (direction)
    {
      case Direction.RIGHT:
        for (int i = 0; i < maxCols; i++)
        {
          dangerBoard.AddDangerBoard(new Vector2Int(spawnPos.x, i), 3.0f);
        }
        laserHandler.AcitvateLaser(BorderSet.LEFT, spawnPos.x);
        break;
      case Direction.UP:
        for (int i = 0; i < maxRows; i++)
        {
          dangerBoard.AddDangerBoard(new Vector2Int(i, spawnPos.y), 3.0f);
        }
        laserHandler.AcitvateLaser(BorderSet.BOT, spawnPos.y);
        break;
      case Direction.LEFT:
        for (int i = 0; i < maxCols; i++)
        {
          dangerBoard.AddDangerBoard(new Vector2Int(spawnPos.x, i), 3.0f);
        }
        laserHandler.AcitvateLaser(BorderSet.RIGHT, spawnPos.x);
        break;
      default:
        for (int i = 0; i < maxRows; i++)
        {
          dangerBoard.AddDangerBoard(new Vector2Int(i, spawnPos.y), 3.0f);
        }
        laserHandler.AcitvateLaser(BorderSet.TOP, spawnPos.y);
        break;
    }
  }
}
