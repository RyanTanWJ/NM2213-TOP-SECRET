using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour {

  public delegate void DeactivateBoulder(GameObject boulder);
  public static event DeactivateBoulder DeactivateBoulderEvent;

  public delegate void MakeGameHarder();
  public static event MakeGameHarder MakeGameHarderEvent;

  private void OnCollisionEnter2D(Collision2D collision)
  {
    Debug.Log("collided with: " + collision.gameObject.name);
    DeactivateBoulderEvent(collision.gameObject);
    MakeGameHarderEvent();
  }
}
