using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorInfoContainer {

  public BoardManager.Hazard Hazard;
  public float Timer;

  public IndicatorInfoContainer(BoardManager.Hazard hazard, float timer)
  {
    Hazard = hazard;
    Timer = timer;
  }
}
