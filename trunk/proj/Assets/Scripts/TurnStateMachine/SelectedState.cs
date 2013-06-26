using System;
using UnityEngine;

/// <summary>
/// Basic unit selection state of single player turn state machine.
/// Selects specified unit.
/// Support action events, another unit selection event and terrain position selecion event.
/// Ignores same unit selecton event.
/// </summary>
public class SelectedState : TurnState
{
    /// <summary>
    /// Selected unit.
    /// </summary>
    protected Unit unit;

    /// <summary>
    /// Creates selected unit state.
    /// </summary>
    /// <param name="ui">UI reference.</param>
    /// <param name="player">Player info reference.</param>
    /// <param name="unit">Selected unit.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when any parameter is null.</exception>
    protected internal SelectedState(InGameUI ui, PlayerInfo player, Unit unit)
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
    /// Marks specified unit as selected.
    /// Shows unit statistics.
    /// </remarks>
    public override void Enter()
    {
        base.Enter();
        unit.Select();
        Debug.Log("Zmieniam jednostkę zaznaczoną na " + unit);
        ShowStats();
    }

    /// <summary>
    /// State exit behaviour, called in case of out-transition occurrence.
    /// </summary>
    /// <remarks>
    /// Deselects unit.
    /// </remarks>
    public override void Exit()
    {
        unit.Deselect();
        base.Exit();
    }

    private void ShowStats()
    {
        ui.UnitHP.text = string.Format(
            "HP: {0:N0}%", (float)unit.HealthStatistics.RemainingPoints * 100f / (float)unit.HealthStatistics.TotalPoints);
        ui.UnitHP.MarkAsChanged();
        ui.UnitMovementPoints.text = string.Format(
            "Move: {0:N1}", unit.MovementStatistics.RemainingRange);
        ui.UnitMovementPoints.MarkAsChanged();
        ui.UnitStats.text = string.Format(
            "Attacks: {0}", unit.AttackStatistics.RemainingQuantity);
        ui.UnitStats.MarkAsChanged();
    }

    #region Events
    /// <summary>
    /// Unit selected event behaviour.
    /// </summary>
    /// <remarks>
    /// Returns selected state if another players unit is clicked, otherwise ignores this event.
    /// </remarks>
    /// <param name="unit">Unit being selected.</param>
    /// <returns>New state of single player turn state machine.</returns>
    public override TurnState UnitSelected(Unit unit)
    {
        if (unit != this.unit && unit.PlayerOwner == player.Index)
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
    /// Returns ready state.
    /// </remarks>
    /// <param name="unit">Terrain position being selected.</param>
    /// <returns>New state of single player turn state machine.</returns>
    public override TurnState TerrainPositionSelected(Vector3 position)
    {
        return new ReadyState(ui, player);
    }

    /// <summary>
    /// Unit move action selected event behaviour.
    /// </summary>
    /// <remarks>
    /// Returns move action selection state for selected unit.
    /// </remarks>
    /// <returns>New state of single player turn state machine.</returns>
    public override TurnState MoveActionSelected()
    {
        return new MoveSelectedState(ui, player, unit);
    }

    /// <summary>
    /// Unit attack action selected event behaviour.
    /// </summary>
    /// <remarks>
    /// Returns unit or field attack selection state for selected unit.
    /// </remarks>
    /// <returns>New state of single player turn state machine.</returns>
    public override TurnState AttackActionSelected()
    {
        switch (unit.AttackStatistics.Area)
        {
            case AttackAreaEnum.Unit:
                return new UnitAttackSelectedState(ui, player, unit);
            case AttackAreaEnum.Field:
                return new FieldAttackSelectedState(ui, player, unit); 
            default:
                throw new InvalidProgramException("Unreacheable code path.");
        }
    }

    /// <summary>
    /// Unit special action selected event behaviour.
    /// </summary>
    /// <remarks>
    /// Returns special ability selection state for specified type of unit
    /// or executes special ability if no animation specified.
    /// </remarks>
    /// <returns>New state of single player turn state machine.</returns>
	public override TurnState SpecialActionSelected ()
	{
        switch (unit.UnitType)
        {
            case UnitTypeEnum.IFV:
                IFVUnit ifv = (IFVUnit)unit;
				ifv.UseSpecial();
				return new ReadyState(ui, player);
            case UnitTypeEnum.Tank:
                return new TankSpecialAttackSelectedState(ui, player, unit); 
            case UnitTypeEnum.HeavyTank:
                HeavyTank heayTank = (HeavyTank)unit;
                heayTank.UseSpecial();
                return new ReadyState(ui, player);
            case UnitTypeEnum.Helicopter:
                return new HelicopterSpecialAttackSelectedState(ui, player, unit);
            case UnitTypeEnum.Artillery:
                return new ArtillerySpecialAttackSelectedState(ui, player, unit);
            default:
                throw new InvalidProgramException("Unreacheable code path.");
        }
	}
    #endregion

    #region Helpers
    /// <summary>
    /// Checks if specified target point is visible by currently selected unit.
    /// </summary>
    /// <param name="target">Target point.</param>
    /// <returns>Target visibility.</returns>
    protected bool IsTargetVisible(Vector3 target)
    {
        BoxCollider collider = unit.GetComponent<BoxCollider>();
        Vector3 position = unit.transform.position + collider.center;
        Vector3 direction = target + Vector3.up - position;
        return !Physics.Raycast(position, direction.normalized, direction.magnitude);
    }

    /// <summary>
    /// Checks if specified unit is visible by currently selected unit.
    /// </summary>
    /// <param name="target">Target unit.</param>
    /// <returns>Target visibility.</returns>
    protected bool IsTargetVisible(Unit target)
    {
        BoxCollider collider = unit.GetComponent<BoxCollider>();
        BoxCollider targetCollider = target.GetComponent<BoxCollider>();
        Vector3 position = unit.transform.position + collider.center;
        Vector3 direction = target.transform.position + targetCollider.center - position;
        RaycastHit hit;
        if (Physics.Raycast(position, direction.normalized, out hit, direction.magnitude))
        {
            return hit.collider == targetCollider;
        }
        else
        {
            return true;
        }
    }
    #endregion
}
