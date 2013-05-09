using UnityEngine;

public abstract class UILogic : MonoBehaviour {

	public virtual void Show () {
		enabled = true;
	}
	
	public virtual void Hide () {
		enabled = false;
	}
	
}
