using UnityEngine;

[RequireComponent(typeof(Animation))]
public class TracksAnimation : MonoBehaviour {
	
	private Material leftTrackMaterial;
	private Material rightTrackMaterial;
	private new Animation animation;
	
	public string forwardClipName = "forward";
	public string backwardClipName = "backward";
	public string turnLeftClipName = "turnLeft";
	public string turnRightClipName = "turnRight";
	
	public float speed = 1.5f;
	public string meshName = "hullMesh";
	public string leftTrackMaterialName = "leftTrackMaterial";
	public float leftSpeedTrim = 1.0f;
	public string rightTrackMaterialName = "rightTrackMaterial";
	public float rightSpeedTrim = 1.0f;
	
	void Awake () {
		animation = base.animation;
		
		Transform hull = transform.FindChild(meshName);
		SkinnedMeshRenderer renderer = hull.GetComponent<SkinnedMeshRenderer>();
		foreach (Material material in renderer.materials) {
			if (material.name == leftTrackMaterialName + " (Instance)") {
				if (material.shader.name != "Diffuse") 
					Debug.LogWarning("TracksAnimation Script works with Diffuse materials only");
				leftTrackMaterial = material;
			} else if (material.name == rightTrackMaterialName + " (Instance)") {
				if (material.shader.name != "Diffuse") 
					Debug.LogWarning("TracksAnimation Script works with Diffuse materials only");
				rightTrackMaterial = material;
			}
		}
	}
	
	void Update () {
		float forwardWeight = animation[forwardClipName].weight;
		float backwardWeight = animation[backwardClipName].weight;
		float turnLeftWeight = animation[turnLeftClipName].weight;
		float turnRightWeight = animation[turnRightClipName].weight;
		
		float leftDirection = forwardWeight - backwardWeight - turnLeftWeight + turnRightWeight;
		float rightDirection = forwardWeight - backwardWeight + turnLeftWeight - turnRightWeight;
		
		Vector2 offset = rightTrackMaterial.mainTextureOffset;
		offset.x += speed * rightSpeedTrim * rightDirection * Time.deltaTime;
		rightTrackMaterial.mainTextureOffset = offset;
		offset = leftTrackMaterial.mainTextureOffset;
		offset.x -= speed * leftSpeedTrim * leftDirection * Time.deltaTime;
		leftTrackMaterial.mainTextureOffset = offset;
	}
}
