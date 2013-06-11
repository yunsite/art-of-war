using UnityEngine;

/// <summary>
/// Unit movement selection state of single player turn state machine.
/// Supports terrain position selected event for move capable units.
/// Supports all other events shipped with SelectedState.
/// </summary>
public class MoveSelectedState : SelectedState
{
    internal MoveSelectedState(InGameUI ui, Unit unit) : base(ui, unit) { }

    public override void Enter()
    {
        base.Enter();
        unit.SelectMovement();
    }

    #region Events
    public override TurnState TerrainPositionSelected(Vector3 position)
    {
        if (unit.CanMove(position))
        {
            unit.MoveToPosition(position);
            return new ActionExecutionState(ui, unit);
        }
        else
        {
            return base.TerrainPositionSelected(position);
        }
    }
    #endregion
}
