using System;

/// <summary>
/// Abstract base class for any action events suppressing state of single player turn state machine.
/// Disables unit action buttons while state execution.
/// </summary>
public abstract class ActionsDisabledState : TurnState
{
    /// <summary>
    /// Initializes actions disabled state base variables.
    /// </summary>
    /// <param name="ui">UI reference.</param>
    /// <param name="player">Player info reference.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when any parameter is null.</exception>
    protected ActionsDisabledState(InGameUI ui, PlayerInfo player) : base(ui, player) { }

    /// <summary>
    /// State entry behaviour, called in case of in-transition occurrence.
    /// </summary>
    /// <remarks>
    /// Disables action UI controls.
    /// </remarks>
    public override void Enter()
    {
        base.Enter();
        DisableActionButtons();
    }

    /// <summary>
    /// State exit behaviour, called in case of out-transition occurrence.
    /// </summary>
    /// <remarks>
    /// Enables action UI controls.
    /// </remarks>
    public override void Exit()
    {
        EnableActionButtons();
        base.Exit();
    }

    #region UI control management
    private void DisableActionButtons()
    {
        ui.MoveButton.Disable();
        ui.AttackButton.Disable();
        ui.SpecialAbilityButton.Disable();
    }

    private void EnableActionButtons()
    {
        ui.SpecialAbilityButton.Enable();
        ui.AttackButton.Enable();
        ui.MoveButton.Enable();
    }
    #endregion
}