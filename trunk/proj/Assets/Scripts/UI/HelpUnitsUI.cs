/// <summary>
/// Contains units description control in help view.
/// </summary>
public class HelpUnitsUI : UILogic
{
    /// <summary>
    /// References to unit model selector utility.
    /// </summary>
    public UnitModelSelector UnitModelListPrefab;

    /// <summary>
    /// IFV description pane.
    /// </summary>
    public HelpUnitDescriptionUI IfvTabPane;

    /// <summary>
    /// Tank description pane.
    /// </summary>
    public HelpUnitDescriptionUI TankTabPane;

    /// <summary>
    /// Heavy Tank description pane.
    /// </summary>
    public HelpUnitDescriptionUI HeavyTankTabPane;

    /// <summary>
    /// Helicopter description pane.
    /// </summary>
    public HelpUnitDescriptionUI HelicopterTabPane;

    /// <summary>
    /// Artillery description pane.
    /// </summary>
    public HelpUnitDescriptionUI ArtilleryTabPane;

    /// <summary>
    /// Previous unit description button.
    /// </summary>
    public ButtonLogic PrevUnitButton;

    /// <summary>
    /// Next unit description button.
    /// </summary>
    public ButtonLogic NextUnitButton;
}

