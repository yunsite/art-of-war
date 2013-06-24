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
    protected GameState previous;
    
    public virtual string LevelName
    {
        get { return string.Empty; }
    }
    public virtual bool IsLevelAdditive
    {
        get { return false; }
    }
	
	protected GameState(GameController parent, GameState previous) {
		if (parent == null) throw new ArgumentNullException("parent");
		this.parent = parent;
        this.previous = previous;
	}

    public static GameState InitialState(GameController controller)
    {
        return ResolveState(GameStateEnum.MainMenu, controller, null);
    }
	
	public GameState SwitchState(GameStateEnum nextState, GameState previous)
	{
		return ResolveState(nextState, parent, previous);
	}

    public GameState PreviousState()
    {
        if (previous == null) throw new InvalidOperationException("No previous state supported");
        return previous;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
	
	private static GameState ResolveState(GameStateEnum state, GameController controller, GameState previous) {
		GameState result;
		switch (state) {
		case GameStateEnum.Help:
			result = new HelpState(controller, previous);
			break;
		case GameStateEnum.HighScores:
			throw new NotImplementedException();
		case GameStateEnum.InGame:
			result = new InGameState(controller, previous);
			break;
		case GameStateEnum.MainMenu:
			result = new MainMenuState(controller, previous);
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

    public virtual void OnEscape() { }
}


