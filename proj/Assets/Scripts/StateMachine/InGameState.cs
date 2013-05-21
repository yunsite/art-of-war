using System;
using UnityEngine;


public class InGameState : GameState
{
	public InGameState (GameController controller) : base (controller) { }
	
	private void ShowInGameMenu()
	{
		throw new NotImplementedException();
	}
	
	private void HideInGameMenu()
	{
		throw new NotImplementedException();
	}
	
	private void UnitClickedEventHandler (object sender, EventArgs args)
	{
		throw new NotImplementedException();
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
		throw new NotImplementedException();
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
	
	public bool AttackEnded()
	{
		throw new NotImplementedException();
	}
	
	public bool MovementEnded()
	{
		throw new NotImplementedException();
	}
	
	public void EndTurnButtonClickedHandler(object sender, EventArgs args)
	{
		throw new NotImplementedException();
	}
}


