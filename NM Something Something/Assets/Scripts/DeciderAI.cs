using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Processes information from DifficultyAI and PositioningAI to determine what hazard to spawn where.
/// </summary>
public class DeciderAI : MonoBehaviour
{
  public List<HazardContainer> Decide(List<BoardManager.Hazard> hazardTypes, int hazards, float indicatorDelay, List<Vector2Int> connectedComponent, List<int> rows, List<int> cols)
  {
    List<HazardContainer> retVal = new List<HazardContainer>();

    //Player cannot move
    if (connectedComponent.Count <= 1 || (rows.Count == 1 && cols.Count == 1))
    {
      return retVal;
    }

    /*
    //Choose a Hazard
    BoardManager.Hazard hazardToSpawn = hazardTypes[UnityEngine.Random.Range(0, hazardTypes.Count)];

    //Get the player's "best position"
    Vector2Int hazardSpawnPoint = connectedComponent[0];

    //Assign the border set
    BoardManager.BorderSet borderSet;

    if (hazardToSpawn == BoardManager.Hazard.PUFFERFISH)
    {
      borderSet = BoardManager.BorderSet.TOP;
      connectedComponent = SpaceLeftToMove(connectedComponent, hazardSpawnPoint);
    }
    else
    {
      //Fairness Check
      if (rows.Count == 1)
      {
        borderSet = UnityEngine.Random.Range(0, 1.0f) < 0.5 ? BoardManager.BorderSet.TOP : BoardManager.BorderSet.BOT;
        cols.Remove(hazardSpawnPoint.y);
        connectedComponent = SpaceLeftToMove(connectedComponent, borderSet, hazardSpawnPoint.y);
      }
      //(rows.Count==1 && cols.Count==1) should be caught above
      else if (cols.Count == 1)
      {
        borderSet = UnityEngine.Random.Range(0, 1.0f) < 0.5 ? BoardManager.BorderSet.LEFT : BoardManager.BorderSet.RIGHT;
        rows.Remove(hazardSpawnPoint.x);
        connectedComponent = SpaceLeftToMove(connectedComponent, borderSet, hazardSpawnPoint.x);
      }
      else
      {
        BoardManager.BorderSet[] borderSetList = { BoardManager.BorderSet.LEFT, BoardManager.BorderSet.RIGHT, BoardManager.BorderSet.TOP, BoardManager.BorderSet.BOT };
        borderSet = borderSetList[UnityEngine.Random.Range(0, borderSetList.Length)];

        if (borderSet == BoardManager.BorderSet.TOP || borderSet == BoardManager.BorderSet.BOT)
        {
          cols.Remove(hazardSpawnPoint.y);
          connectedComponent = SpaceLeftToMove(connectedComponent, borderSet, hazardSpawnPoint.y);
        }
        else
        {
          rows.Remove(hazardSpawnPoint.x);
          connectedComponent = SpaceLeftToMove(connectedComponent, borderSet, hazardSpawnPoint.x);
        }
      }
    }

    //First Hazard should never have any delay
    float preIndicatorDelay = 0;

    HazardContainer hazardContainer =
      new HazardContainer(hazardSpawnPoint, hazardToSpawn, borderSet, preIndicatorDelay, indicatorDelay);

    retVal.Add(hazardContainer);

    int hazardsLeft = hazards - 1;//-1 for the first hazard
    */

    BoardManager.Hazard hazardToSpawn;
    Vector2Int hazardSpawnPoint;
    BoardManager.BorderSet borderSet;
    float preIndicatorDelay;
    HazardContainer hazardContainer;
    bool playerTargetted = false;

    //Terminate if spawn enough hazards, no more space for player to move, 
    while (hazards > 0 && connectedComponent.Count > 1 && (rows.Count > 0 || cols.Count > 0))
    {
      //Choose a Hazard
      hazardToSpawn = hazardTypes[UnityEngine.Random.Range(0, hazardTypes.Count)];

      //Get the player's "best position" if player has not yet been targetted
      hazardSpawnPoint = playerTargetted ? new Vector2Int(rows[UnityEngine.Random.Range(0, rows.Count)], cols[UnityEngine.Random.Range(0, cols.Count)]) : connectedComponent[0];

      //Fairness Check
      List<Vector2Int> testTheWater;

      //Assign the border set
      if (hazardToSpawn == BoardManager.Hazard.PUFFERFISH)
      {
        borderSet = BoardManager.BorderSet.TOP; //This can be anything it doesn't matter for pufferfish
        testTheWater = SpaceLeftToMove(connectedComponent, hazardSpawnPoint);
        if (testTheWater.Count <= 0)
        {
          continue;
        }
        connectedComponent = testTheWater;
        playerTargetted = true;
      }
      else
      {
        //Choose a random border set to spawn from
        BoardManager.BorderSet[] borderSetList = { BoardManager.BorderSet.LEFT, BoardManager.BorderSet.RIGHT, BoardManager.BorderSet.TOP, BoardManager.BorderSet.BOT };
        if (rows.Count == 0)
        {
          borderSet = borderSetList[UnityEngine.Random.Range(2, borderSetList.Length)];
        }
        else if (cols.Count == 0)
        {
          borderSet = borderSetList[UnityEngine.Random.Range(0, (borderSetList.Length / 2))];
        }
        else
        {
          borderSet = borderSetList[UnityEngine.Random.Range(0, borderSetList.Length)];
        }

        if (borderSet == BoardManager.BorderSet.TOP || borderSet == BoardManager.BorderSet.BOT)
        {
          cols.Remove(hazardSpawnPoint.y);
          testTheWater = SpaceLeftToMove(connectedComponent, borderSet, hazardSpawnPoint.y);
          if (testTheWater.Count <= 0)
          {
            continue;
          }
          connectedComponent = testTheWater;
          playerTargetted = true;
        }
        else
        {
          rows.Remove(hazardSpawnPoint.x);
          testTheWater = SpaceLeftToMove(connectedComponent, borderSet, hazardSpawnPoint.x);
          if (testTheWater.Count <= 0)
          {
            continue;
          }
          connectedComponent = testTheWater;
          playerTargetted = true;
        }
      }

      //Randomise Delay
      preIndicatorDelay = UnityEngine.Random.Range(0, 0.2f);

      hazardContainer =
        new HazardContainer(hazardSpawnPoint, hazardToSpawn,
        borderSet, preIndicatorDelay, indicatorDelay);

      retVal.Add(hazardContainer);

      hazards--;
    }

    return retVal;
  }

