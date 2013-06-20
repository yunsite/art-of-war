using UnityEngine;
using System.Collections;

public class MoveTest : MonoBehaviour {

    public MotionController unit;
    public PositionObserver ground;

	// Use this for initialization
	void Start () {
        ground.Clicked += ground_Clicked;
	}

    void ground_Clicked(object sender, PositionEventArgs e)
    {
        unit.Move(e.Position);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
