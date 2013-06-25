using System;

/// <summary>
/// Abstrct base class of every game state.
/// </summary>
public abstract class GameState
{
    /// <summary>
    /// Parent game controller.
    /// </summary>
    protected GameController parent;

    /// <summary>
    /// Previous game state. Null if no previous state.
    /// </summary>
    protected GameState previous;

    /// <summary>
    /// Gets level scene name associated with current state.
    /// </summary>
    public virtual string LevelName
    {
        get { return string.Empty; }
    }

    /// <summary>
    /// Gets level additiv indicator.
    /// Returns true when level should be loaded additively, otherwise false.
    /// Defaults to false.
    /// </summary>
    public virtual bool IsLevelAdditive
    {
        get { return false; }
    }

    /// <summary>
    /// Gets reload level in state enter indicator.
    /// Returns true when level scene should be reloaded each time when entering current state, otherwise false.
    /// Defaults to false.
    /// </summary>
    public virtual bool ReloadOnEnter
    {
        get { return false; }
    }

    /// <summary>
    /// Initializes game state base properties.
    /// </summary>
    /// <param name="parent">Parent game controller.</param>
    /// <param name="previous">Previous game state.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when parent is null.</exception>
    protected GameState(GameController parent, GameState previous)
    {
        if (parent == null) throw new ArgumentNullException("parent");
        this.parent = parent;
        this.previous = previous;
    }

    /// <summary>
    /// Returns initial state machine state.
    /// </summary>
    /// <param name="controller">Parent game controller.</param>
    /// <returns>Initial state.</returns>
    public static GameState InitialState(GameController controller)
    {
        return ResolveState(GameStateEnum.MainMenu, controller, null);
    }

    /// <summary>
    /// Returns new state machine state.
    /// </summary>
    /// <param name="nextState">Requested state identifier.</param>
    /// <param name="previous">Previous game state.</param>
    /// <returns></returns>
    public GameState SwitchState(GameStateEnum nextState, GameState previous)
    {
        return ResolveState(nextState, parent, previous);
    }

    /// <summary>
    /// Retirns previous game state.
    /// </summary>
    /// <returns>Previously loaded game state.</returns>
    public GameState PreviousState()
    {
        if (previous == null) throw new InvalidOperationException("No previous state supported");
        return previous;
    }

    /// <summary>
    /// State enter event.
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// State exit event.
    /// </summary>
    public virtual void Exit() { }

    /// <summary>
    /// Escape key clicked event.
    /// </summary>
    public virtual void OnEscape() { }

    private static GameState ResolveState(GameStateEnum state, GameController controller, GameState previous)
    {
        GameState result;
        switch (state)
        {
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
}

/// <summary>
/// Represents enumeration of each game state identifier.
/// </summary>
public enum GameStateEnum
{
    MainMenu, MapSelection, InGame, HighScores, Help, PlaceUnits
}
