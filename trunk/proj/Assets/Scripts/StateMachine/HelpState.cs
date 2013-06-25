using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game state representing help view.
/// </summary>
public class HelpState : GameState {
	
	private HelpUI ui;
	private UILogic selectedItem;
	private UnitSelection unitSelection;
	
    /// <summary>
    /// Creates help game state instance.
    /// </summary>
    /// <param name="parent">Parent game controller.</param>
    /// <param name="previous">Previous game state.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when parent is null.</exception>
	public HelpState (GameController controller, GameState previous) : base(controller, previous) { }

    /// <summary>
    /// State enter event. Initializes state internal variables and prepares view elements.
    /// </summary>
	public override void Enter ()
	{
		base.Enter ();
		GameManager manager = GameManager.Instance();
		ui = manager.GameUIInstance.HelpUIInstance;
		if (unitSelection == null) unitSelection = new UnitSelection(ui.UnitsTabPane);
		InferSelectedItem();
		AttachEventHandlers();
		ui.Show();
	}

    /// <summary>
    /// State exit event. Hides view elements.
    /// </summary>
	public override void Exit ()
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
        parent.GoToPreviousState();
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
		private List<HelpUnitDescriptionUI> tabPanes;
		private UnitModelSelector selector;
		
		// Alignment for ButtonsFont 42
		private const string unitDescriptionFormat = 
				"Health Points:                                              {0}" +
				"\nDeffence:                                                      {1}" +
				"\nAttack Area:                                                 {2}" +
				"\nAttack Power:                                              {3}" +
				"\nAttack Range:                                              {4}" +
				"\nAttack Quantity:                                           {5}" +
				"\nMovement Range:                                        {6}" +
				"\nDifficult Terrain Movement Ability:              {7}" +
				"\nSpecial Ability: \n    {8}";
		
		public UnitSelection (HelpUnitsUI ui) {
			this.ui = ui;
			this.tabPanes = new List<HelpUnitDescriptionUI> {
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
			selector.Show();
			selector.SelectModel(index);
			ApplyUnitInfo();
			ui.Show();
		}
		
		public void Exit () {
			ui.Hide();
			selector.Hide();
			DetachEventHandlers();
			GameObject.Destroy(selector.gameObject);
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
				tabPanes[this.index].Hide();
				this.index = index % tabPanes.Count;
				tabPanes[this.index].Show();
			}
		}
		
		private void NextItem () {
			selector.NextModel();
			Select(selector.GetModel());
			ApplyUnitInfo();
		}
		
		private void PrevItem () {
			selector.PrevModel();
			Select(selector.GetModel());
			ApplyUnitInfo();
		}
		
		private void ApplyUnitInfo () {
			Unit unit = selector.GetUnit();
			if (unit != null) {
				string description = string.Format(
					unitDescriptionFormat,
					unit.HealthStatistics.TotalPoints,
					unit.HealthStatistics.Deffence,
					unit.AttackStatistics.Area,
					unit.AttackStatistics.Power,
					unit.AttackStatistics.Range,
					unit.AttackStatistics.TotalQuantity,
					unit.MovementStatistics.TotalRange,
					unit.MovementStatistics.DifficultTerrainMoveAbility,
					GetUnitSpecialAbilityDescription(unit.UnitType));
				UILabel content = this.tabPanes[index].ContentLabel;
				content.text = description;
				content.MarkAsChanged();
			} else {
				Debug.LogWarning("No unit type information available.");
			}
		}

		string GetUnitSpecialAbilityDescription (UnitTypeEnum unitType)
		{
			string result;
			switch (unitType) {
			case UnitTypeEnum.IFV:
				result = "Gas - two times larger movement range in one turn.";
				break;
			case UnitTypeEnum.Tank:
				result = "Break - special projectile damaging each vehicle on trajectory.";
				break;
			case UnitTypeEnum.HeavyTank:
				result = "Fortification - Increases deffence to 200% for 2 turns taking all movement quantity.";
				break;
			case UnitTypeEnum.Helicopter:
				result = "Valkiria - attacks 2 to 4 targets with power of 100% dvided between all of it.";
				break;
			case UnitTypeEnum.Artillery:
				result = "Raid - each target inside attack range receives 50% regular damage.";
				break;
			default:
				throw new InvalidProgramException("Unreacheable code.");
			}
			
			return result;
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
