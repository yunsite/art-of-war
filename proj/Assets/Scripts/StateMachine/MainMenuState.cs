using System;
using UnityEngine;

public class MainMenuState : GameState {
	
	private MainMenuUI ui;
	
	public MainMenuState(GameController controller) : base (controller) { }
	
	protected override void Enter ()
	{
		base.Enter ();
		GameManager manager = GameManager.Instance();
		ui = manager.GameUIInstance.MainMenuUIInstance;
		AttachEventHandlers();
		ui.Show();
	}
	
	protected override void Exit ()
	{
		ui.Hide();
		DetachEventHandlers();
		base.Exit ();
	}
	
	#region Event Handlers
	private void AttachEventHandlers () {
		ui.StartButton.ButtonClicked += StartButtonClickedHandler;
		ui.ScoreButton.ButtonClicked += HighScoresButtonClickedHandler;
		ui.HelpButton.ButtonClicked += HelpButtonClickedHandler;
		ui.ExitButton.ButtonClicked += ExitButtonClickedHandler;
	}
	
	private void DetachEventHandlers () {
		ui.ExitButton.ButtonClicked -= ExitButtonClickedHandler;
		ui.HelpButton.ButtonClicked -= HelpButtonClickedHandler;
		ui.ScoreButton.ButtonClicked -= HighScoresButtonClickedHandler;
		ui.StartButton.ButtonClicked -= StartButtonClickedHandler;
	}
	
	private void StartButtonClickedHandler(object sender, EventArgs args) {
		Debug.Log("Start button clicked");
		parent.GoToInGameState();
	}
	
	private void HighScoresButtonClickedHandler(object sender, EventArgs args) {
		Debug.Log("High Scores button clicked");
	}
	
	private void HelpButtonClickedHandler(object sender, EventArgs args) {
		Debug.Log("Help button clicked");
		parent.GoToHelpState();
	}
	
	private void ExitButtonClickedHandler(object sender, EventArgs args) {
		Debug.Log("Exit button clicked");
	}
	#endregion
}
