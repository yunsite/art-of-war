using UnityEngine;

// Skrypt uruchamiający atak jednostki na wybrany cel
[RequireComponent(typeof(Unit))]
public class AttackTrigger : MonoBehaviour {
	
	private Unit self;
	public Unit target;
	
	// Use this for initialization
	void Awake () {
		self = GetComponent<Unit>();
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null && Input.GetButtonDown("Jump")) {
			self.Attack(target, () => {
				Debug.Log("Attack ended");
			});
		}
	}
}
