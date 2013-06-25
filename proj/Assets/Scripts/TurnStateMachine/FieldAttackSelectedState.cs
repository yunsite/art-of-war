using UnityEngine;

/// <summary>
/// Field area attack selection state of single player turn state machine.
/// Supports terrain position selected event for attack field area capable units.
/// Supports all other events shipped with SelectedState.
/// </summary>
public class FieldAttackSelectedState : SelectedState
{
    /// <summary>
    /// Creater field area attack selection state.
    /// </summary>
    /// <param name="ui">UI reference.</param>
    /// <param name="player">Player info reference.</param>
    /// <param name="unit">Selected unit.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when any parameter is null.</exception>
    internal FieldAttackSelectedState(InGameUI ui, PlayerInfo player, Unit unit) : base(ui, player, unit) { }

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
    /// Terrain position selected event behaviour.
    /// </summary>
    /// <remarks>
    /// Triggers attack and returns action execution state if attack is posible,
    /// otherwise returns ready state.
    /// </remarks>
    /// <param name="unit">Terrain position being selected.</param>
    /// <returns>New state of single player turn state machine.</returns>
    public override TurnState TerrainPositionSelected(Vector3 position)
    {
        if (unit.CanAttack(position))
        {
            if (IsTargetVisible(position))
            {
                unit.Attack(position);
                Debug.Log("Atakuje jednostka zaznaczona: " + unit + " obszar o środku w: " + position);
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
