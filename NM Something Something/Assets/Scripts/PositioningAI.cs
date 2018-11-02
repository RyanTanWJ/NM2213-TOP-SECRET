using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finds the amount of space that the player can move
/// Determines the rows or columns that hazards can spawn on the player
/// </summary>
public class PositioningAI : MonoBehaviour
{
  private List<int> retRows = new List<int>();
  private List<int> retCols = new List<int>();
  List<Vector2Int> retConnectedComponent = new List<Vector2Int>();

  private bool[,] visited;

  public void Location(Vector2Int playerPos, bool[,] dangerBoard, out List<Vector2Int> connectedComponent, out List<int> rows, out List<int> cols)
  {
    UpdateRetRowsAndCols(playerPos, dangerBoard);

    retRows.Sort();
    retCols.Sort();

    rows = retRows;
    cols = retCols;
    connectedComponent = retConnectedComponent;
  }

  private void UpdateRetRowsAndCols(Vector2Int playerPos, bool[,] dangerBoard)
  {
    retRows = new List<int>();
    retCols = new List<int>();

    visited = new bool[dangerBoard.GetLength(0), dangerBoard.GetLength(1)];

    Vector2Int playerClosestEmptySpace = FindClosestEmptySpace(playerPos, dangerBoard, visited, playerPos);

    retConnectedComponent = FindConnectedComponent(playerClosestEmptySpace, dangerBoard);

    foreach (Vector2Int connectedTile in retConnectedComponent)
    {
      if (!retRows.Contains(connectedTile.x))
      {
        retRows.Add(connectedTile.x);
      }
      if (!retCols.Contains(connectedTile.y))
      {
        retCols.Add(connectedTile.y);
      }
    }
  }

  //___NEEDS___
  //________SANITY________
  //______________CHECK______________
  private Vector2Int FindClosestEmptySpace(Vector2Int recurPlayerPos, bool[,] dangerBoard, bool[,] visited, Vector2Int refPlayerPos)
  {
    if (!dangerBoard[recurPlayerPos.x, recurPlayerPos.y])
    {
      return recurPlayerPos;
    }

    visited[recurPlayerPos.x, recurPlayerPos.y] = true;

    List<Vector2Int> possiblePositions = new List<Vector2Int>();

    if (recurPlayerPos.y < dangerBoard.GetLength(1) - 1 && !visited[recurPlayerPos.x, recurPlayerPos.y + 1])
    {
      FindPossiblePositionsUp(recurPlayerPos, dangerBoard, visited, refPlayerPos, possiblePositions);
    }
    if (recurPlayerPos.y > 1 && !visited[recurPlayerPos.x, recurPlayerPos.y - 1])
    {
      FindPossiblePositionsDown(recurPlayerPos, dangerBoard, visited, refPlayerPos, possiblePositions);
    }
    if (recurPlayerPos.x < dangerBoard.GetLength(0) - 1 && !visited[recurPlayerPos.x + 1, recurPlayerPos.y])
    {
      FindPossiblePositionsRight(recurPlayerPos, dangerBoard, visited, refPlayerPos, possiblePositions);
    }
    if (recurPlayerPos.x > 1 && !visited[recurPlayerPos.x - 1, recurPlayerPos.y])
    {
      FindPossiblePositionsLeft(recurPlayerPos, dangerBoard, visited, refPlayerPos, possiblePositions);
    }
    return possiblePositions[UnityEngine.Random.Range(0, possiblePositions.Count)];
  }

