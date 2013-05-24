using UnityEngine;

[RequireComponent(typeof(Animation))]
public class TracksAnimationTest : MonoBehaviour {
	
	public string forwardClipName = "forward";
	public string backwardClipName = "backward";
	public string turnLeftClipName = "turnLeft";
	public string turnRightClipName = "turnRight";
	public string noneClipName = "none";
	
	public float inputTreshold = 0.1f;
	
	void Update () {
		float horizontalAxis = Input.GetAxis("Horizontal");
		float verticalAxis = Input.GetAxis("Vertical");
		if (horizontalAxis > inputTreshold) {
			animation.CrossFade(turnRightClipName);
		} else if (horizontalAxis < -inputTreshold) {
			animation.CrossFade(turnLeftClipName);
		} else if (verticalAxis > inputTreshold) {
			animation.CrossFade(forwardClipName);
		} else if (verticalAxis < -inputTreshold) {
			animation.CrossFade(backwardClipName);
		} else {
			animation.CrossFade(noneClipName);
		}
	}
}
