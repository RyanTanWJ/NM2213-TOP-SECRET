using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Determines when and where to spawn Hazards
/// </summary>
public class HazardSpawner : MonoBehaviour
{
  [SerializeField]
  DifficultyAI diffAI;
  [SerializeField]
  PositioningAI posAI;
  [SerializeField]
  DeciderAI deciderAI;

  private const float minSpawnDelay = 0;
  private const float maxSpawnDelay = 0.2f;

  public List<HazardContainer> GetHazards(Vector2Int playerPos, bool[,] boolDangerBoard)
  {
    List<BoardManager.Hazard> hazardTypes;
    int hazards;
    float indicatorDelay;
    List<Vector2Int> connectedComponent;
    List<int> rows;
    List<int> cols;

    diffAI.Difficulty(out hazardTypes, out hazards, out indicatorDelay);
    posAI.Location(playerPos, boolDangerBoard, out connectedComponent, out rows, out cols);

    List<HazardContainer> hazardContainers = deciderAI.Decide(hazardTypes, hazards, indicatorDelay, connectedComponent, rows, cols);
    return hazardContainers;
  }
}



