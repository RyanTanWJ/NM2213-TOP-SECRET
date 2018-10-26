using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyAI : MonoBehaviour {

  //Upper limit for the number of hazards that can be spawned
  private const int maxHazards = 10;

  //Original number of hazards that can be spawned
  private const int OGHazards = 1;

  //Lower limit for the delay before indicator switches off and hazard spawns, currently set to average human reaction time
  private const float minIndicatorDelay = 0.4f;

  //Original delay before indicator switches off and hazard spawns
  private const float OGIndicatorDelay = 1.5f;

  //Max Number of Hazards that should be spawned
  private int numberOfHazards = OGHazards;

  //Delay before indicator switches off and hazard spawns
  private float indicatorDelay = OGIndicatorDelay;

  //Decides what hazards are available
  HazardDifficultyMask hazardDifficulty = new HazardDifficultyMask();

  private void OnEnable()
  {
    InvisibleWall.MakeGameHarderEvent += MakeGameHarder;
    Laser.MakeGameHarderEvent += MakeGameHarder;
    Pufferfish.MakeGameHarderEvent += MakeGameHarder;
    ClawPool.MakeGameHarderEvent += MakeGameHarder;
  }

  private void OnDisable()
  {
    InvisibleWall.MakeGameHarderEvent -= MakeGameHarder;
    Laser.MakeGameHarderEvent -= MakeGameHarder;
    Pufferfish.MakeGameHarderEvent -= MakeGameHarder;
    ClawPool.MakeGameHarderEvent += MakeGameHarder;
  }

  private List<BoardManager.Hazard> GetListOfHazards()
  {
    List<BoardManager.Hazard> retList = new List<BoardManager.Hazard>();
    if (hazardDifficulty.ContainsBoulder())
    {
      retList.Add(BoardManager.Hazard.BOULDER);
    }
    if (hazardDifficulty.ContainsClaw())
    {
      retList.Add(BoardManager.Hazard.CLAW);
    }
    if (hazardDifficulty.ContainsLaser())
    {
      retList.Add(BoardManager.Hazard.LASER);
    }
    if (hazardDifficulty.ContainsPufferfish())
    {
      retList.Add(BoardManager.Hazard.PUFFERFISH);
    }
    return retList;
  }

  /// <summary>
  /// Generic Function that makes the game harder by decreasing indicator delay
  /// </summary>
  public void MakeGameHarder()
  {
    //Increase Hazard Types
    if (!hazardDifficulty.AllHazards())
    {
      hazardDifficulty.IncreaseDifficulty();
      return;
    }
    //Decrease delay
    if (indicatorDelay > minIndicatorDelay)
    {
      indicatorDelay -= 0.1f / numberOfHazards;
      Mathf.Clamp(indicatorDelay, minIndicatorDelay, OGIndicatorDelay);
      return;
    }
    //Increase Hazard
    if (numberOfHazards < maxHazards)
    {
      numberOfHazards++;
      indicatorDelay = OGIndicatorDelay;
    }
  }

  public void Difficulty(out List<BoardManager.Hazard> hazards, out int hazardNum, out float indDelay)
  {
    hazards = GetListOfHazards();
    hazardNum = numberOfHazards;
    indDelay = indicatorDelay;
  }
}
