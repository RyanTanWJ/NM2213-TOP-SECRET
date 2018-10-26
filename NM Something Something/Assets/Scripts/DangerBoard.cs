using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerBoard {

  float[,] dangerBoard;

  public DangerBoard(int row, int col)
  {
    dangerBoard = new float[row, col];
    PopulateDangerBoard();
  }

  private void PopulateDangerBoard()
  {
    for (int i=0; i<dangerBoard.GetLength(0); i++)
    {
      for (int j = 0; j < dangerBoard.GetLength(1); j++)
      {
        dangerBoard[i, j] = 0;
      }
    }
  }

  public bool[,] GetDangerBoard()
  {
    bool[,] boolBoard = new bool[dangerBoard.GetLength(0), dangerBoard.GetLength(1)];
    for (int i = 0; i < dangerBoard.GetLength(0); i++)
    {
      for (int j = 0; j < dangerBoard.GetLength(1); j++)
      {
        if (dangerBoard[i, j] > 0)
        {
          boolBoard[i, j] = true;
        }
        else
        {
          boolBoard[i, j] = false;
        }
      }
    }
    return boolBoard;
  }

  public void AddDangerBoard(Vector2Int dangerPos, float dangerTimer)
  {
    dangerBoard[dangerPos.x, dangerPos.y] = Mathf.Max(dangerTimer, dangerBoard[dangerPos.x, dangerPos.y]);
  }

  public void UpdateDangerBoard(float timePerFrame)
  {
    for (int i = 0; i < dangerBoard.GetLength(0); i++)
    {
      for (int j = 0; j < dangerBoard.GetLength(1); j++)
      {
        if (dangerBoard[i, j] > 0)
        {
          if (timePerFrame >= dangerBoard[i, j])
          {
            dangerBoard[i, j] = 0;
            continue;
          }
          dangerBoard[i, j] -= timePerFrame;
        }
      }
    }
  }

  public void Print()
  {
    Debug.Log("Printing Danger Board:");
    for (int i=0; i < dangerBoard.GetLength(0); i++)
    {
      string row = "";
      for (int j=0; j< dangerBoard.GetLength(1); j++)
      {
        row += " " + (dangerBoard[i,j]>0);
      }
      Debug.Log("Row " + i + ": " + row);
    }
  }
}
