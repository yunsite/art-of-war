using UnityEngine;

/// <summary>
/// Unit area attack selection state of single player turn state machine.
/// Supports unit selected event for attack enemy capable units.
/// Supports all other events shipped with SelectedState.
/// </summary>
public class UnitAttackSelectedState : SelectedState
{
    /// <summary>
    /// Creater unit area attack selection state.
    /// </summary>
    /// <param name="ui">UI reference.</param>
    /// <param name="player">Player info reference.</param>
    /// <param name="unit">Selected unit.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when any parameter is null.</exception>
    internal UnitAttackSelectedState(InGameUI ui, PlayerInfo player, Unit unit) : base(ui, player, unit) { }

    /// <summary>
    /// State entry behaviour, called in case of in-transition occurrence.
    /// </summary>
    /// <remarks>
    /// Marks selected unit to attack.
    /// </remarks>
    public override void Enter()
    {
        base.Enter();
        unit.SelectAttack();
    }

    #region Events
    /// <summary>
    /// Unit selected event behaviour.
    /// </summary>
    /// <remarks>
    /// Triggers attack and returns action execution state if attack is posible,
    /// returns selected state if same player unit was selected,
    /// otherwise returns ready state.
    /// </remarks>
    /// <param name="unit">Unit being selected.</param>
    /// <returns>New state of single player turn state machine.</returns>
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
