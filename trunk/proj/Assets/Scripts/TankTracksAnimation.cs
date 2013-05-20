using UnityEngine;

[RequireComponent(typeof(Animation))]
public class TankTracksAnimation : MonoBehaviour {
	
	private Material leftTrackMaterial;
	private Material rightTrackMaterial;
	private Animation animation;
	private AnimationClip forwardClip;
	private AnimationClip backwardClip;
	private AnimationClip turnLeftClip;
	private AnimationClip turnRightClip;
	
	public float speed = 1.5f;
	
	void Awake () {
		animation = GetComponent<Animation>();
		forwardClip = animation.GetClip("forward");
		backwardClip = animation.GetClip("backward");
		turnLeftClip = animation.GetClip("turnLeft");
		turnRightClip = animation.GetClip("turnRight");
		
		Transform hull = transform.FindChild("hull");
		SkinnedMeshRenderer renderer = hull.GetComponent<SkinnedMeshRenderer>();
		foreach (Material material in renderer.materials) {
			if (material.name == "leftTrackMaterial (Instance)") {
				if (material.shader.name != "Diffuse") 
					Debug.LogWarning("TankTracksAnimation Script works with Diffuse materials only");
				leftTrackMaterial = material;
			} else if (material.name == "rightTrackMaterial (Instance)") {
				if (material.shader.name != "Diffuse") 
					Debug.LogWarning("TankTracksAnimation Script works with Diffuse materials only");
				rightTrackMaterial = material;
			}
		}
	}
	
	void Update () {
		float leftDirection = 0.0f;
		float rightDirection = 0.0f;
		if (animation.clip == forwardClip) {
			rightDirection = 1.0f;
			leftDirection = 1.0f;
		} else if (animation.clip == backwardClip) {
			rightDirection = -1.0f;
			leftDirection = -1.0f;
		} else if (animation.clip == turnLeftClip) {
			rightDirection = 1.0f;
			leftDirection = -1.0f;
		} else if (animation.clip == turnRightClip) {
			rightDirection = -1.0f;
			leftDirection = 1.0f;
		}
		
		Vector2 offset = rightTrackMaterial.mainTextureOffset;
		offset.x += speed * rightDirection * Time.deltaTime;
		rightTrackMaterial.mainTextureOffset = offset;
		offset = leftTrackMaterial.mainTextureOffset;
		offset.x -= speed * leftDirection * Time.deltaTime;
		leftTrackMaterial.mainTextureOffset = offset;
	}
}
