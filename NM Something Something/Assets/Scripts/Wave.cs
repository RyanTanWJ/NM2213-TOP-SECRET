using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave {

  public WaveManager.Hazard Hazard;
  public int HazardNum;

  public Wave(WaveManager.Hazard haz, int hazNum)
  {
    Hazard = haz;
    HazardNum = hazNum;
  }
}
