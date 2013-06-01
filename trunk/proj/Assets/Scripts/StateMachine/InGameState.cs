using System;
using UnityEngine;

public class InGameState : GameState
{
	
	private InGameUI ui;
	private Unit selectedUnit;
	private bool isAtacking;
	private bool isMoving;
	private bool isBlocked;
	
	public InGameState (GameController controller) : base (controller) {
		GameManager manager = GameManager.Instance();
		manager.LevelLoadedEvent += LevelLoaded;
		if (Application.loadedLevelName != "MainGameScene") 
			Application.LoadLevel("MainGameScene");

		ui = manager.GameUIInstance.InGameUIInstance;
		ui.EndTurnButton.ButtonClicked += EndTurnButtonClickedHandler;
		ui.AttackButton.ButtonClicked += AttackButtonClickedHandler;
		ui.MoveButton.ButtonClicked += MoveButtonClickedHandler;
		ui.Show();


	}

	public void LevelLoaded(object sender, EventArgs args)
	{
		var units = (Unit[])UnityEngine.Object.FindObjectsOfType (typeof(Unit));
		foreach(var unit in units)
		{
			unit.Clicked += UnitClickedEventHandler;
		}
		var positionObserver =(PositionObserver) UnityEngine.Object.FindObjectOfType (typeof(PositionObserver));
		positionObserver.Clicked += TerrainClickedHandler;

		GameManager manager = GameManager.Instance();
		manager.LevelLoadedEvent -= LevelLoaded;
	}

	private void ShowInGameMenu()
	{
		ui.Show();
	}
	
	private void HideInGameMenu()
	{
		ui.Hide();
	}
	
	private void UnitClickedEventHandler (object sender, EventArgs args)
	{
		if(!isAtacking && selectedUnit == null)
		{
			SelectUnit((Unit) sender);
			Debug.Log("Zmieniam jednostke zaznaczone na "+((Unit) sender).ToString());
		}
		else if(!isAtacking && selectedUnit != (Unit) sender)
		{
			SelectUnit((Unit) sender);
			Debug.Log("Zmieniam jednostke zaznaczona na "+((Unit) sender).ToString());
			
		}
		else if(isAtacking && selectedUnit != null)
		{
            selectedUnit.Attack((Unit)sender,()=> AttackEnded());
			Debug.Log("Atakuje jednostka zaznaczona: "+selectedUnit.ToString()+" jednostke: "+((Unit) sender).ToString());
			
			BlockUIInput();
			selectedUnit.Select();
		}
	}
	
	private void SelectUnit(Unit unit)
	{
		if (selectedUnit != null) {
			selectedUnit.Deselect();
		}
		
		selectedUnit = unit;
		selectedUnit.Select();
	}
	
	public void RestartGame()
	{
		throw new NotImplementedException();
	}
	
	private void BlockUIInput()
	{
		ui.AttackButton.enabled = false;
		ui.MoveButton.enabled = false;
		ui.SpecialAbilityButton.enabled = false;
	}
	
	private void UnBlockUIInput()
	{
		ui.AttackButton.enabled = true;
		ui.MoveButton.enabled = true;
		ui.SpecialAbilityButton.enabled = true;
	}
	
	private void ShowSelectedUnitInfo()
	{
		throw new NotImplementedException();
	}
	
	private void SpecialAbilityButtonClickedHandler(object sender, EventArgs args)
	{
		throw new NotImplementedException();
	}
	
	public void CancelUsingSpecialAbility()
	{
		throw new NotImplementedException();
	}
	
	public void UsingSpecialAbilityEnded()
	{
		throw new NotImplementedException();
	}
	
	private bool CheckEndGameConditions()
	{
		throw new NotImplementedException();
	}
	
	private bool DisplayWiner()
	{
		throw new NotImplementedException();
	}
	
	public void AttackEnded()
	{
		isAtacking = false;
		UnBlockUIInput();
	}
	
	public void MovementEnded()
	{
		isMoving = false;
		UnBlockUIInput();
	}
	
	public void EndTurnButtonClickedHandler(object sender, EventArgs args)
	{
		isAtacking = false;
		isMoving = false;
		selectedUnit.Deselect();
		selectedUnit = null;
		Debug.Log("End of tour button clicked");
	}
	
	private void AttackButtonClickedHandler(object sender, EventArgs args)
	{
		isAtacking = true;
		isMoving = false;
		if (selectedUnit != null) selectedUnit.SelectAttack();
	}
	
	private void MoveButtonClickedHandler(object sender, EventArgs args)
	{
		isMoving = true;
		isAtacking = false;
		if (selectedUnit != null) selectedUnit.SelectMovement();
	}
	
	private void TerrainClickedHandler(object sender, PositionEventArgs args) //PositionArgs args)
	{
		if(isMoving)
		{
			selectedUnit.MoveToPosition(args.Position,MovementEnded);
			BlockUIInput();
			selectedUnit.Select();
		} else {
			selectedUnit.Deselect();
			selectedUnit = null;
		}
	}


}


