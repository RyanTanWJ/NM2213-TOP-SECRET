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

  public void GetHazards()
  {
    int hazards;
    float indicatorDelay;
    diffAI.Difficulty(out hazards, out indicatorDelay);
    List<int> rows;
    List<int> cols;
    posAI.Location(out rows, out cols);

  }
}
