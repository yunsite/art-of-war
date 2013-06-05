using System;
using UnityEngine;

public class GameController : MonoBehaviour {
	
	public GameState CurrentGameState { get; private set; }
	
	void Start () {
		CurrentGameState = GameState.InitialState(this);
	}
	
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


