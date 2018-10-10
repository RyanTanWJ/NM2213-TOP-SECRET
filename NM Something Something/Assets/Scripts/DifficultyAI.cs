using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyAI : MonoBehaviour {

  //Upper limit for the number of hazards that can be spawned
  private readonly int maxHazards = 10;

  //Lower limit for the delay before indicator switches off and hazard spawns, currently set to average human reaction time
  private readonly float minIndicatorDelay = 0.02f;

  //Original delay before indicator switches off and hazard spawns
  private const float OGIndicatorDelay = 1.0f;

  //Max Number of Hazards that should be spawned
  private int numberOfHazards = 1;

  //Delay before indicator switches off and hazard spawns
  private float indicatorDelay = OGIndicatorDelay;

  //TODO: Way to decide between different hazards

  private void OnEnable()
  {
    InvisibleWall.MakeGameHarderEvent += MakeGameHarder;
  }

  private void OnDisable()
  {
    InvisibleWall.MakeGameHarderEvent -= MakeGameHarder;
  }

  /// <summary>
  /// Generic Function that makes the game harder by decreasing indicator delay
  /// </summary>
  public void MakeGameHarder()
  {
    if (indicatorDelay > minIndicatorDelay)
    {
      indicatorDelay -= 0.1f;
      return;
    }
    if (numberOfHazards < maxHazards)
    {
      numberOfHazards++;
      indicatorDelay = OGIndicatorDelay;
    }
  }

  public void Difficulty(out int hazards, out float delay)
  {
    hazards = numberOfHazards;
    delay = indicatorDelay;
  }

}
