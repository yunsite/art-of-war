using UnityEngine;

/// <summary>
/// Unit movement selection state of single player turn state machine.
/// Supports terrain position selected event for move capable units.
/// Supports all other events shipped with SelectedState.
/// </summary>
public class MoveSelectedState : SelectedState
{
    /// <summary>
    /// Creates move selected state.
    /// </summary>
    /// <param name="ui">UI reference.</param>
    /// <param name="player">Player info reference.</param>
    /// <param name="unit">Selected unit.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when any parameter is null.</exception>
    internal MoveSelectedState(InGameUI ui, PlayerInfo player, Unit unit) : base(ui, player, unit) { }

    /// <summary>
    /// State entry behaviour, called in case of in-transition occurrence.
    /// </summary>
    /// <remarks>
    /// Marks specified unit as selected for movement and displays range.
    /// Shows unit statistics.
    /// </remarks>
    public override void Enter()
    {
        base.Enter();
        unit.SelectMovement();
    }

    #region Events
    /// <summary>
    /// Terrain position selected event behaviour.
    /// </summary>
    /// <remarks>
    /// Triggers movement and returns action execution state if unit can move,
    /// therwise ignores that event.
    /// </remarks>
    /// <param name="unit">Terrain position being selected.</param>
    /// <returns>New state of single player turn state machine.</returns>
    public override TurnState TerrainPositionSelected(Vector3 position)
    {
        position.y = unit.transform.position.y;
        if (unit.CanMove(position))
        {
            if (IsTargetVisible(position))
            {
                unit.MoveToPosition(position);
                return new ActionExecutionState(ui, player, unit);
            }
            else
            {
                return this;
            }
        }
        else
        {
            return base.TerrainPositionSelected(position);
        }
    }
    #endregion
}
