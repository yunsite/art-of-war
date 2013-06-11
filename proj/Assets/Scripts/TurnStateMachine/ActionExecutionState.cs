using System;
using UnityEngine;

/// <summary>
/// Unit action execution state of single player turn state machine.
/// Ignores unit selected event and terrain position selected event.
/// Supports action completed event.
/// Suppresses unit action events.
/// </summary>
public class ActionExecutionState : ActionsDisabledState
{
    private Unit unit;

    internal ActionExecutionState(InGameUI ui, Unit unit)
        : base(ui)
    {
        if (unit == null)
        {
            throw new ArgumentNullException("enemy");
        }

        this.unit = unit;
    }

    public override void Enter()
    {
        base.Enter();
        ui.EndTurnButton.Disable();
    }

    public override void Exit()
    {
        ui.EndTurnButton.Enable();
        base.Exit();
    }

    #region Events
    public override TurnState UnitSelected(Unit unit)
    {
        return this;
    }

    public override TurnState TerrainPositionSelected(Vector3 position)
    {
        return this;
    }

    public override TurnState ActionCompleted()
    {
        return new SelectedState(ui, unit);
    }
    #endregion
}
