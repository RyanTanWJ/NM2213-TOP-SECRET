using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave {

  public WaveManager.Hazard Hazard;
  public int HazardNum;
  public float DelayTimer;
  public float WaveTimer;

  public Wave(WaveManager.Hazard haz, int hazNum, float waveTime = 3.5f, float delay = 1.0f)
  {
    Hazard = haz;
    HazardNum = hazNum;
    WaveTimer = waveTime;
    DelayTimer = delay;
  }
}
