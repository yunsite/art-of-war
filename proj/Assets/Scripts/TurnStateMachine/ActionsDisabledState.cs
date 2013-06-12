using System;

/// <summary>
/// Abstract base class for any action events suppressing state of single player turn state machine.
/// Disables unit action buttons while state execution.
/// </summary>
public abstract class ActionsDisabledState : TurnState
{
    protected ActionsDisabledState(InGameUI ui, PlayerInfo player) : base(ui, player) { }

    public override void Enter()
    {
        base.Enter();
        DisableActionButtons();
    }

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