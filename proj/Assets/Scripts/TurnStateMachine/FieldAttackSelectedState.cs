using UnityEngine;

/// <summary>
/// Field area attack selection state of single player turn state machine.
/// Supports terrain position selected event for attack field area capable units.
/// Supports all other events shipped with SelectedState.
/// </summary>
public class FieldAttackSelectedState : SelectedState
{
    internal FieldAttackSelectedState(InGameUI ui, PlayerInfo player, Unit unit) : base(ui, player, unit) { }

    public override void Enter()
    {
        base.Enter();
        unit.SelectAttack();
    }

    #region Events
    public override TurnState TerrainPositionSelected(Vector3 position)
    {
        if (unit.CanAttack(position))
        {
            unit.Attack(position);
            Debug.Log("Atakuje jednostka zaznaczona: " + unit + " obszar o œrodku w: " + position);
            return new ActionExecutionState(ui, player, unit);
        }
        else
        {
            return base.TerrainPositionSelected(position);
        }
    }
    #endregion
}
