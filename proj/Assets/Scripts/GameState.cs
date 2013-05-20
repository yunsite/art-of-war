using System;
using UnityEngine;

//wiem ze nie w mojej kwestii byla implementacja tego ale potrzebniwalem chociaz prototypu (Marek Kokot)
public abstract class GameState
{
	private GameController parent;

	public GameState SwitchState(GameStateEnum nextState)
	{
		if(nextState.Equals(GameStateEnum.Help)) {
			return null;//(GameState) new HelpState();
		}
		if(nextState.Equals(GameStateEnum.HighScores)) {
			return null;//(GameState) new HighScoreState();
		}
		if(nextState.Equals(GameStateEnum.InGame)) {
			return null;//(GameState) new InGameState();
		}
		if(nextState.Equals(GameStateEnum.MainMenu)) {
			return null;//(GameState) new MainMenuState();
		}
		if(nextState.Equals(GameStateEnum.MapSelection)) {
			return null;//(GameState) new MapSelectionState();
		}
		if(nextState.Equals(GameStateEnum.PlaceUnits)) {
			return null;//(GameState) new PlaceUnitState();
		}
		return null;
	}
}


