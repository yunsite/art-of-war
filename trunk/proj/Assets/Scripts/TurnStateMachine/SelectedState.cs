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
    protected Unit unit;

    protected internal SelectedState(InGameUI ui, PlayerInfo player, Unit unit)
        : base(ui, player)
    {
        if (unit == null)
        {
            throw new ArgumentNullException("enemy");
        }

        this.unit = unit;
    }

    public override void Enter()
    {
        base.Enter();
        unit.Select();
        Debug.Log("Zmieniam jednostkę zaznaczoną na " + unit);
        ShowStats();
    }

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

    public override TurnState TerrainPositionSelected(Vector3 position)
    {
        return new ReadyState(ui, player);
    }

    public override TurnState MoveActionSelected()
    {
        return new MoveSelectedState(ui, player, unit);
    }

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
                // Niema potrzeby tworzyć osobnego stanu dla ciężkiego czołgu,
                // bo jego umiejętność specjalna nie wymaga żadnych argumentów.
                HeavyTank heayTank = (HeavyTank)unit;
                heayTank.UseSpecial();
                // Jeżeli umiejętność specjalna będzie uruchamiać animację trzeba przejść do stanu:
                // return new ActionExecutionState(ui, player, unit);
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
}
