/// <summary>
/// Contains in game ui controls.
/// </summary>
public class InGameUI : UILogic
{
    #region Turn controls
    /// <summary>
    /// Turn info label.
    /// </summary>
    public UILabel TurnInfo;

    /// <summary>
    /// End of turn button.
    /// </summary>
    public ButtonLogic EndTurnButton;
    #endregion

    #region Unit controls
    /// <summary>
    /// Unit movememnt points label.
    /// </summary>
    public UILabel UnitMovementPoints;

    /// <summary>
    /// Unit statistics label.
    /// </summary>
    public UILabel UnitStats;

    /// <summary>
    /// Unit health points label.
    /// </summary>
    public UILabel UnitHP;

    /// <summary>
    /// Unit move ability button.
    /// </summary>
    public ButtonLogic MoveButton;

    /// <summary>
    /// Unit attack ability button.
    /// </summary>
    public ButtonLogic AttackButton;

    /// <summary>
    /// Unit special ability button.
    /// </summary>
    public ButtonLogic SpecialAbilityButton;
    #endregion

    #region In game menu controls
    /// <summary>
    /// In game menu controls.
    /// </summary>
    public InGameMenuUI InGameMenuUIInstance;
    #endregion
}
