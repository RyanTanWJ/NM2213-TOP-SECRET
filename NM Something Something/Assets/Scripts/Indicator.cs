using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour{

  public delegate void TimerDone(Indicator self);
  public static event TimerDone TimerDoneEvent;

  public BoardManager.BorderSet BorderSet;

  public BoardManager.Hazard Hazard;

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
        TimerDoneEvent(this);
        return;
      }
      FlashTimers[0] -= Time.deltaTime;
    }
	}

  public void AddTimer(float time)
  {
    FlashTimers.Add(time);
  }
}
