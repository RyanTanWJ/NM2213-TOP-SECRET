using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour{

  public delegate void TimerDone(Indicator self, IndicatorInfoContainer container);
  public static event TimerDone TimerDoneEvent;

  public BoardManager.BorderSet BorderSet;

  [SerializeField]
  private List<Sprite> warningSymbols;

  [SerializeField]
  private SpriteRenderer warningSymbol;

  private List<IndicatorInfoContainer> FlashTimers = new List<IndicatorInfoContainer>();

	// Update is called once per frame
	void Update () {
		if (FlashTimers.Count==0)
    {
      gameObject.SetActive(false);
    }
    else
    {
      if (FlashTimers[0].Timer <= 0)
      {
        TimerDoneEvent(this, FlashTimers[0]);
        FlashTimers.RemoveAt(0);
        return;
      }
      FlashTimers[0].Timer -= Time.deltaTime;
    }
	}

  public void SetWarningSymbol(BoardManager.Hazard hazard)
  {
    switch (hazard)
    {
      case BoardManager.Hazard.SUSHI:
        warningSymbol.sprite = warningSymbols[1];
        break;
      case BoardManager.Hazard.CLAW:
        warningSymbol.sprite = warningSymbols[2];
        break;
      case BoardManager.Hazard.WASABI:
        warningSymbol.sprite = warningSymbols[3];
        break;
      case BoardManager.Hazard.PUFFERFISH:
        warningSymbol.sprite = warningSymbols[0];
        break;
      default:
        Debug.LogError("FAILURE");
        break;
    }
  }

  public void AddTimer(IndicatorInfoContainer time)
  {
    FlashTimers.Add(time);
  }
}
