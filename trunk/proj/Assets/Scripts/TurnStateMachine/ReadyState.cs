using System;
using UnityEngine;

/// <summary>
/// Ready for unit selection state of single player turn state machine.
/// Supports unit selected event and ignores terrain position selected event.
/// Suppresses unit action events.
/// </summary>
public class ReadyState : ActionsDisabledState
{
    public ReadyState(InGameUI ui, PlayerInfo player) : base(ui, player) { }

    #region Events
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

    public override TurnState TerrainPositionSelected(Vector3 position)
    {
        return this;
    }
    #endregion
}
