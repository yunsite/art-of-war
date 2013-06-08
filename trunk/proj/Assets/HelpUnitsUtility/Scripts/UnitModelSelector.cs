using UnityEngine;

public class UnitModelSelector : UILogic {
	
	public new UnitModelCamera modelCamera;
	
	public void NextModel() {
		modelCamera.NextTarget();
	}
	
	public void PrevModel() {
		modelCamera.PrevTarget();
	}
	
	public void SelectModel(int index) {
		modelCamera.SelectTarget(index);
	}
	
	public int GetModel () {
		return modelCamera.index;
	}
	
	public Unit GetUnit () {
		return modelCamera.GetUnit();
	}
}
