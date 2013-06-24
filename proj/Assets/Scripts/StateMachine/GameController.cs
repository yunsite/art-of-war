using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour {

    private bool isLoaded = false;
	public GameState CurrentGameState { get; private set; }
	
	IEnumerator Start () {
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
	
	public void GoToMainMenuState()
	{
        StartCoroutine(SwitchState(GameStateEnum.MainMenu));
	}
	
	public void GoToMapSelectionState()
	{
        StartCoroutine(SwitchState(GameStateEnum.MapSelection));
	}
	
	public void GoToInGameState()
	{
        StartCoroutine(SwitchState(GameStateEnum.InGame));
	}	
	
	public void GoToHighScoreState()	
	{
        StartCoroutine(SwitchState(GameStateEnum.HighScores));
	}

	public void GoToHelpState()	
	{
        StartCoroutine(SwitchState(GameStateEnum.Help));
	}	
	
	public void GoToPlaceUnitsState()
	{
        StartCoroutine(SwitchState(GameStateEnum.PlaceUnits));
	}

    public void GoToPreviousState()
    {
        StartCoroutine(PreviousState());
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
        if (!string.IsNullOrEmpty(levelName) && Application.loadedLevelName != levelName)
        {
            isLoaded = false;
            if (CurrentGameState.IsLevelAdditive)
            {
                // Application.LoadLevelAdditiveAsync tylko w Unity Pro :(
                Application.LoadLevelAdditive(levelName);
            }
            else
            {
                // Application.LoadLevelAsync tylko w Unity Pro :(
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


