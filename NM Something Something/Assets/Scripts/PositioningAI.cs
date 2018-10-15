using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finds the amount of space that the player can move
/// Determines the rows or columns that hazards can spawn on the player
/// </summary>
public class PositioningAI : MonoBehaviour
{
  //Need a way in boardmanager to transform from world space to grid space
  //Function should take current position of all hazards in grid space
  //Find connected component that player is in and decide where to spawn from there

  //public void Location(Vector2Int playerLocations, out List<int> rows, out List<int> cols)
  public void Location(out List<int> rows, out List<int> cols)
  {
    List<int> retRows = new List<int>();
    List<int> retCols = new List<int>();
    for (int i=1; i<7; i++)
    {
      retRows.Add(i);
      retCols.Add(i);
    }
    rows = retRows;
    cols = retCols;
  }
}
