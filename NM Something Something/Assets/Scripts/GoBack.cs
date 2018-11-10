using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoBack : MonoBehaviour {

  public delegate void GoBackDelegate();
  public static event GoBackDelegate GoBackEvent;

  [SerializeField]
  private Button ReturnButton;

  private void Start()
  {
    ReturnButton.onClick.AddListener(goBack);
  }

  // Update is called once per frame
  void Update () {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      goBack();
    }
	}

  private void goBack()
  {
    GoBackEvent();
  }
}
