using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBGM : MonoBehaviour {

  [SerializeField]
  private AudioSource BGM;

  private void OnEnable()
  {
    Player.GameOverEvent += Stop;
  }

  private void OnDisable()
  {
    Player.GameOverEvent -= Stop;
  }

  private void Stop()
  {
    BGM.Stop();
  }
}
