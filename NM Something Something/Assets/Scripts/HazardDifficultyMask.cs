using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDifficultyMask {

  private const int OGDifficulty = 1;
  private const int MaxDifficulty = 15;

  private int difficulty = OGDifficulty;

  private const int Boulder = 1<<0;
  private const int Claw = 1<<1;
  private const int Laser = 1<<2;
  private const int Pufferfish = 1<<3;

  /// <summary>
  /// Count the number of obstacles dodged since the last hazard introduction
  /// </summary>
  private int collisionCount = 0;
  /// <summary>
  /// Introduce new Obstacle only after this many dodged obstacles
  /// </summary>
  private const int collisionCountMax = 5;

  public void IncreaseDifficulty()
  {
    //Bit wise Difficulty Increment
    /*
    if (difficulty == 1)
    {
      difficulty = 3;
      return;
    }
    if (difficulty < MaxDifficulty)
    {
      difficulty += 1;
    }
    */
    if (AllHazards())
    {
      Debug.LogError("How did you get here?");
      return;
    }
    if (collisionCount < collisionCountMax)
    {
      collisionCount++;
      return;
    }
    else
    {
      if (ContainsLaser())
      {
        difficulty += Pufferfish;
      }
      if (ContainsClaw())
      {
        difficulty += Laser;
        collisionCount = 0;
      }
      if (ContainsBoulder())
      {
        difficulty += Claw;
        collisionCount = 0;
      }
    }
  }

  public bool ContainsBoulder()
  {
    return (difficulty & Boulder) != 0;
  }

  public bool ContainsClaw()
  {
    return (difficulty & Claw) != 0;
  }

  public bool ContainsLaser()
  {
    return (difficulty & Laser) != 0;
  }

  public bool ContainsPufferfish()
  {
    return (difficulty & Pufferfish) != 0;
  }

  public bool AllHazards()
  {
    return difficulty == MaxDifficulty;
  }
}
