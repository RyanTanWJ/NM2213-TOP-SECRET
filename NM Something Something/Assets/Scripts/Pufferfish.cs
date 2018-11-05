using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pufferfish : MonoBehaviour {
  private const float hardCodedAnimationTime = 1.5f;

  [SerializeField]
  Animator animator;

  [SerializeField]
  List<GameObject> shards;

  [SerializeField]
  AudioSource blinking;

  [SerializeField]
  AudioSource splat;

  public delegate void MakeGameHarder();
  public static event MakeGameHarder MakeGameHarderEvent;

  public delegate void DeactivatePufferfish(GameObject pufferfish);
  public static event DeactivatePufferfish DeactivatePufferfishEvent;

  /// <summary>
  /// Activates the bomb indicator for the given time before activating the actual bomb
  /// </summary>
  /// <param name="time">The amount of time the indicator should play</param>
  public void TimeBomb(float time)
  {
    explode(time);
  }

  private void explode(float indicatorTime)
  {
    StartCoroutine(PufferfishAnimation(indicatorTime));
  }

  /// <summary>
  /// Plays animation then disables it
  /// </summary>
  IEnumerator PufferfishAnimation(float indicatorTime)
  {
    yield return new WaitForSeconds(indicatorTime);
    animator.SetBool("IndicatorOff", true);
    yield return new WaitForSeconds(hardCodedAnimationTime);
    animator.SetBool("IndicatorOff", false);
    MakeGameHarderEvent();
    ResetShards();
    DeactivatePufferfishEvent(gameObject);
  }

  private void ResetShards()
  {
    foreach (GameObject shard in shards)
    {
      shard.gameObject.transform.localPosition = new Vector3(0, 0, 0);
      shard.gameObject.SetActive(false);
    }
  }

  public void Blink()
  {
    blinking.Play();
  }

  public void Splat()
  {
    splat.Play();
  }
}
