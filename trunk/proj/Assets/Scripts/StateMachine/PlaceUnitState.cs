using System;

public class PlaceUnitState : GameState
{
	//private SelectedUIInstance ui;
	
	public PlaceUnitState(GameController controller) : base (controller) 
	{
		if (UnityEngine.Application.loadedLevelName != "HillyTestScene") 
			UnityEngine.Application.LoadLevel("HillyTestScene");
		GameManager manager = GameManager.Instance();
		//ui = manager.SelectedUIInstance.SelectedUIInstance;
		//ui.BWPButton.ButtonClicked += BWPButtonClickedHandler;
		//ui.TankButton.ButtonClicked += TankButtonClickedHandler;
		//ui.HeliButton.ButtonClicked += HeliButtonClickedHandler;
		//ui.HeavyTankButton.ButtonClicked += HeavyTankButtonClickedHandler;
		//ui.AltyleryButton.ButtonClicked += AltyleryButtonClickedHandler;
		//ui.Show();
	}
	
	private void SelectUnit(Unit unit)
	{
		//unit.OnClick();
	}

	//co się dzieje po zakończeniu rundy?
	private void EndTurnButtonClickedHandler(Object sender, EventArgs args)
	{
		throw new NotImplementedException();			
	}
	
	private void DeselectUnitButtonClickedHandler(Object sender, EventArgs args)
	{
		DeselectCurrnetUnit();		
	}
	
	//nie wiem jak to ma działać
	private void DeselectCurrnetUnit()
	{
		throw new NotImplementedException();			
	}
	
	//nie wiem jak to ma działać
	private void CreateUnitOnPosition(UnityEngine.Vector3 worldPosition, UnityEngine.Object prefab)
	{
		throw new NotImplementedException();
	}
	
	private void RemoveUnit(Unit unit)
	{
		unit.Die();			
	}
	
	private void BWPButtonClickedHandler(Object sender, EventArgs args)
	{
		CreateUnitOnPosition(new UnityEngine.Vector3(0,0,0), UnityEngine.Resources.Load("Prefab_BWPUnit"));
	}

	private void TankButtonClickedHandler(Object sender, EventArgs args)
	{
		//CreateUnitOnPosition(new UnityEngine.Vector3(0,0,0), UnityEngine.Resources.Load("Prefab_TankUnit"));
	}
	
	private void HeliButtonClickedHandler(Object sender, EventArgs args)
	{
		CreateUnitOnPosition(new UnityEngine.Vector3(0,0,0), UnityEngine.Resources.Load("PPrefab_HeliUnit"));
	}
	
	private void HeavyTankButtonClickedHandler(Object sender, EventArgs args)
	{
		CreateUnitOnPosition(new UnityEngine.Vector3(0,0,0), UnityEngine.Resources.Load("Prefab_HeavyTankUnit"));
	}
	
	private void AltyleryButtonClickedHandler(Object sender, EventArgs args)
	{
		//CreateUnitOnPosition(new UnityEngine.Vector3(0,0,0), Resources.Load("Prefab_AltyleryUnit"));
	}
	
	//nie wiem jak to ma działać
	private void UpdatePoints(Object sender, EventArgs args)
	{
		throw new NotImplementedException();			
	}
	
	private void ContinueButtonClickedHandler(Object sender, EventArgs args)
	{
		 GameManager.Instance().GameControllerInstance.GoToInGameState();
	}
}