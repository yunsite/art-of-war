using System;
using UnityEngine;

public class InGameState : GameState
{
	
	private InGameUI ui;
    private TurnState turn;

    public InGameState(GameController controller) : base(controller) { }

    protected override void Enter()
    {
        base.Enter();
        LoadLevelAsync("MainGameScene", LevelLoadedHandler);
        GameManager manager = GameManager.Instance();
        ui = manager.GameUIInstance.InGameUIInstance;
        AttachUiEventHandlers();
        turn = new ReadyState(ui);
        turn.Enter();
        ui.Show();
    }

    protected override void Exit()
    {
        ui.Hide();
        turn.Exit();
        turn = null;
        DetachUiEventHandlers();
        base.Exit();
    }

    private void LoadLevelAsync(string levelName, EventHandler callback)
    {
        GameManager manager = GameManager.Instance();
        if (Application.loadedLevelName != levelName)
        {
            manager.LevelLoadedEvent += callback;
            Application.LoadLevel(levelName);
        }
    }

    private void AttachUiEventHandlers()
    {
        ui.EndTurnButton.ButtonClicked += EndTurnButtonClickedHandler;
        ui.AttackButton.ButtonClicked += AttackButtonClickedHandler;
        ui.MoveButton.ButtonClicked += MoveButtonClickedHandler;
		ui.SpecialAbilityButton.ButtonClicked += SpecialAbilityButtonClickedHandler;
    }

    private void DetachUiEventHandlers()
    {
        ui.MoveButton.ButtonClicked -= MoveButtonClickedHandler;
        ui.AttackButton.ButtonClicked -= AttackButtonClickedHandler;
        ui.EndTurnButton.ButtonClicked -= EndTurnButtonClickedHandler;
		ui.SpecialAbilityButton.ButtonClicked -= SpecialAbilityButtonClickedHandler;
    }

	private void LevelLoadedHandler(object sender, EventArgs args)
	{
		var units = (Unit[])UnityEngine.Object.FindObjectsOfType (typeof(Unit));
		foreach(var unit in units)
		{
			unit.Clicked += UnitClickedEventHandler;
            unit.ActionCompleted += UnitActionCompletedEventHandler;
		}
		var positionObserver =(PositionObserver) UnityEngine.Object.FindObjectOfType (typeof(PositionObserver));
		positionObserver.Clicked += TerrainClickedHandler;

		GameManager manager = GameManager.Instance();
		manager.LevelLoadedEvent -= LevelLoadedHandler;
	}

	private void ShowInGameMenu()
	{
		ui.Show();
	}
	
	private void HideInGameMenu()
	{
		ui.Hide();
	}

    public void RestartGame()
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
	
	public void EndTurnButtonClickedHandler(object sender, EventArgs args)
	{
        Debug.Log("End of tour button clicked");
		var AllUnits = GameObject.FindObjectsOfType(typeof(Unit)) as Unit[];
		foreach(var unit in AllUnits)
		{
			unit.EndTour();
		}
		return;
        //throw new NotImplementedException();
	}

    #region Turn state management
    private void UnitClickedEventHandler(object sender, EventArgs args)
    {
        Unit clickedUnit = (Unit)sender;
        SwitchTurnState(turn.UnitSelected(clickedUnit));
    }

    private void TerrainClickedHandler(object sender, PositionEventArgs args)
    {
        SwitchTurnState(turn.TerrainPositionSelected(args.Position));
    }

    private void MoveButtonClickedHandler(object sender, EventArgs args)
    {
        SwitchTurnState(turn.MoveActionSelected());
    }

    private void UnitActionCompletedEventHandler(object sender, EventArgs e)
    {
        SwitchTurnState(turn.ActionCompleted());
    }

    private void AttackButtonClickedHandler(object sender, EventArgs args)
    {
        SwitchTurnState(turn.SpecialActionSelected());
    }

    private void SpecialAbilityButtonClickedHandler(object sender, EventArgs args)
    {
        SwitchTurnState(turn.SpecialActionSelected());
    }

    private void SwitchTurnState(TurnState turnState)
    {
        if (turnState != turn)
        {
            turn.Exit();
            turn = turnState;
            turn.Enter();
        }
    }
    #endregion
}


