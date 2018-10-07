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

  List<GameObject> TopIndicators = new List<GameObject>();
  List<GameObject> BotIndicators = new List<GameObject>();
  List<GameObject> LeftIndicators = new List<GameObject>();
  List<GameObject> RightIndicators = new List<GameObject>();

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

  List<Boulder> hazards = new List<Boulder>();

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
    GameObject indicator;
    Vector3 tilePosition;
    for (int i = 0; i < maxRows; i++)
    {
      indicator = Instantiate(arrowIndicator, platforms.transform);
      tilePosition = GetGridPosition(i, 0);
      indicator.transform.position = tilePosition;
      LeftIndicators.Add(indicator);
      //indicator.SetActive(false);

      indicator = Instantiate(arrowIndicator, platforms.transform);
      tilePosition = GetGridPosition(i, maxCols - 1);
      indicator.transform.position = tilePosition;
      RightIndicators.Add(indicator);
      indicator.transform.eulerAngles = new Vector3(0, 0, 180);
      //indicator.SetActive(false);
    }
    for (int j = 0; j < maxCols; j++)
    {
      indicator = Instantiate(arrowIndicator, platforms.transform);
      tilePosition = GetGridPosition(0, j);
      indicator.transform.position = tilePosition;
      BotIndicators.Add(indicator);
      indicator.transform.eulerAngles = new Vector3(0, 0, 90);
      //indicator.SetActive(false);

      indicator = Instantiate(arrowIndicator, platforms.transform);
      tilePosition = GetGridPosition(maxRows - 1, j);
      indicator.transform.position = tilePosition;
      TopIndicators.Add(indicator);
      indicator.transform.eulerAngles = new Vector3(0, 0, -90);
      //indicator.SetActive(false);
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
        direction.x = 0.01f;
        break;
      case Direction.DOWN:
        direction.x = -0.01f;
        break;
      case Direction.LEFT:
        direction.y = 0.01f;
        break;
      case Direction.RIGHT:
        direction.y = -0.01f;
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

    /*
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
    */
    while (boulder.ShouldMove)
    {
      gameObj.transform.position = gameObj.transform.position + direction * speed;
      yield return null;
    }
  }

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
