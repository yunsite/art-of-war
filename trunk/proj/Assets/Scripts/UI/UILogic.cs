using UnityEngine;

public abstract class UILogic : MonoBehaviour {

	public virtual void Show () {
		gameObject.SetActive(true);
	}
	
	public virtual void Hide () {
		gameObject.SetActive(false);
	}
	
}
