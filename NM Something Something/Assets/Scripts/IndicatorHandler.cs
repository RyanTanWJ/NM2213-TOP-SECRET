using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorHandler : MonoBehaviour{

  public enum IndicatorSet { TOP, BOT, LEFT, RIGHT };

  private List<Indicator> TopIndicators = new List<Indicator>();
  private List<Indicator> BotIndicators = new List<Indicator>();
  private List<Indicator> LeftIndicators = new List<Indicator>();
  private List<Indicator> RightIndicators = new List<Indicator>();

  public delegate void SpawnHazards(BoardManager.Direction direction, Vector2Int spawnPos);
  public static event SpawnHazards SpawnHazardsEvent;

  private void OnEnable()
  {
    Indicator.TimerDoneEvent += OnTimerDone;
  }

  private void OnDisable()
  {
    Indicator.TimerDoneEvent -= OnTimerDone;
  }

  public void AddIndicatorToSet(IndicatorSet indicatorSet, Indicator indicator)
  {
    switch (indicatorSet)
    {
      case IndicatorSet.LEFT:
        indicator.SetIndicatorSet(IndicatorSet.LEFT);
        LeftIndicators.Add(indicator);
        break;
      case IndicatorSet.RIGHT:
        indicator.SetIndicatorSet(IndicatorSet.RIGHT);
        RightIndicators.Add(indicator);
        indicator.transform.eulerAngles = new Vector3(0, 0, 180);
        break;
      case IndicatorSet.BOT:
        indicator.SetIndicatorSet(IndicatorSet.BOT);
        BotIndicators.Add(indicator);
        indicator.transform.eulerAngles = new Vector3(0, 0, 90);
        break;
      case IndicatorSet.TOP:
        indicator.SetIndicatorSet(IndicatorSet.TOP);
        TopIndicators.Add(indicator);
        indicator.transform.eulerAngles = new Vector3(0, 0, -90);
        break;
      default:
        Debug.LogError("The specified IndicatorSet does not exist!");
        break;
    }
  }

  public void AcitvateIndicator(IndicatorSet indicatorSet, int index, float time)
  {
    switch (indicatorSet)
    {
      case IndicatorSet.LEFT:
        LeftIndicators[index].gameObject.SetActive(true);
        LeftIndicators[index].AddTimer(time);
        break;
      case IndicatorSet.RIGHT:
        RightIndicators[index].gameObject.SetActive(true);
        RightIndicators[index].AddTimer(time);
        break;
      case IndicatorSet.BOT:
        BotIndicators[index].gameObject.SetActive(true);
        BotIndicators[index].AddTimer(time);
        break;
      case IndicatorSet.TOP:
        TopIndicators[index].gameObject.SetActive(true);
        TopIndicators[index].AddTimer(time);
        break;
      default:
        Debug.LogError("The specified IndicatorSet does not exist!");
        break;
    }
  }

  private void OnTimerDone(IndicatorSet set, Indicator indicator)
  {
    int row = 0;
    int col = 0;
    switch (set)
    {
      case IndicatorSet.LEFT:
        row = LeftIndicators.IndexOf(indicator);
        SpawnHazardsEvent(BoardManager.Direction.RIGHT, new Vector2Int(row, col));
        break;
      case IndicatorSet.RIGHT:
        row = RightIndicators.IndexOf(indicator);
        col = RightIndicators.Count - 1;
        SpawnHazardsEvent(BoardManager.Direction.LEFT, new Vector2Int(row, col));
        break;
      case IndicatorSet.BOT:
        col = BotIndicators.IndexOf(indicator);
        SpawnHazardsEvent(BoardManager.Direction.UP, new Vector2Int(row, col));
        break;
      case IndicatorSet.TOP:
        col = TopIndicators.IndexOf(indicator);
        row = TopIndicators.Count - 1;
        SpawnHazardsEvent(BoardManager.Direction.DOWN, new Vector2Int(row, col));
        break;
      default:
        Debug.LogError("The specified IndicatorSet does not exist!");
        break;
    }
  }
}
