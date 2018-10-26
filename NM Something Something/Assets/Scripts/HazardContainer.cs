using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardContainer
{
  public HazardContainer(Vector2Int hazardSpawnPoint, BoardManager.Hazard hazardToSpawn, BoardManager.BorderSet borderSet, float preIndicatorDelay, float indicatorDelay)
  {
    HazardSpawnPoint = hazardSpawnPoint;
    HazardToSpawn = hazardToSpawn;
    BorderSet = borderSet;
    PreIndicatorDelay = preIndicatorDelay;
    IndicatorDelay = indicatorDelay;
  }

  public Vector2Int HazardSpawnPoint { get; private set; }

  public BoardManager.Hazard HazardToSpawn { get; private set; }

  public BoardManager.BorderSet BorderSet { get; private set; }

  public float PreIndicatorDelay { get; private set; }

  public float IndicatorDelay { get; private set; }

  public float CombinedDelay
  {
    get { return PreIndicatorDelay + IndicatorDelay; }
  }

  public void PrintContents()
  {
    Debug.Log("___Printing Container Contents___");
    Debug.Log("HazardSpawnPoint = " + HazardSpawnPoint);
    Debug.Log("HazardToSpawn = " + HazardToSpawn);
    Debug.Log("BorderSet = " + BorderSet);
    Debug.Log("PreIndicatorDelay = " + PreIndicatorDelay);
    Debug.Log("IndicatorDelay = " + IndicatorDelay);
    Debug.Log("___Finished Printing Contents___");
  }
}
