using System;
using UnityEngine;


//nie ma na razie klasy GameState i pochodnych wiec rzucam same wyjatki poki co
public class GameController
{
	
	public GameState CurrentGameState { get; set; }
	
	
	public void GoToMainMenuState()
	{
		CurrentGameState = CurrentGameState.SwitchState(GameStateEnum.MainMenu);
	}
	
	public void GoToMapSelectionState()
	{
		CurrentGameState = CurrentGameState.SwitchState(GameStateEnum.MapSelection);
	}
	
	public void GoToInGameState()
	{
		CurrentGameState = CurrentGameState.SwitchState(GameStateEnum.InGame);
	}	
	
	public void GoToHighScoreState()	
	{
		CurrentGameState = CurrentGameState.SwitchState(GameStateEnum.HighScores);
	}

	public void GoToHelpState()	
	{
		CurrentGameState = CurrentGameState.SwitchState(GameStateEnum.Help);
	}	
	
	public void GoToPlaceUnitsState()
	{
		CurrentGameState = CurrentGameState.SwitchState(GameStateEnum.PlaceUnits);
	}
}


