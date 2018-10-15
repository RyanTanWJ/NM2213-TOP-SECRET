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

  public void GetHazards(out int hazards, out float indicatorDelay, out List<int> rows, out List<int> cols)
  {
    diffAI.Difficulty(out hazards, out indicatorDelay);
    Debug.Log("number of hazards: " + hazards);
    Debug.Log("number of indicatorDelay: " + indicatorDelay);
    posAI.Location(out rows, out cols);
  }
}
