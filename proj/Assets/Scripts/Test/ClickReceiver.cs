using UnityEngine;
using System.Collections;

// Skrypt testuący zdarzenie kliknięcia na jednostkę.
[RequireComponent(typeof(Unit))]
public class ClickReceiver : MonoBehaviour {
	
	private Unit self;
	
	// Use this for initialization
	void Awake () {
		self = GetComponent<Unit>();
		self.Clicked += HandleSelfClicked;
	}

	void HandleSelfClicked (object sender, System.EventArgs e)
	{
		Debug.Log(sender + " clicked.");
	}
}
