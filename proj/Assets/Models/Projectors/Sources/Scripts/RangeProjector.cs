using UnityEngine;

[RequireComponent(typeof(Projector))]
public class RangeProjector : MonoBehaviour {
	
	private Transform selfTransform;
	private Projector projector;
	
	public float IntensityPercentage = 100;
	public float Height = 20;
	public float RangeRaius = 40;
	public float RadiusOffsetPercentage = 5.5f;
	
	void Awake () {
		selfTransform = transform;
		projector = GetComponent<Projector>();
        projector.material = new Material(projector.material);
	}
	
	void Start () {
		if (Height <= 0) Height = 10;
		if (RangeRaius <= 0) RangeRaius = Height;
		SetRange(RangeRaius);
	}
	
	public void SetRange(float radius) {
		if (radius > 0) {
			Vector3 position = selfTransform.position;
			position.y = Terrain.activeTerrain.SampleHeight(position) + Height;
			selfTransform.position = position;
			projector.farClipPlane = (1f + IntensityPercentage / 100f) * Height;
			RangeRaius = radius;
			projector.fieldOfView = 2.0f * Mathf.Rad2Deg 
				* Mathf.Atan(RangeRaius * (1f + RadiusOffsetPercentage / 100f) / Height);
		}
	}
	
	public void SetColor (Color color) {
		projector.material.color = color;
	}
}
