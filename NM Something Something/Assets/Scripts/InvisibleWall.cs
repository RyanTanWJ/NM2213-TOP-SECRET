using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
  public delegate void DeactivateBoulder(GameObject boulder);
  public static event DeactivateBoulder DeactivateBoulderEvent;

  public delegate void DeactivateClaw(GameObject claw);
  public static event DeactivateClaw DeactivateClawEvent;

  public delegate void MakeGameHarder();
  public static event MakeGameHarder MakeGameHarderEvent;

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.name.Contains("Boulder"))
    {
      DeactivateBoulderEvent(collision.gameObject);
      MakeGameHarderEvent();
    }
    else if (collision.gameObject.CompareTag("Nuisance"))
    {
      DeactivateClawEvent(collision.gameObject);
    }
    else
    {
      Debug.LogError(collision.gameObject.name + " not handled by Invisible Wall");
    }
  }
}