  private List<Vector2Int> SpaceLeftToMove(List<Vector2Int> SpaceToMove, BoardManager.BorderSet borderSet, int idx)
  {
    List<Vector2Int> spaceLeft = new List<Vector2Int>();
    if (borderSet == BoardManager.BorderSet.LEFT || borderSet == BoardManager.BorderSet.RIGHT)
    {
      foreach (Vector2Int space in SpaceToMove)
      {
        if (space.x != idx)
        {
          spaceLeft.Add(space);
        }
      }
    }
    else
    {
      foreach (Vector2Int space in SpaceToMove)
      {
        if (space.y != idx)
        {
          spaceLeft.Add(space);
        }
      }
    }

    return spaceLeft;
  }

  private List<Vector2Int> SpaceLeftToMove(List<Vector2Int> SpaceToMove, Vector2Int PufferPos)
  {
    List<Vector2Int> spaceLeft = new List<Vector2Int>();

    foreach (Vector2Int space in SpaceToMove)
    {
      //Is this O(1)? (-.-")
      if (space != PufferPos
        && space != new Vector2Int(PufferPos.x + 1, PufferPos.y)
        && space != new Vector2Int(PufferPos.x - 1, PufferPos.y)
        && space != new Vector2Int(PufferPos.x, PufferPos.y + 1)
        && space != new Vector2Int(PufferPos.x, PufferPos.y - 1)
        && space != new Vector2Int(PufferPos.x + 1, PufferPos.y - 1)
        && space != new Vector2Int(PufferPos.x - 1, PufferPos.y + 1)
        && space != new Vector2Int(PufferPos.x + 1, PufferPos.y + 1)
        && space != new Vector2Int(PufferPos.x - 1, PufferPos.y - 1)
        )
      {
        spaceLeft.Add(space);
      }
    }

    return spaceLeft;
  }
}
