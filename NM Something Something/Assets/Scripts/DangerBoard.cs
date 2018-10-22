using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerBoard {

  List<float>[,] dangerBoard;

  public DangerBoard(int row, int col)
  {
    dangerBoard = new List<float>[row, col];
    PopulateDangerBoard();
  }

  public void PopulateDangerBoard()
  {
    for (int i=0; i<dangerBoard.GetLength(0); i++)
    {
      for (int j = 0; j < dangerBoard.GetLength(1); j++)
      {
        dangerBoard[i, j] = new List<float>();
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
        if (dangerBoard[i, j].Count > 0)
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

  public void UpdateDangerBoard(float timePerFrame)
  {
    for (int i = 0; i < dangerBoard.GetLength(0); i++)
    {
      for (int j = 0; j < dangerBoard.GetLength(1); j++)
      {
        if (dangerBoard[i, j].Count > 0)
        {
          if (timePerFrame> dangerBoard[i, j][0])
          {
            dangerBoard[i, j].RemoveAt(0);
            continue;
          }
          dangerBoard[i, j][0] -= timePerFrame;
        }
      }
    }
  }

  public void Print()
  {
    Debug.Log("Printing Danger Board:");
    foreach (List<float> dangerTimer in dangerBoard)
    {
      Debug.Log(dangerTimer);
    }
  }
}
