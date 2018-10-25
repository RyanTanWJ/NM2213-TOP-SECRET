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
  
  private const float minSpawnDelay = 0;
  private const float maxSpawnDelay = 0.2f;

  public void GetHazards(Vector2Int playerPos, bool[,] boolDangerBoard, out List<BoardManager.Hazard> hazardTypes, out int hazards, out float indicatorDelay, out List<int> rows, out List<int> cols)
  {
    diffAI.Difficulty(out hazardTypes, out hazards, out indicatorDelay);
    posAI.Location(playerPos, boolDangerBoard, out rows, out cols);
    //spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
  }
}
