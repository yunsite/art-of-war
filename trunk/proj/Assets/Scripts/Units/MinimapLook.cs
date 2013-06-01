using UnityEngine;

public class MinimapLook : MonoBehaviour {
	
	public Transform target;
	
	void Update () {
		transform.position = target.position;
	}
}
