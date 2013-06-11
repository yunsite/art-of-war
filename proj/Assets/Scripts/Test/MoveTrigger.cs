using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Unit))]
public class MoveTrigger : MonoBehaviour {
	
	private Unit self;
	public Transform target;
	
	void Awake () {
		self = GetComponent<Unit>();
        self.ActionCompleted += self_ActionCompleted;
	}

    void self_ActionCompleted(object sender, System.EventArgs e)
    {
        Debug.Log("Target reached");
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetButtonDown("Vertical")) {
			self.MoveToPosition(target.position);
		}
	}
}
