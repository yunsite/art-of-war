using UnityEngine;

public class TerrainClickReceiver : MonoBehaviour {
	
	private PositionObserver self;
	
	void Awake () {
		self = GetComponent<PositionObserver>();
		self.Clicked += HandleSelfClicked;
	}

	void HandleSelfClicked (object sender, PositionEventArgs e)
	{
		Debug.Log("Terrain clicked at position: " + e.Position);
	}
	
}
