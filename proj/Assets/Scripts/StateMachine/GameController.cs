using System.Collections;
using UnityEngine;

/// <summary>
/// Entire game controller responsible for game state management and scene management.
/// </summary>
public class GameController : MonoBehaviour {

    private bool isLoaded = false;

    /// <summary>
    /// Gets current game state instance.
    /// </summary>
	public GameState CurrentGameState { get; private set; }
	
    /// <summary>
    /// Switches current game state to main menu state.
    /// </summary>
	public void GoToMainMenuState()
	{
        StartCoroutine(SwitchState(GameStateEnum.MainMenu));
	}

    /// <summary>
    /// Switches current game state to map selection state.
    /// </summary>
    /// <remarks>
    /// Map selection state is not implemented yet.
    /// </remarks>
	public void GoToMapSelectionState()
	{
        StartCoroutine(SwitchState(GameStateEnum.MapSelection));
	}

    /// <summary>
    /// Switches current game state to in game state.
    /// </summary>
	public void GoToInGameState()
	{
        StartCoroutine(SwitchState(GameStateEnum.InGame));
	}

    /// <summary>
    /// Switches current game state to high score state.
    /// </summary>
    /// <remarks>
    /// High score state is not implemented yet.
    /// </remarks>
	public void GoToHighScoreState()	
	{
        StartCoroutine(SwitchState(GameStateEnum.HighScores));
	}

    /// <summary>
    /// Switches current game state to help state.
    /// </summary>
	public void GoToHelpState()	
	{
        StartCoroutine(SwitchState(GameStateEnum.Help));
	}

    /// <summary>
    /// Switches current game state to place unit state.
    /// </summary>
    /// <remarks>
    /// Place unit state is not implemented yet.
    /// </remarks>
	public void GoToPlaceUnitsState()
	{
        StartCoroutine(SwitchState(GameStateEnum.PlaceUnits));
	}

    /// <summary>
    /// Switches current game state to previous game state.
    /// </summary>
    public void GoToPreviousState()
    {
        StartCoroutine(PreviousState());
    }

    IEnumerator Start()
    {
        CurrentGameState = GameState.InitialState(this);
        yield return StartCoroutine(LoadScene());
        CurrentGameState.Enter();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CurrentGameState.OnEscape();
        }
    }

    IEnumerator SwitchState(GameStateEnum state)
    {
        CurrentGameState.Exit();
        CurrentGameState = CurrentGameState.SwitchState(state, CurrentGameState);
        yield return StartCoroutine(LoadScene());
        CurrentGameState.Enter();
    }

    IEnumerator PreviousState()
    {
        CurrentGameState.Exit();
        CurrentGameState = CurrentGameState.PreviousState();
        yield return StartCoroutine(LoadScene());
        CurrentGameState.Enter();
    }

    IEnumerator LoadScene()
    {
        string levelName = CurrentGameState.LevelName;
        if (!string.IsNullOrEmpty(levelName)
            && (Application.loadedLevelName != levelName || CurrentGameState.ReloadOnEnter))
        {
            isLoaded = false;
            if (CurrentGameState.IsLevelAdditive)
            {
                Application.LoadLevelAdditive(levelName);
            }
            else
            {
                Application.LoadLevel(levelName);
            }

            while (!isLoaded) yield return null;
        }
    }

    void OnLevelWasLoaded(int index)
    {
        isLoaded = true;
    }
}


