using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Determines when to spawn Hazards
/// </summary>
public class HazardSpawner : MonoBehaviour
{
  [SerializeField]
  DifficultyAI diffAI;
  [SerializeField]
  PositioningAI posAI;

  public void GetHazards(Vector2Int playerPos, bool[,] boolDangerBoard, out List<BoardManager.Hazard> hazardTypes, out int hazards, out float indicatorDelay, out List<int> rows, out List<int> cols)
  {
    diffAI.Difficulty(out hazardTypes, out hazards, out indicatorDelay);
    posAI.Location(playerPos, boolDangerBoard, out rows, out cols);
  }
}
