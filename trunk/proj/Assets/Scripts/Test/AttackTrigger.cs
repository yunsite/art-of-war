using UnityEngine;

// Skrypt uruchamiający atak jednostki na wybrany cel
[RequireComponent(typeof(Unit))]
public class AttackTrigger : MonoBehaviour {
	
	private Unit self;
	public Unit target;
	
	// Use this for initialization
	void Awake () {
		self = GetComponent<Unit>();
        self.ActionCompleted += self_ActionCompleted;
	}

    void self_ActionCompleted(object sender, System.EventArgs e)
    {
        Debug.Log("Attack ended");
    }
	
	// Update is called once per frame
	void Update () {
		if (target != null && Input.GetButtonDown("Jump")) {
			self.Attack(target);
		}
	}
}
