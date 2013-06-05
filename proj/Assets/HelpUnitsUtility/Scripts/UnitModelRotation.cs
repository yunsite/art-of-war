using UnityEngine;

public class UnitModelRotation : MonoBehaviour {
	
	public float speed = 60f;
	
	void Update () {
		transform.Rotate(0, speed * Time.deltaTime, 0, Space.Self);
	}
}
