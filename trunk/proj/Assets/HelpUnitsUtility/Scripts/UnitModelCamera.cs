using System.Collections.Generic;
using UnityEngine;

public class UnitModelCamera : MonoBehaviour {
	
	public List<Transform> targets;
	public Transform selectedTarget;
	public int index;
	public float speed = 5f;
		
	void Start () {
		selectedTarget = targets[index];
	}
	
	void Update () {
		transform.position = Vector3.Lerp(transform.position, selectedTarget.position, Time.deltaTime * speed);
		transform.forward = Vector3.Lerp(transform.forward, selectedTarget.forward, Time.deltaTime * speed);
	}
	
	public void NextTarget () {
		index = (index + 1) % targets.Count;
		selectedTarget = targets[index];
	}
	
	public void PrevTarget () {
		index = (targets.Count + index - 1) % targets.Count;
		selectedTarget = targets[index];
	}
	
	public void SelectTarget(int index) {
		this.index = index % targets.Count;
		selectedTarget = targets[this.index];
	}
}
