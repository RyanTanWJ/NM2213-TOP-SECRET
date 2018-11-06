using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorHandler : MonoBehaviour{

  private List<Indicator> TopIndicators = new List<Indicator>();
  private List<Indicator> BotIndicators = new List<Indicator>();
  private List<Indicator> LeftIndicators = new List<Indicator>();
  private List<Indicator> RightIndicators = new List<Indicator>();

  public delegate void SpawnHazards(BoardManager.Hazard hazard, BoardManager.Direction direction, Vector2Int spawnPos);
  public static event SpawnHazards SpawnHazardsEvent;

  private void OnEnable()
  {
    Indicator.TimerDoneEvent += OnTimerDone;
  }

  private void OnDisable()
  {
    Indicator.TimerDoneEvent -= OnTimerDone;
  }

  public void AddIndicatorToSet(BoardManager.BorderSet indicatorSet, Indicator indicator)
  {
    switch (indicatorSet)
    {
      case BoardManager.BorderSet.LEFT:
        indicator.BorderSet = BoardManager.BorderSet.LEFT;
        LeftIndicators.Add(indicator);
        indicator.transform.eulerAngles = new Vector3(0, 0, 90);
        break;
      case BoardManager.BorderSet.RIGHT:
        indicator.BorderSet = BoardManager.BorderSet.RIGHT;
        RightIndicators.Add(indicator);
        indicator.transform.eulerAngles = new Vector3(0, 0, -90);
        break;
      case BoardManager.BorderSet.BOT:
        indicator.BorderSet = BoardManager.BorderSet.BOT;
        BotIndicators.Add(indicator);
        indicator.transform.eulerAngles = new Vector3(0, 0, 180);
        break;
      case BoardManager.BorderSet.TOP:
        indicator.BorderSet = BoardManager.BorderSet.TOP;
        TopIndicators.Add(indicator);
        break;
      default:
        Debug.LogError("The specified IndicatorSet does not exist!");
        break;
    }
    indicator.SetWarningSymbolOrientation(new Vector3(0, 0, 0));
  }

  public void AcitvateIndicator(BoardManager.Hazard hazard, BoardManager.BorderSet indicatorSet, int index, float time)
  {
    switch (indicatorSet)
    {
      case BoardManager.BorderSet.LEFT:
        LeftIndicators[index].gameObject.SetActive(true);
        LeftIndicators[index].SetWarningSymbol(hazard);
        LeftIndicators[index].AddTimer(new IndicatorInfoContainer(hazard, time));
        break;
      case BoardManager.BorderSet.RIGHT:
        RightIndicators[index].gameObject.SetActive(true);
        RightIndicators[index].SetWarningSymbol(hazard);
        RightIndicators[index].AddTimer(new IndicatorInfoContainer(hazard, time));
        break;
      case BoardManager.BorderSet.BOT:
        BotIndicators[index].gameObject.SetActive(true);
        BotIndicators[index].SetWarningSymbol(hazard);
        BotIndicators[index].AddTimer(new IndicatorInfoContainer(hazard, time));
        break;
      case BoardManager.BorderSet.TOP:
        TopIndicators[index].gameObject.SetActive(true);
        TopIndicators[index].SetWarningSymbol(hazard);
        TopIndicators[index].AddTimer(new IndicatorInfoContainer(hazard, time));
        break;
      default:
        Debug.LogError("The specified BorderSet does not exist!");
        break;
    }
  }

  private void OnTimerDone(Indicator indicator, IndicatorInfoContainer container)
  {
    int row = 0;
    int col = 0;
    switch (indicator.BorderSet)
    {
      case BoardManager.BorderSet.LEFT:
        row = LeftIndicators.IndexOf(indicator);
        SpawnHazardsEvent(container.Hazard, BoardManager.Direction.RIGHT, new Vector2Int(row, col));
        break;
      case BoardManager.BorderSet.RIGHT:
        row = RightIndicators.IndexOf(indicator);
        col = RightIndicators.Count - 1;
        SpawnHazardsEvent(container.Hazard, BoardManager.Direction.LEFT, new Vector2Int(row, col));
        break;
      case BoardManager.BorderSet.BOT:
        col = BotIndicators.IndexOf(indicator);
        SpawnHazardsEvent(container.Hazard, BoardManager.Direction.UP, new Vector2Int(row, col));
        break;
      case BoardManager.BorderSet.TOP:
        col = TopIndicators.IndexOf(indicator);
        row = TopIndicators.Count - 1;
        SpawnHazardsEvent(container.Hazard, BoardManager.Direction.DOWN, new Vector2Int(row, col));
        break;
      default:
        Debug.LogError("The specified IndicatorSet does not exist!");
        break;
    }
  }
}
