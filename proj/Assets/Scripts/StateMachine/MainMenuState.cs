using System;
using UnityEngine;

public class MainMenuState : GameState {
	
	private MainMenuUI ui;
	
	public MainMenuState(GameController controller) : base (controller) {
		GameManager manager = GameManager.Instance();
		ui = manager.GameUIInstance.MainMenuUIInstance;
		ui.StartButton.ButtonClicked += StartButtonClickedHandler;
		ui.ScoreButton.ButtonClicked += HighScoresButtonClickedHandler;
		ui.HelpButton.ButtonClicked += HelpButtonClickedHandler;
		ui.ExitButton.ButtonClicked += ExitButtonClickedHandler;
		ui.Show ();
	}
	
	private void StartButtonClickedHandler(object sender, EventArgs args) {
		Debug.Log("Start button clicked");
		ui.Hide();
		parent.GoToInGameState();
	}
	
	private void HighScoresButtonClickedHandler(object sender, EventArgs args) {
		Debug.Log("High Scores button clicked");
	}
	
	private void HelpButtonClickedHandler(object sender, EventArgs args) {
		Debug.Log("Help button clicked");
	}
	
	private void ExitButtonClickedHandler(object sender, EventArgs args) {
		Debug.Log("Exit button clicked");
	}
}
