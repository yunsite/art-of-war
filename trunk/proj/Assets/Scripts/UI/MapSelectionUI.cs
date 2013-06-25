/// <summary>
/// Contains map selection controls.
/// </summary>
public class MapSelectionUI : UILogic
{
    /// <summary>
    /// Previous map image.
    /// </summary>
    public UISprite PrevMapImage;

    /// <summary>
    /// Currently selected map image.
    /// </summary>
    public UISprite CurrMapImage;

    /// <summary>
    /// Next map image.
    /// </summary>
    public UISprite NextMapImage;

    /// <summary>
    /// Player 1 name label.
    /// </summary>
    public UIInput Player1Name;

    /// <summary>
    /// Player 2 name label.
    /// </summary>
    public UIInput Player2Name;

    /// <summary>
    /// Back to previous view button.
    /// </summary>
    public ButtonLogic BackButton;

    /// <summary>
    /// Go to next view button.
    /// </summary>
    public ButtonLogic NextButton;

    /// <summary>
    /// Next map button.
    /// </summary>
    public ButtonLogic NextMapButton;

    /// <summary>
    /// Previous map button.
    /// </summary>
    public ButtonLogic PrevMapButton;
}
