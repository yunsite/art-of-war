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

    /// <summary>
    /// Creates action execution state.
    /// </summary>
    /// <param name="ui">UI reference.</param>
    /// <param name="player">Player info reference.</param>
    /// <param name="unit">Unit carrying out action.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when any parameter is null.</exception>
    internal ActionExecutionState(InGameUI ui, PlayerInfo player, Unit unit)
        : base(ui, player)
    {
        if (unit == null)
        {
            throw new ArgumentNullException("unit");
        }

        this.unit = unit;
    }

    /// <summary>
    /// State entry behaviour, called in case of in-transition occurrence.
    /// </summary>
    /// <remarks>
    /// Disables action UI controls and end of turn button.
    /// </remarks>
    public override void Enter()
    {
        base.Enter();
        ui.EndTurnButton.Disable();
    }

    /// <summary>
    /// State exit behaviour, called in case of out-transition occurrence.
    /// </summary>
    /// <remarks>
    /// Enables action UI controls and end of turn button.
    /// </remarks>
    public override void Exit()
    {
        ui.EndTurnButton.Enable();
        base.Exit();
    }

    #region Events
    /// <summary>
    /// Unit selected event behaviour.
    /// </summary>
    /// <remarks>
    /// Ignores this event.
    /// </remarks>
    /// <param name="unit">Unit being selected.</param>
    /// <returns>New state of single player turn state machine.</returns>
    public override TurnState UnitSelected(Unit unit)
    {
        return this;
    }

    /// <summary>
    /// Terrain position selected event behaviour.
    /// </summary>
    /// <remarks>
    /// Ignores this event.
    /// </remarks>
    /// <param name="unit">Terrain position being selected.</param>
    /// <returns>New state of single player turn state machine.</returns>
    public override TurnState TerrainPositionSelected(Vector3 position)
    {
        return this;
    }

    /// <summary>
    /// Unit action completed event behaviour.
    /// </summary>
    /// <remarks>
    /// Return selection state for current unit.
    /// </remarks>
    /// <returns>New state of single player turn state machine.</returns>
    public override TurnState ActionCompleted()
    {
        return new SelectedState(ui, player, unit);
    }
    #endregion
}
