using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

  private BoardManager.BorderSet set;

  [SerializeField]
  private Animator laserAnimator;

  public void SetLaserSet(BoardManager.BorderSet laserSet)
  {
    set = laserSet;
  }

  public void FireLaser()
  {
    StartCoroutine(LaserTimer());
  }

  /// <summary>
  /// Plays animation then disables it
  /// </summary>
  /// <returns></returns>
  IEnumerator LaserTimer()
  {
    laserAnimator.Play("LaserFire");
    yield return new WaitForSeconds(laserAnimator.GetCurrentAnimatorStateInfo(0).length);
    gameObject.SetActive(false);
  }
}
