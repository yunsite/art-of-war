using UnityEngine;

/// <summary>
/// Contains entire game user interface controls.
/// </summary>
public class GameUI : MonoBehaviour
{
    /// <summary>
    /// Main menu view controls.
    /// </summary>
    public MainMenuUI MainMenuUIInstance;

    /// <summary>
    /// Map selection view controls.
    /// </summary>
    public MapSelectionUI MapSelectionUIInstance;

    /// <summary>
    /// In game view controls.
    /// </summary>
    public InGameUI InGameUIInstance;

    /// <summary>
    /// Help view controls.
    /// </summary>
    public HelpUI HelpUIInstance;
}

