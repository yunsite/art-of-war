using System;

// wiem ze nie w mojej kwestii byla implementacja tego ale potrzebniwalem chociaz prototypu (Marek Kokot)
// dodałem swoje zmiany potrzebne do implementacji i przetestowania MainMenuState (Zbigniew Prasak)
// Zmieniłem koncepcje przełączania stanów tak że wywoływane są metody Enter i Exit.
// Dodatkowo tworzenie stanów przeniosłem do metody statycznej ResolveState, której implementacja może
// umożliwiać różne polityki czasu życia stanów (obecnie stan jest porzucany gdy nie jest już potrzebny,
// ale łatwo jest zaimplementować słownik stanów w takim podejściu) (Zbigniew Prasak).
public abstract class GameState
{
	protected GameController parent;
	
	protected GameState(GameController parent) {
		if (parent == null) throw new ArgumentNullException("parent");
		this.parent = parent;
	}
	
	public static GameState InitialState (GameController controller) {
		GameState result = ResolveState(GameStateEnum.MainMenu, controller);
		result.Enter();
		return result;
	}
	
	public GameState SwitchState(GameStateEnum nextState)
	{
		this.Exit();
		GameState result = ResolveState(nextState, parent);
		result.Enter();
		return result;
	}
	
	protected virtual void Enter () { }
	protected virtual void Exit () { }
	
	private static GameState ResolveState(GameStateEnum state, GameController controller) {
		GameState result;
		switch (state) {
		case GameStateEnum.Help:
			result = new HelpState(controller);
			break;
		case GameStateEnum.HighScores:
			throw new NotImplementedException();
		case GameStateEnum.InGame:
			result = new InGameState(controller);
			break;
		case GameStateEnum.MainMenu:
			result = new MainMenuState(controller);
			break;
		case GameStateEnum.MapSelection:
			throw new NotImplementedException();
		case GameStateEnum.PlaceUnits:
			throw new NotImplementedException();
		default:
			throw new InvalidProgramException("Unreacheable path.");
		}
		
		return result;
	}
}


