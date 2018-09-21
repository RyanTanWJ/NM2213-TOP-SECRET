using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBack : MonoBehaviour {

  public delegate void GoBackDelegate();
  public static event GoBackDelegate GoBackEvent;
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown)
    {
      GoBackEvent();
    }
	}
}
