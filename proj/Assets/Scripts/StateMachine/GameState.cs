using System;

// wiem ze nie w mojej kwestii byla implementacja tego ale potrzebniwalem chociaz prototypu (Marek Kokot)
// dodałem swoje zmiany potrzebne do implementacji i przetestowania MainMenuState (Zbigniew Prasak)
public abstract class GameState
{
	protected GameController parent;
	
	protected GameState(GameController parent) {
		if (parent == null) throw new ArgumentNullException("parent");
		this.parent = parent;
	}

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
			return new MainMenuState(parent);
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


