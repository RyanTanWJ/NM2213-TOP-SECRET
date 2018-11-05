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
  private List<int> retRows;
  private List<int> retCols;
  List<Vector2Int> retConnectedComponent;

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

    Vector2Int playerClosestEmptySpace = FindClosestEmptySpace(playerPos, dangerBoard);

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
  private Vector2Int FindClosestEmptySpace(Vector2Int recurPlayerPos, bool[,] dangerBoard)
  {
    if (!dangerBoard[recurPlayerPos.x, recurPlayerPos.y])
    {
      return recurPlayerPos;
    }

    List<Vector2Int> closestTiles = new List<Vector2Int>();
    for (int i=0; i < dangerBoard.GetLength(0); i++)
    {
      for (int j=0; j<dangerBoard.GetLength(1); j++)
      {
        //Only check if not a danger zone
        if (!dangerBoard[i, j])
        {
          Vector2Int tileInQuestion = new Vector2Int(i,j);
          if (closestTiles.Count > 0)
          {
            //if result is closer then replace
            if ((closestTiles[0] - recurPlayerPos).sqrMagnitude > (tileInQuestion - recurPlayerPos).sqrMagnitude)
            {
              closestTiles.Clear();
              closestTiles.Add(tileInQuestion);
            }
            //if result is equal then add to
            else if ((closestTiles[0] - recurPlayerPos).sqrMagnitude == (tileInQuestion - recurPlayerPos).sqrMagnitude)
            {
              closestTiles.Add(tileInQuestion);
            }
          }
          else
          {
            closestTiles.Add(tileInQuestion);
          }
        }
      }
    }
    return closestTiles[UnityEngine.Random.Range(0, closestTiles.Count)];
    /*
    if (!dangerBoard[recurPlayerPos.x, recurPlayerPos.y])
    {
      return recurPlayerPos;
    }

    visited[recurPlayerPos.x, recurPlayerPos.y] = true;

    List<Vector2Int> possiblePositions = new List<Vector2Int>();

    Vector2Int closest;

    // -2 to ignore board border
    if (recurPlayerPos.y < dangerBoard.GetLength(1) - 2 && !visited[recurPlayerPos.x, recurPlayerPos.y + 1])
    {
      closest = FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x, recurPlayerPos.y + 1), dangerBoard, visited, refPlayerPos);
      if (closest != nullAlternative)
      {
        possiblePositions.Add(closest);
      }
    }

    // 1 to ignore board border
    if (recurPlayerPos.y > 1 && !visited[recurPlayerPos.x, recurPlayerPos.y - 1])
    {
      closest = FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x, recurPlayerPos.y - 1), dangerBoard, visited, refPlayerPos);
      if (closest != nullAlternative)
      {
        possiblePositions.Add(closest);
      }
    }

    if (recurPlayerPos.x < dangerBoard.GetLength(0) - 2 && !visited[recurPlayerPos.x + 1, recurPlayerPos.y])
    {
      closest = FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x + 1, recurPlayerPos.y), dangerBoard, visited, refPlayerPos);
      if (closest != nullAlternative)
      {
        possiblePositions.Add(closest);
      }
    }

    if (recurPlayerPos.x > 1 && !visited[recurPlayerPos.x - 1, recurPlayerPos.y])
    {
      closest = FindClosestEmptySpace(new Vector2Int(recurPlayerPos.x - 1, recurPlayerPos.y), dangerBoard, visited, refPlayerPos);
      if (closest != nullAlternative)
      {
        possiblePositions.Add(closest);
      }
    }

    if (possiblePositions.Count > 0)
    {
      //Debug.Log("______Printing Possible Positions:______");
      //foreach (Vector2Int v in possiblePositions)
      //{
      //  Debug.Log("Current player position is: " + refPlayerPos.ToString() + ". Comparing to position: " + v.ToString() + ". sqrMagnitude = " + (refPlayerPos - v).sqrMagnitude);
      //}
      return possiblePositions[UnityEngine.Random.Range(0, possiblePositions.Count)];
    }
    else
    {
      //Debug.Log("_________Possible Positions is EMPTY._________");
      return nullAlternative;
    }
    */
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
