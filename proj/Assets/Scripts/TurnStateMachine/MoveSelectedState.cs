using UnityEngine;

/// <summary>
/// Unit movement selection state of single player turn state machine.
/// Supports terrain position selected event for move capable units.
/// Supports all other events shipped with SelectedState.
/// </summary>
public class MoveSelectedState : SelectedState
{
    internal MoveSelectedState(InGameUI ui, PlayerInfo player, Unit unit) : base(ui, player, unit) { }

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

    #region Helpers
    private bool IsTargetVisible(Vector3 target)
    {
        BoxCollider collider = unit.GetComponent<BoxCollider>();
        Vector3 position = unit.transform.position + collider.center;
        Vector3 direction = target + Vector3.up - position;
        return !Physics.Raycast(position, direction.normalized, direction.magnitude);
    }
    #endregion
}
