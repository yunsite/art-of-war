using UnityEngine;

/// <summary>
/// Unit area attack selection state of single player turn state machine.
/// Supports unit selected event for attack enemy capable units.
/// Supports all other events shipped with SelectedState.
/// </summary>
public class UnitAttackSelectedState : SelectedState
{
    internal UnitAttackSelectedState(InGameUI ui, PlayerInfo player, Unit unit) : base(ui, player, unit) { }

    public override void Enter()
    {
        base.Enter();
        unit.SelectAttack();
    }

    #region Events
    public override TurnState UnitSelected(Unit enemy)
    {
        if (enemy.PlayerOwner == player.Index)
        {
            return base.UnitSelected(enemy);
        }
        else if (unit.CanAttack(enemy.transform.position))
        {
            unit.Attack(enemy);
            Debug.Log("Atakuje jednostka zaznaczona: " + unit + " jednostkê: " + enemy);
            return new ActionExecutionState(ui, player, unit);
        }
        else
        {
            return this;
        }
    }
    #endregion
}
