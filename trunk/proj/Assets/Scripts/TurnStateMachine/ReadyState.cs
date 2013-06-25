using System;
using UnityEngine;

/// <summary>
/// Ready for unit selection state of single player turn state machine.
/// Supports unit selected event and ignores terrain position selected event.
/// Suppresses unit action events.
/// </summary>
public class ReadyState : ActionsDisabledState
{
    /// <summary>
    /// Creates ready state.
    /// </summary>
    /// <param name="ui">UI reference.</param>
    /// <param name="player">Player info reference.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when any parameter is null.</exception>
    public ReadyState(InGameUI ui, PlayerInfo player) : base(ui, player) { }

    /// <summary>
    /// State entry behaviour, called in case of in-transition occurrence.
    /// </summary>
    /// <remarks>
    /// Resets unit statistics.
    /// </remarks>
    public override void Enter()
    {
        base.Enter();
        ResetStats();
    }

    private void ResetStats()
    {
        string line = "-----";
        ui.UnitHP.text = line;
        ui.UnitHP.MarkAsChanged();
        ui.UnitMovementPoints.text = line;
        ui.UnitMovementPoints.MarkAsChanged();
        ui.UnitStats.text = line;
        ui.UnitStats.MarkAsChanged();
    }

    #region Events
    /// <summary>
    /// Unit selected event behaviour.
    /// </summary>
    /// <remarks>
    /// Returns selected state if selected unit belongs to current player, otherwise ignores this event.
    /// </remarks>
    /// <param name="unit">Unit being selected.</param>
    /// <returns>New state of single player turn state machine.</returns>
    public override TurnState UnitSelected(Unit unit)
    {
        if (unit.PlayerOwner == player.Index)
        {
            return new SelectedState(ui, player, unit);
        }
        else
        {
            return this;
        }
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
    #endregion
}
