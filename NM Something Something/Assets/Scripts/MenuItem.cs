using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
  public delegate void MenuItemButtonClicked(MenuItem self);
  public static event MenuItemButtonClicked MenuItemButtonClickedEvent;

  [SerializeField]
  Button MenuItemButton;
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

  private void Start()
  {
    MenuItemButton.onClick.AddListener(OnButtonClicked);
  }

  private void OnButtonClicked()
  {
    MenuItemButtonClickedEvent(this);
  }
}
