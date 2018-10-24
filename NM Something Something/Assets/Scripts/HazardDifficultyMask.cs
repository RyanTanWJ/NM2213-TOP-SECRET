using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDifficultyMask {

  private const int OGDifficulty = 15;
  private const int MaxDifficulty = 15;

  private int difficulty = OGDifficulty;

  private const int Boulder = 1<<0;
  private const int Claw = 1<<1;
  private const int Laser = 1<<2;
  private const int Pufferfish = 1<<3;

  public void IncreaseDifficulty()
  {
    if (difficulty == 1)
    {
      difficulty = 3;
      return;
    }
    if (difficulty < MaxDifficulty)
    {
      difficulty += 1;
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
