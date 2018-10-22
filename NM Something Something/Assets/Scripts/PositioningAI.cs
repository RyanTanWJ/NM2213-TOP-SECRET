﻿using System;
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

  //public void Location(Vector2Int playerLocations, out List<int> rows, out List<int> cols)
  public void Location(Vector2Int playerPos, bool[,] dangerBoard, out List<int> rows, out List<int> cols)
  {
    UpdateRetRowsAndCols(playerPos, dangerBoard);

    rows = retRows;
    cols = retCols;

  }

  private void UpdateRetRowsAndCols(Vector2Int playerPos, bool[,] dangerBoard)
  {
    retRows = new List<int>();
    retCols = new List<int>();

    Vector2Int playerClosestEmptySpace = FindClosestEmptySpace(playerPos, dangerBoard);
    List<Vector2Int> connectedComponent = FindConnectedComponent(playerClosestEmptySpace, dangerBoard);
    foreach(Vector2Int connectedTile in connectedComponent)
    {
      if (!retRows.Contains(connectedTile.x))
      {
        retRows.Add(connectedTile.x);
      }
      if (!retCols.Contains(connectedTile.y))
      {
        retRows.Add(connectedTile.y);
      }
    }
  }

  private Vector2Int FindClosestEmptySpace(Vector2Int playerPos, bool[,] dangerBoard)
  {
    if (!dangerBoard[playerPos.x, playerPos.y])
    {
      return playerPos;
    }
    List<Vector2Int> possiblePositions = new List<Vector2Int>();
    if (playerPos.y < dangerBoard.GetLength(1) - 1)
    {
      possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(playerPos.x, playerPos.y + 1), dangerBoard));
    }
    if (playerPos.y > 1)
    {
      possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(playerPos.x, playerPos.y - 1), dangerBoard));
    }
    if (playerPos.x < dangerBoard.GetLength(0) - 1)
    {
      possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(playerPos.x + 1, playerPos.y), dangerBoard));
    }
    if (playerPos.x > 1)
    {
      possiblePositions.Add(FindClosestEmptySpace(new Vector2Int(playerPos.x - 1, playerPos.y), dangerBoard));
    }
    return possiblePositions[UnityEngine.Random.Range(0, possiblePositions.Count)];
  }

  private List<Vector2Int> FindConnectedComponent(Vector2Int closestEmptySpace, bool[,] dangerBoard)
  {
    List<Vector2Int> connectedComponent = new List<Vector2Int>();
    connectedComponent.Add(closestEmptySpace);

    Debug.Log(closestEmptySpace);

    List<Vector2Int> recurList;

    if (closestEmptySpace.y < dangerBoard.GetLength(1) - 1 && !dangerBoard[closestEmptySpace.x, closestEmptySpace.y + 1])
    {
      recurList = FindConnectedComponent(new Vector2Int(closestEmptySpace.x, closestEmptySpace.y + 1), dangerBoard);
      foreach (Vector2Int connectedTile in recurList)
      {
        if (!connectedComponent.Contains(connectedTile))
        {
          connectedComponent.Add(connectedTile);
        }
      }
    }

    if (closestEmptySpace.y > 1 && !dangerBoard[closestEmptySpace.x, closestEmptySpace.y - 1] && !connectedComponent.Contains(new Vector2Int(closestEmptySpace.x, closestEmptySpace.y - 1)))
    {
      recurList = FindConnectedComponent(new Vector2Int(closestEmptySpace.x, closestEmptySpace.y - 1), dangerBoard);
      foreach (Vector2Int connectedTile in recurList)
      {
        if (!connectedComponent.Contains(connectedTile))
        {
          connectedComponent.Add(connectedTile);
        }
      }
    }

    if (closestEmptySpace.x < dangerBoard.GetLength(0) - 1 && !dangerBoard[closestEmptySpace.x + 1, closestEmptySpace.y] && !connectedComponent.Contains(new Vector2Int(closestEmptySpace.x + 1, closestEmptySpace.y)))
    {
      recurList = FindConnectedComponent(new Vector2Int(closestEmptySpace.x + 1, closestEmptySpace.y), dangerBoard);
      foreach (Vector2Int connectedTile in recurList)
      {
        if (!connectedComponent.Contains(connectedTile))
        {
          connectedComponent.Add(connectedTile);
        }
      }
    }

    if (closestEmptySpace.x > 1 && !dangerBoard[closestEmptySpace.x - 1, closestEmptySpace.y] && !connectedComponent.Contains(new Vector2Int(closestEmptySpace.x - 1, closestEmptySpace.y)))
    {
      recurList = FindConnectedComponent(new Vector2Int(closestEmptySpace.x - 1, closestEmptySpace.y), dangerBoard);
      foreach (Vector2Int connectedTile in recurList)
      {
        if (!connectedComponent.Contains(connectedTile))
        {
          connectedComponent.Add(connectedTile);
        }
      }
    }
    return connectedComponent;
  }
}
