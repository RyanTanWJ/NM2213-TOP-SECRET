using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
  /// <summary>
  /// Represents a movement Direction
  /// </summary>
  public enum Direction { UP, DOWN, LEFT, RIGHT };

  public enum Hazard { SUSHI, WASABI, CLAW, PUFFERFISH };

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
  GameObject borderPlatforms;

  [SerializeField]
  IndicatorHandler indicatorHandler;

  //The Prefab used for the floor
  [SerializeField]
  GameObject tiledFloor;

  //The Prefabs used to show the floor tiles the player cannot walk on
  [SerializeField]
  GameObject tabledFloor;
  [SerializeField]
  GameObject tabledFloorAlter;
  [SerializeField]
  List<GameObject> tabledFloorAlterTop;
  [SerializeField]
  List<GameObject> tabledFloorAlterBot;

  //The Prefab used for the arrow indicators
  [SerializeField]
  GameObject WarningBubbleIndicator;

  [SerializeField]
  Player player;

  [SerializeField]
  SushiPool sushiPool;

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

  float spawnDelay = 2.0f;
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
  void Start()
  {
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
      //dangerBoard.Print();
      List<HazardContainer> hazContainers = hazSpawner.GetHazards(player.BoardPosition, dangerBoard.GetDangerBoard());
      //Debug.Log("__________hazContainers.Count = " + hazContainers.Count);
      if (hazContainers.Count > 0)
      {
        //Debug.Log("Spawning Hazards:");
        foreach (HazardContainer hazardContainer in hazContainers)
        {
          //hazardContainer.PrintContents();
          StartCoroutine(SpawnHazard(hazardContainer));
        }
      }
      //dangerBoard.Print();
      currDelay = 0;
    }
    else
    {
      currDelay += Time.deltaTime;
    }
  }

  private void LateUpdate()
  {
    dangerBoard.UpdateDangerBoard(Time.deltaTime);
  }

  void GeneratePlatforms()
  {
    bool bottomOffset = false;
    for (int i = 0; i < maxRows; i++)
    {
      for (int j = 0; j < maxCols; j++)
      {
        GameObject tile;
        if (i == 0 && (j == 0 || j == maxCols - 1))
        {
          tile = Instantiate(tabledFloorAlter, borderPlatforms.transform);
        }
        else if (i == 0)
        {
          bottomOffset = true;
          tile = Instantiate(tabledFloorAlterBot[UnityEngine.Random.Range(0, tabledFloorAlterBot.Count)], borderPlatforms.transform);
        }
        else if (j == 0 || j == maxCols - 1)
        {
          tile = Instantiate(tabledFloor, borderPlatforms.transform);
        }
        else if (i == maxRows - 1)
        {
          tile = Instantiate(tabledFloorAlterTop[UnityEngine.Random.Range(0, tabledFloorAlterTop.Count)], borderPlatforms.transform);
        }
        else
        {
          tile = Instantiate(tiledFloor, platforms.transform);
        }
        Vector3 tilePosition = GetGridPosition(i, j);
        if (bottomOffset)
        {
          tilePosition.y -= offset / 2;
          bottomOffset = false;
        }
        tile.transform.position = tilePosition;
      }
    }
  }

  void GenerateIndicators()
  {
    GameObject indicatorObj;
    Indicator indicator;
    Vector3 tilePosition;
    for (int i = 0; i < maxRows - 1; i++)
    {
      indicatorObj = Instantiate(WarningBubbleIndicator, indicatorHandler.transform);
      tilePosition = GetGridPosition(i, -1);
      tilePosition.x = tilePosition.x - offset / 2;
      indicatorObj.transform.position = tilePosition;
      indicator = indicatorObj.GetComponent<Indicator>();
      indicatorHandler.AddIndicatorToSet(BorderSet.LEFT, indicator);

      indicatorObj = Instantiate(WarningBubbleIndicator, indicatorHandler.transform);
      tilePosition = GetGridPosition(i, maxCols);
      tilePosition.x = tilePosition.x + offset / 2;
      indicatorObj.transform.position = tilePosition;
      indicator = indicatorObj.GetComponent<Indicator>();
      indicatorHandler.AddIndicatorToSet(BorderSet.RIGHT, indicator);
    }
    for (int j = 0; j < maxCols - 1; j++)
    {
      indicatorObj = Instantiate(WarningBubbleIndicator, indicatorHandler.transform);
      tilePosition = GetGridPosition(0, j);
      tilePosition.y = tilePosition.y - offset / 3 *2;
      indicatorObj.transform.position = tilePosition;
      indicator = indicatorObj.GetComponent<Indicator>();
      indicatorHandler.AddIndicatorToSet(BorderSet.BOT, indicator);

      indicatorObj = Instantiate(WarningBubbleIndicator, indicatorHandler.transform);
      tilePosition = GetGridPosition(maxRows, j);
      tilePosition.y = tilePosition.y + offset / 2;
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

  private void SetHazardPosition(GameObject boulder, int x, int y)
  {
    boulder.gameObject.transform.position = GetGridPosition(x, y);
    boulder.transform.rotation = Quaternion.identity;
  }

  private void MoveBoulder(Sushi boulder)
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

  private void MoveClaw(Claw claw)
  {
    Vector3 direction = new Vector3();

    switch (claw.GetDirection())
    {
      case Direction.UP:
        direction.y = 0.1f;
        direction.x = 0;
        break;
      case Direction.DOWN:
        direction.y = -0.1f;
        direction.x = 0;
        break;
      case Direction.RIGHT:
        direction.x = 0.1f;
        direction.y = 0;
        break;
      case Direction.LEFT:
        direction.x = -0.1f;
        direction.y = 0;
        break;
      default:
        break;
    }

    claw.ShouldMove = true;
    StartCoroutine(HazardSmoothMovement(claw, direction, claw.Speed));
  }

  //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
  IEnumerator HazardSmoothMovement(Sushi boulder, Vector3 direction, float speed)
  {
    GameObject gameObj = boulder.gameObject;
    while (boulder.ShouldMove)
    {
      gameObj.transform.position = gameObj.transform.position + direction * speed * Time.deltaTime;
      yield return null;
    }
  }

  //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
  IEnumerator HazardSmoothMovement(Claw claw, Vector3 direction, float speed)
  {
    GameObject gameObj = claw.gameObject;
    while (claw.ShouldMove)
    {
      gameObj.transform.position = gameObj.transform.position + direction * speed * Time.deltaTime;
      yield return null;
    }
  }

  private void RemoveAllHazardsAndNuisances()
  {
    sushiPool.ReturnAllSushi();
    //TODO: Remove Nuisances
  }

  private void SpawnHazard(Hazard hazardType, Direction direction, Vector2Int spawnPos)
  {
    switch (hazardType)
    {
      case Hazard.SUSHI:
        TriggerBoulderHazard(direction, spawnPos);
        break;
      case Hazard.WASABI:
        TriggerLaserHazard(direction, spawnPos);
        break;
      case Hazard.CLAW:
        TriggerClawHazard(direction, spawnPos);
        break;
      default:
        Debug.LogError("Hazard type: " + hazardType + " not handled in SpawnHazard method.");
        break;
    }
  }

  private void TriggerBoulderHazard(Direction direction, Vector2Int spawnPos)
  {
    GameObject sushiObj = sushiPool.RetrieveSushi();
    Sushi boulder = sushiObj.GetComponent<Sushi>();
    SetHazardPosition(sushiObj, spawnPos.x, spawnPos.y);
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

  private void TriggerLaserHazard(Direction direction, Vector2Int spawnPos)
  {
    switch (direction)
    {
      case Direction.RIGHT:
        laserHandler.AcitvateLaser(BorderSet.LEFT, spawnPos.x);
        break;
      case Direction.UP:
        laserHandler.AcitvateLaser(BorderSet.BOT, spawnPos.y);
        break;
      case Direction.LEFT:
        laserHandler.AcitvateLaser(BorderSet.RIGHT, spawnPos.x);
        break;
      default:
        laserHandler.AcitvateLaser(BorderSet.TOP, spawnPos.y);
        break;
    }
  }

  private void TriggerClawHazard(Direction direction, Vector2Int spawnPos)
  {
    GameObject clawObj = clawPool.RetrieveClaw();
    Claw claw = clawObj.GetComponent<Claw>();
    SetHazardPosition(clawObj, spawnPos.x, spawnPos.y);
    switch (direction)
    {
      case Direction.RIGHT:
        claw.SetClawDirection(Direction.RIGHT);
        MoveClaw(claw);
        break;
      case Direction.UP:
        claw.SetClawDirection(Direction.UP);
        MoveClaw(claw);
        break;
      case Direction.LEFT:
        claw.SetClawDirection(Direction.LEFT);
        MoveClaw(claw);
        break;
      default:
        claw.SetClawDirection(Direction.DOWN);
        MoveClaw(claw);
        break;
    }
  }

  private void TriggerPufferfishHazard(float indicatorTime, Vector2Int spawnPos)
  {
    GameObject pufferfishObj = pufferfishPool.RetrievePufferfish();
    Pufferfish pufferfish = pufferfishObj.GetComponent<Pufferfish>();
    SetHazardPosition(pufferfishObj, spawnPos.x, spawnPos.y);
    pufferfish.TimeBomb(indicatorTime);
  }

  private void AddPufferfishDangerTimer(Vector2Int spawnPos, float extraDelay)
  {
    const float pufferfishDangerTimer = 2.5f;

    //Middle
    dangerBoard.AddDangerBoard(new Vector2Int(spawnPos.x, spawnPos.y), pufferfishDangerTimer);
    //Right
    dangerBoard.AddDangerBoard(new Vector2Int(spawnPos.x + 1, spawnPos.y), pufferfishDangerTimer);
    //Left
    dangerBoard.AddDangerBoard(new Vector2Int(spawnPos.x - 1, spawnPos.y), pufferfishDangerTimer);
    //Up
    dangerBoard.AddDangerBoard(new Vector2Int(spawnPos.x, spawnPos.y + 1), pufferfishDangerTimer);
    //Down
    dangerBoard.AddDangerBoard(new Vector2Int(spawnPos.x, spawnPos.y - 1), pufferfishDangerTimer);
    //Top Right
    dangerBoard.AddDangerBoard(new Vector2Int(spawnPos.x + 1, spawnPos.y + 1), pufferfishDangerTimer);
    //Top Left
    dangerBoard.AddDangerBoard(new Vector2Int(spawnPos.x - 1, spawnPos.y + 1), pufferfishDangerTimer);
    //Bot Right
    dangerBoard.AddDangerBoard(new Vector2Int(spawnPos.x + 1, spawnPos.y - 1), pufferfishDangerTimer);
    //Bot Left
    dangerBoard.AddDangerBoard(new Vector2Int(spawnPos.x - 1, spawnPos.y - 1), pufferfishDangerTimer);
  }

  private void AddHorizontalDangerBoard(Vector2Int spawnPos, float boulderDangerTimer)
  {
    for (int i = 0; i < maxCols; i++)
    {
      dangerBoard.AddDangerBoard(new Vector2Int(spawnPos.x, i), boulderDangerTimer);
    }
  }

  private void AddVerticalDangerBoard(Vector2Int spawnPos, float boulderDangerTimer)
  {
    for (int i = 0; i < maxRows; i++)
    {
      dangerBoard.AddDangerBoard(new Vector2Int(i, spawnPos.y), boulderDangerTimer);
    }
  }

  IEnumerator SpawnHazard(HazardContainer hazardContainer)
  {
    //Debug.Log("___Entered SpawnHazard Coroutine___");
    if (hazardContainer.HazardToSpawn == Hazard.PUFFERFISH)
    {
      AddPufferfishDangerTimer(hazardContainer.HazardSpawnPoint, hazardContainer.CombinedDelay);
      yield return new WaitForSeconds(hazardContainer.PreIndicatorDelay);
      TriggerPufferfishHazard(hazardContainer.IndicatorDelay, hazardContainer.HazardSpawnPoint);
    }
    else
    {
      const float boulderTime = 1.0f;
      const float clawTime = 0.5f;
      const float laserTime = 3.2f;
      if (hazardContainer.BorderSet == BorderSet.BOT || hazardContainer.BorderSet == BorderSet.TOP)
      {
        switch (hazardContainer.HazardToSpawn)
        {
          case Hazard.SUSHI:
            AddVerticalDangerBoard(hazardContainer.HazardSpawnPoint, hazardContainer.CombinedDelay + boulderTime);
            break;
          case Hazard.CLAW:
            AddVerticalDangerBoard(hazardContainer.HazardSpawnPoint, hazardContainer.CombinedDelay + clawTime);
            break;
          case Hazard.WASABI:
            AddVerticalDangerBoard(hazardContainer.HazardSpawnPoint, hazardContainer.CombinedDelay + laserTime);
            break;
        }
      }
      else
      {
        switch (hazardContainer.HazardToSpawn)
        {
          case Hazard.SUSHI:
            AddHorizontalDangerBoard(hazardContainer.HazardSpawnPoint, hazardContainer.CombinedDelay + boulderTime);
            break;
          case Hazard.CLAW:
            AddHorizontalDangerBoard(hazardContainer.HazardSpawnPoint, hazardContainer.CombinedDelay + clawTime);
            break;
          case Hazard.WASABI:
            AddHorizontalDangerBoard(hazardContainer.HazardSpawnPoint, hazardContainer.CombinedDelay + laserTime);
            break;
        }
      }
      yield return new WaitForSeconds(hazardContainer.PreIndicatorDelay);
      indicatorHandler.AcitvateIndicator(hazardContainer.HazardToSpawn, hazardContainer.BorderSet, hazardContainer.HazardSpawnPoint.x, hazardContainer.IndicatorDelay);
    }
  }
}
