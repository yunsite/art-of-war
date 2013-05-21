using System;
using UnityEngine;

public class ButtonLogic : MonoBehaviour {

	public EventHandler ButtonClicked;
	
	void OnClick () {
		if (ButtonClicked != null) {
			ButtonClicked (gameObject, new EventArgs());
		}
	}
	
}
