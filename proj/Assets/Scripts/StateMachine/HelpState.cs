using System;

public class HelpState : GameState {
	
	private HelpUI ui;
	private UILogic selectedItem;
	
	public HelpState (GameController controller) : base(controller) { }
	
	protected override void Enter ()
	{
		base.Enter ();
		GameManager manager = GameManager.Instance();
		ui = manager.GameUIInstance.HelpUIInstance;
		InferSelectedItem();
		AttachEventHandlers();
		ui.Show();
	}
	
	protected override void Exit ()
	{
		ui.Hide();
		DetachEventHandlers();
		selectedItem = null;
		ui = null;
		base.Exit ();
	}
	
	#region Selection
	private void InferSelectedItem () {
		if (ui.GoalTabPane.gameObject.activeSelf) {
			selectedItem = ui.GoalTabPane;
		} else if (ui.ControlTabPane.gameObject.activeSelf) {
			selectedItem = ui.ControlTabPane;
		} else if (ui.TurnsTabPane.gameObject.activeSelf) {
			selectedItem = ui.TurnsTabPane;
		} else if (ui.UnitsTabPane.gameObject.activeSelf) {
			selectedItem = ui.UnitsTabPane;
		} else {
			selectedItem = ui.GoalTabPane;
			selectedItem.gameObject.SetActive(true);
		}
	}
	
	private void SelectItem (UILogic item) {
		if (selectedItem != item) {
			selectedItem.gameObject.SetActive(false);
			selectedItem = item;
			selectedItem.gameObject.SetActive(true);
		}
	}
	#endregion
	
	#region Event Handlers
	private void AttachEventHandlers () {
		ui.BackButton.ButtonClicked	+= BackButtonClicked;
		ui.GoalTabButton.ButtonClicked += GoalTabButtonClicked;
		ui.ControlTabButton.ButtonClicked += ControlTabButtonClicked;
		ui.TurnsTabButton.ButtonClicked += TurnsTabButtonClicked;
		ui.UnitsTabButton.ButtonClicked += UnitsTabButtonClicked;
	}
	
	private void DetachEventHandlers () {
		ui.UnitsTabButton.ButtonClicked -= UnitsTabButtonClicked;
		ui.TurnsTabButton.ButtonClicked -= TurnsTabButtonClicked;
		ui.ControlTabButton.ButtonClicked -= ControlTabButtonClicked;
		ui.GoalTabButton.ButtonClicked -= GoalTabButtonClicked;
		ui.BackButton.ButtonClicked -= BackButtonClicked;
	}
	
	private void BackButtonClicked(object sender, EventArgs args) {
		parent.GoToMainMenuState();
	}
	
	private void GoalTabButtonClicked(object sender, EventArgs args) {
		SelectItem(ui.GoalTabPane);
	}
	
	private void ControlTabButtonClicked(object sender, EventArgs args) {
		SelectItem(ui.ControlTabPane);
	}
	
	private void TurnsTabButtonClicked(object sender, EventArgs args) {
		SelectItem(ui.TurnsTabPane);
	}
	
	private void UnitsTabButtonClicked(object sender, EventArgs args) {
		SelectItem(ui.UnitsTabPane);
	}
	#endregion
}
