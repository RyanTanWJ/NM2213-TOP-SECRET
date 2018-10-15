using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour{

  public delegate void TimerDone(IndicatorHandler.IndicatorSet indicatorSet, Indicator self);
  public static event TimerDone TimerDoneEvent;

  private IndicatorHandler.IndicatorSet set;

  private List<float> FlashTimers = new List<float>();
	// Update is called once per frame
	void Update () {
		if (FlashTimers.Count==0)
    {
      gameObject.SetActive(false);
    }
    else
    {
      if (FlashTimers[0] <= 0)
      {
        FlashTimers.RemoveAt(0);
        TimerDoneEvent(set, this);
        return;
      }
      FlashTimers[0] -= Time.deltaTime;
    }
	}

  public void AddTimer(float time)
  {
    FlashTimers.Add(time);
  }

  public void SetIndicatorSet(IndicatorHandler.IndicatorSet indicatorSet)
  {
    set = indicatorSet;
  }
}
