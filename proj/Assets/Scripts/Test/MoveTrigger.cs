using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Unit))]
public class MoveTrigger : MonoBehaviour {
	
	private Unit self;
	public Transform target;
	
	void Awake () {
		self = GetComponent<Unit>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetButtonDown("Vertical")) {
			self.MoveToPosition(target.position, () => {
				Debug.Log("Target reached");
			});
		}
	}
}
