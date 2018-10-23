using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour {

  [SerializeField]
  List<Sprite> sprites;
  [SerializeField]
  Image image;

  public void SwitchTextHighlight(bool highlight)
  {
    if (highlight)
    {
      image.sprite = sprites[1];
    }
    else
    {
      image.sprite = sprites[0];
    }
  }
}
