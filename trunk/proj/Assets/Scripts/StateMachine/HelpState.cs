using System;
using System.Collections.Generic;
using UnityEngine;

public class HelpState : GameState {
	
	private HelpUI ui;
	private UILogic selectedItem;
	private UnitSelection unitSelection;
	
	public HelpState (GameController controller) : base(controller) { }
	
	protected override void Enter ()
	{
		base.Enter ();
		GameManager manager = GameManager.Instance();
		ui = manager.GameUIInstance.HelpUIInstance;
		if (unitSelection == null) unitSelection = new UnitSelection(ui.UnitsTabPane);
		InferSelectedItem();
		AttachEventHandlers();
		ui.Show();
	}
	
	protected override void Exit ()
	{
		ui.Hide();
		DetachEventHandlers();
		if (selectedItem == ui.UnitsTabPane) {
			unitSelection.Exit();
		}
		
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
			unitSelection.Enter();
		} else {
			selectedItem = ui.GoalTabPane;
			selectedItem.gameObject.SetActive(true);
		}
	}
	
	private void SelectItem (UILogic item) {
		if (selectedItem != item) {
			if (selectedItem == ui.UnitsTabPane) {
				unitSelection.Exit();
			}
			selectedItem.gameObject.SetActive(false);
			selectedItem = item;
			selectedItem.gameObject.SetActive(true);
			if (selectedItem == ui.UnitsTabPane) {
				unitSelection.Enter();
			}
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
	
	#region Unit Model List
	private class UnitSelection {
		private HelpUnitsUI ui;
		private int index;
		private List<UILogic> tabPanes;
		private UnitModelSelector selector;
		
		public UnitSelection (HelpUnitsUI ui) {
			this.ui = ui;
			this.tabPanes = new List<UILogic> {
				ui.IfvTabPane,
				ui.TankTabPane,
				ui.HeavyTankTabPane,
				ui.HelicopterTabPane,
				ui.ArtilleryTabPane
			};
		}
		
		public void Enter () {
			InstantiateSelector();
			int index = InferSelection();
			if (index < 0) index = 0;
			Select(index);
			AttachEventHandlers();
			ui.Show();
			selector.Show();
			selector.SelectModel(index);
		}
		
		public void Exit () {
			selector.Hide();
			ui.Hide();
			DetachEventHandlers();
			GameObject.Destroy(selector);
		}
		
		private int InferSelection () {
			int result = 0;
			foreach (var item in tabPanes) {
				if (item.gameObject.activeSelf) {
					return result;
				} else {
					result++;
				}
			}
			
			return -1;
		}
		
		private void InstantiateSelector () {
			selector = (UnitModelSelector)GameObject.Instantiate(
				ui.UnitModelListPrefab,
				ui.UnitModelListPrefab.transform.position,
				ui.UnitModelListPrefab.transform.rotation);
		}
		
		private void Select(int index) {
			if (this.index != index) {
				tabPanes[this.index].gameObject.SetActive(false);
				this.index = index % tabPanes.Count;
				tabPanes[this.index].gameObject.SetActive(true);
			}
		}
		
		private void NextItem () {
			selector.NextModel();
			Select(selector.GetModel());
		}
		
		private void PrevItem () {
			selector.PrevModel();
			Select(selector.GetModel());
		}
		
		private void AttachEventHandlers () {
			ui.NextUnitButton.ButtonClicked += NextUnitButtonClicked;
			ui.PrevUnitButton.ButtonClicked += PrevUnitButtonClicked;
		}
		
		private void DetachEventHandlers () {
			ui.PrevUnitButton.ButtonClicked -= PrevUnitButtonClicked;
			ui.NextUnitButton.ButtonClicked -= NextUnitButtonClicked;
		}
		
		private void NextUnitButtonClicked(object sender, EventArgs args) {
			NextItem();
		}
		
		private void PrevUnitButtonClicked(object sender, EventArgs args) {
			PrevItem();
		}
	}
	#endregion
}
