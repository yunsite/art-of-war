using System;
using UnityEngine;

public class PositionObserver : MonoBehaviour {
	
	public float raycastDistance = 1000;
	public event EventHandler<PositionEventArgs> Clicked;

	void OnMouseUpAsButton () {
		if (Clicked != null) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, raycastDistance)) {
				Clicked(gameObject, new PositionEventArgs { Position = hit.point });
			}
		}
	}
	
}

public class PositionEventArgs : EventArgs {
	public Vector3 Position;
}