  private void FindPossiblePositionsUp(Vector2Int recurPlayerPos, bool[,] dangerBoard, bool[,] visited, Vector2Int refPlayerPos, List<Vector2Int> possiblePositions)
  {
    if (possiblePositions.Count > 0)
    {
      if (NewPosLessThanCurrPos(recurPlayerPos, refPlayerPos, possiblePositions))
      {
        possiblePositions.Clear();
        possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x, recurPlayerPos.y + 1), dangerBoard, visited, refPlayerPos));
      }
      else if (NewPosEqualToCurrPos(recurPlayerPos, refPlayerPos, possiblePositions))
      {
        possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x, recurPlayerPos.y + 1), dangerBoard, visited, refPlayerPos));
      }
    }
    else
    {
      possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x, recurPlayerPos.y + 1), dangerBoard, visited, refPlayerPos));
    }
  }

  private void FindPossiblePositionsDown(Vector2Int recurPlayerPos, bool[,] dangerBoard, bool[,] visited, Vector2Int refPlayerPos, List<Vector2Int> possiblePositions)
  {
    if (possiblePositions.Count > 0)
    {
      if (NewPosLessThanCurrPos(recurPlayerPos, refPlayerPos, possiblePositions))
      {
        possiblePositions.Clear();
        possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x, recurPlayerPos.y - 1), dangerBoard, visited, refPlayerPos));
      }
      else if (NewPosEqualToCurrPos(recurPlayerPos, refPlayerPos, possiblePositions))
      {
        possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x, recurPlayerPos.y - 1), dangerBoard, visited, refPlayerPos));
      }
    }
    else
    {
      possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x, recurPlayerPos.y - 1), dangerBoard, visited, refPlayerPos));
    }
  }

  private void FindPossiblePositionsRight(Vector2Int recurPlayerPos, bool[,] dangerBoard, bool[,] visited, Vector2Int refPlayerPos, List<Vector2Int> possiblePositions)
  {
    if (possiblePositions.Count > 0)
    {
      if (NewPosLessThanCurrPos(recurPlayerPos, refPlayerPos, possiblePositions))
      {
        possiblePositions.Clear();
        possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x + 1, recurPlayerPos.y), dangerBoard, visited, refPlayerPos));
      }
      else if (NewPosEqualToCurrPos(recurPlayerPos, refPlayerPos, possiblePositions))
      {
        possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x + 1, recurPlayerPos.y), dangerBoard, visited, refPlayerPos));
      }
    }
    else
    {
      possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x + 1, recurPlayerPos.y), dangerBoard, visited, refPlayerPos));
    }
  }

  private void FindPossiblePositionsLeft(Vector2Int recurPlayerPos, bool[,] dangerBoard, bool[,] visited, Vector2Int refPlayerPos, List<Vector2Int> possiblePositions)
  {
    if (possiblePositions.Count > 0)
    {
      if (NewPosLessThanCurrPos(recurPlayerPos, refPlayerPos, possiblePositions))
      {
        possiblePositions.Clear();
        possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x - 1, recurPlayerPos.y), dangerBoard, visited, refPlayerPos));
      }
      else if (NewPosEqualToCurrPos(recurPlayerPos, refPlayerPos, possiblePositions))
      {
        possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x - 1, recurPlayerPos.y), dangerBoard, visited, refPlayerPos));
      }
    }
    else
    {
      possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x - 1, recurPlayerPos.y), dangerBoard, visited, refPlayerPos));
    }
  }

  private static bool NewPosLessThanCurrPos(Vector2Int recurPlayerPos, Vector2Int refPlayerPos, List<Vector2Int> possiblePositions)
  {
    return (recurPlayerPos - refPlayerPos).sqrMagnitude < (possiblePositions[0] - refPlayerPos).sqrMagnitude;
  }

  private static bool NewPosEqualToCurrPos(Vector2Int recurPlayerPos, Vector2Int refPlayerPos, List<Vector2Int> possiblePositions)
  {
    return (recurPlayerPos - refPlayerPos).sqrMagnitude == (possiblePositions[0] - refPlayerPos).sqrMagnitude;
  }

  private List<Vector2Int> FindConnectedComponent(Vector2Int closestEmptySpace, bool[,] dangerBoard)
  {
    List<Vector2Int> connectedComponent = new List<Vector2Int>();

    dangerBoard[closestEmptySpace.x, closestEmptySpace.y] = true;

    //THIS LINE IS IMPORTANT TO ENSURE THAT THE FIRST ELEMENT OF
    //THE RETURNED LIST IS THE (Expected) PLAYER POSITION
    //DO NOT REMOVE THIS COMMENT OR THIS LINE
    connectedComponent.Add(closestEmptySpace);

    List<Vector2Int> recurList;

    if (closestEmptySpace.y < dangerBoard.GetLength(1) - 2 && !dangerBoard[closestEmptySpace.x, closestEmptySpace.y + 1])
    {
      recurList = FindConnectedComponent(new Vector2Int(closestEmptySpace.x, closestEmptySpace.y + 1), dangerBoard);
      connectedComponent.AddRange(recurList);
    }

    if (closestEmptySpace.y > 1 && !dangerBoard[closestEmptySpace.x, closestEmptySpace.y - 1])
    {
      recurList = FindConnectedComponent(new Vector2Int(closestEmptySpace.x, closestEmptySpace.y - 1), dangerBoard);
      connectedComponent.AddRange(recurList);
    }

    if (closestEmptySpace.x < dangerBoard.GetLength(0) - 2 && !dangerBoard[closestEmptySpace.x + 1, closestEmptySpace.y])
    {
      recurList = FindConnectedComponent(new Vector2Int(closestEmptySpace.x + 1, closestEmptySpace.y), dangerBoard);
      connectedComponent.AddRange(recurList);
    }

    if (closestEmptySpace.x > 1 && !dangerBoard[closestEmptySpace.x - 1, closestEmptySpace.y])
    {
      recurList = FindConnectedComponent(new Vector2Int(closestEmptySpace.x - 1, closestEmptySpace.y), dangerBoard);
      connectedComponent.AddRange(recurList);
    }
    return connectedComponent;
  }
}
