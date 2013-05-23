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
		if (Application.loadedLevelName != "HillyTestScene") 
			Application.LoadLevel("HillyTestScene");
		GameManager manager = GameManager.Instance();
		ui = manager.GameUIInstance.InGameUIInstance;
		ui.EndTurnButton.ButtonClicked += EndTurnButtonClickedHandler;
		ui.AttackButton.ButtonClicked += AttackButtonClickedHandler;
		ui.MoveButton.ButtonClicked += MoveButtonClickedHandler;
		ui.Show();
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
			selectedUnit = (Unit) sender;
		}
		else if(!isAtacking && selectedUnit != (Unit) sender)
		{
			selectedUnit = (Unit) sender;
		}
		else if(isAtacking && selectedUnit != null)
		{
			selectedUnit.Attack((Unit) sender);
			BlockUIInput();
		}
	}
	
	private void SelectUnit(Unit unit)
	{
		throw new NotImplementedException();
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
	
	private void EmptySpaceClickedHandler(Vector3 worldPosition)
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
		UnBlockUIInput();
	}
	
	public void MovementEnded()
	{
		UnBlockUIInput();
	}
	
	public void EndTurnButtonClickedHandler(object sender, EventArgs args)
	{
		isAtacking = false;
		isMoving = false;
		selectedUnit = null;
		Debug.Log("End of tour button clicked");
	}
	
	private void AttackButtonClickedHandler(object sender, EventArgs args)
	{
		isAtacking = true;
		isMoving = false;
	}
	
	private void MoveButtonClickedHandler(object sender, EventArgs args)
	{
		isMoving = true;
		isAtacking = false;
	}
	
	private void TerrainClickedHandler(object sender, EventArgs args) //PositionArgs args)
	{
		if(isMoving)
		{
			//selectedUnit.MoveToPosition(args.position);
			BlockUIInput();
		}
	}
}


