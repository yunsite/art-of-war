public class InGameUI : UILogic {
	
	#region Turn controls
	public UILabel TurnInfo;
	public ButtonLogic EndTurnButton;
	#endregion
	
	#region Unit controls
	public UILabel UnitMovementPoints;
	public UILabel UnitStats;
	public UILabel UnitHP;
	public ButtonLogic MoveButton;
	public ButtonLogic AttackButton;
	public ButtonLogic SpecialAbilityButton;
	#endregion
	
	#region In game menu controls
	public ButtonLogic ResumeButton;
	public ButtonLogic RestartButton;
	public ButtonLogic MainMenuButton;
	public ButtonLogic HelpButton;
	public ButtonLogic QuitButton;
	#endregion

}
