using System;
using UnityEngine;

public class InGameState : GameState
{
    private int currentPlayer; // 0 - Player1; 1 - Plyer2;
	private InGameUI ui;
    private MapManager mapManager;
    private TurnState turn;

    public InGameState(GameController controller) : base(controller) { }

    protected override void Enter()
    {
        base.Enter();
        LoadLevelAsync("MainGameScene", LevelLoadedHandler);
        GameManager manager = GameManager.Instance();
        ui = manager.GameUIInstance.InGameUIInstance;
        AttachUiEventHandlers();
        ui.Show();
    }

    protected override void Exit()
    {
        ui.Hide();
        turn.Exit();
        turn = null;
        DetachMapEventHandlers();
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

    private void LevelLoadedHandler(object sender, EventArgs args)
    {
        GameManager manager = GameManager.Instance();
        mapManager = manager.MapManagerInstance;
        PrepareCameras();
        AttachMapEventHandlers();
        currentPlayer = 0;
        BeginTurn();
        manager.LevelLoadedEvent -= LevelLoadedHandler;
    }

    private void PrepareCameras()
    {
        foreach (PlayerInfo player in mapManager.Players)
        {
            player.MainCamera.gameObject.SetActive(false);
            player.MinimapCamera.gameObject.SetActive(false);
        }
    }

    #region Event handlers
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

    private void AttachMapEventHandlers()
    {
        mapManager.Observer.Clicked += TerrainClickedHandler;
        foreach (PlayerInfo player in mapManager.Players)
        {
            foreach (Unit unit in player.Units)
            {
                unit.Clicked += UnitClickedEventHandler;
                unit.ActionCompleted += UnitActionCompletedEventHandler;
            }
        }
    }

    private void DetachMapEventHandlers()
    {
        foreach (PlayerInfo player in mapManager.Players)
        {
            foreach (Unit unit in player.Units)
            {
                unit.ActionCompleted -= UnitActionCompletedEventHandler;
                unit.Clicked -= UnitClickedEventHandler;
            }
        }

        mapManager.Observer.Clicked -= TerrainClickedHandler;
    }
    #endregion

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

    #region Turns management
    private bool CheckEndGameConditions()
	{
		throw new NotImplementedException();
	}
	
	private bool DisplayWiner()
	{
		throw new NotImplementedException();
	}

    private void EndTurnButtonClickedHandler(object sender, EventArgs args)
    {
        Debug.Log("End of turn button clicked");
        EndTurn();
        currentPlayer = (currentPlayer + 1) % mapManager.Players.Length;
        BeginTurn();
    }

    private void BeginTurn()
    {
        PlayerInfo player = mapManager.Players[currentPlayer];
        foreach (Unit unit in player.Units)
        {
            unit.BeginTurn();
        }

        turn = new ReadyState(ui, player);
        turn.Enter();
        player.MainCamera.gameObject.SetActive(true);
        player.MinimapCamera.gameObject.SetActive(true);
    }

    private void EndTurn()
    {
        PlayerInfo player = mapManager.Players[currentPlayer];
        player.MinimapCamera.gameObject.SetActive(false);
        player.MainCamera.gameObject.SetActive(false);
        turn.Exit();
        foreach (Unit unit in player.Units)
        {
            unit.EndTurn();
        }
    }
    #endregion

    #region Single player turn state management
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

    private void AttackButtonClickedHandler(object sender, EventArgs args)
    {
        SwitchTurnState(turn.AttackActionSelected());
    }

    private void SpecialAbilityButtonClickedHandler(object sender, EventArgs args)
    {
        SwitchTurnState(turn.SpecialActionSelected());
    }

    private void UnitActionCompletedEventHandler(object sender, EventArgs e)
    {
        SwitchTurnState(turn.ActionCompleted());
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