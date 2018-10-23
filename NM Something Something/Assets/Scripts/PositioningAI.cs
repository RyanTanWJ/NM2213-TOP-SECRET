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

  private bool[,] visited;

  //public void Location(Vector2Int playerLocations, out List<int> rows, out List<int> cols)
  public void Location(Vector2Int playerPos, bool[,] dangerBoard, out List<int> rows, out List<int> cols)
  {
    UpdateRetRowsAndCols(playerPos, dangerBoard);

    retRows.Sort();
    retCols.Sort();

    rows = retRows;
    cols = retCols;
  }

  private void UpdateRetRowsAndCols(Vector2Int playerPos, bool[,] dangerBoard)
  {
    retRows = new List<int>();
    retCols = new List<int>();

    visited = new bool[dangerBoard.GetLength(0), dangerBoard.GetLength(1)];

    Vector2Int playerClosestEmptySpace = FindClosestEmptySpace(playerPos, dangerBoard, visited);

    List<Vector2Int> connectedComponent = FindConnectedComponent(playerClosestEmptySpace, dangerBoard);

    foreach (Vector2Int connectedTile in connectedComponent)
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

  private Vector2Int FindClosestEmptySpace(Vector2Int playerPos, bool[,] dangerBoard, bool[,] visited)
  {
    if (!dangerBoard[playerPos.x, playerPos.y])
    {
      return playerPos;
    }

    visited[playerPos.x, playerPos.y] = true;

    List<Vector2Int> possiblePositions = new List<Vector2Int>();

    if (playerPos.y < dangerBoard.GetLength(1) - 1 && !visited[playerPos.x, playerPos.y + 1])
    {
      possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(playerPos.x, playerPos.y + 1), dangerBoard, visited));
    }
    if (playerPos.y > 1 && !visited[playerPos.x, playerPos.y - 1])
    {
      possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(playerPos.x, playerPos.y - 1), dangerBoard, visited));
    }
    if (playerPos.x < dangerBoard.GetLength(0) - 1 && !visited[playerPos.x + 1, playerPos.y])
    {
      possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(playerPos.x + 1, playerPos.y), dangerBoard, visited));
    }
    if (playerPos.x > 1 && !visited[playerPos.x - 1, playerPos.y])
    {
      possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(playerPos.x - 1, playerPos.y), dangerBoard, visited));
    }
    return possiblePositions[UnityEngine.Random.Range(0, possiblePositions.Count)];
  }

  private List<Vector2Int> FindConnectedComponent(Vector2Int closestEmptySpace, bool[,] dangerBoard)
  {
    List<Vector2Int> connectedComponent = new List<Vector2Int>();

    dangerBoard[closestEmptySpace.x, closestEmptySpace.y] = true;

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
