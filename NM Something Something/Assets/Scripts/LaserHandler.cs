using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHandler : MonoBehaviour
{
  private List<Laser> TopLasers = new List<Laser>();
  private List<Laser> BotLasers = new List<Laser>();
  private List<Laser> LeftLasers = new List<Laser>();
  private List<Laser> RightLasers = new List<Laser>();

  public void AddLaserToSet(BoardManager.BorderSet indicatorSet, Laser laser)
  {
    switch (indicatorSet)
    {
      case BoardManager.BorderSet.LEFT:
        laser.SetLaserSet(BoardManager.BorderSet.LEFT);
        LeftLasers.Add(laser);
        break;
      case BoardManager.BorderSet.RIGHT:
        laser.SetLaserSet(BoardManager.BorderSet.RIGHT);
        RightLasers.Add(laser);
        laser.transform.eulerAngles = new Vector3(0, 0, 180);
        break;
      case BoardManager.BorderSet.BOT:
        laser.SetLaserSet(BoardManager.BorderSet.BOT);
        BotLasers.Add(laser);
        laser.transform.eulerAngles = new Vector3(0, 0, 90);
        break;
      case BoardManager.BorderSet.TOP:
        laser.SetLaserSet(BoardManager.BorderSet.TOP);
        TopLasers.Add(laser);
        laser.transform.eulerAngles = new Vector3(0, 0, -90);
        break;
      default:
        Debug.LogError("The specified LaserSet does not exist!");
        break;
    }
    laser.gameObject.SetActive(false);
  }

  public void AcitvateLaser(BoardManager.BorderSet laserSet, int index)
  {
    switch (laserSet)
    {
      case BoardManager.BorderSet.LEFT:
        LeftLasers[index].gameObject.SetActive(true);
        LeftLasers[index].FireLaser();
        break;
      case BoardManager.BorderSet.RIGHT:
        RightLasers[index].gameObject.SetActive(true);
        RightLasers[index].FireLaser();
        break;
      case BoardManager.BorderSet.BOT:
        BotLasers[index].gameObject.SetActive(true);
        BotLasers[index].FireLaser();
        break;
      case BoardManager.BorderSet.TOP:
        TopLasers[index].gameObject.SetActive(true);
        TopLasers[index].FireLaser();
        break;
      default:
        Debug.LogError("The specified IndicatorSet does not exist!");
        break;
    }
  }
}
