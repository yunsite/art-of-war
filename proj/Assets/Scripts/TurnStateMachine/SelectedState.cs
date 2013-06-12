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
    }

    public override void Exit()
    {
        unit.Deselect();
        base.Exit();
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
                throw new NotImplementedException();
            case UnitTypeEnum.Tank:
                throw new NotImplementedException();
            case UnitTypeEnum.HeavyTank:
                // Niema potrzeby tworzyć osobnego stanu dla ciężkiego czołgu,
                // bo jego umiejętność specjalna nie wymaga żadnych argumentów.
                HeavyTank heayTank = (HeavyTank)unit;
                heayTank.UseSpecial();
                // Jeżeli umiejętność specjalna będzie uruchamiać animację trzeba przejść do stanu:
                // return new ActionExecutionState(ui, player, unit);
                return new ReadyState(ui, player);
            case UnitTypeEnum.Helicopter:
                throw new NotImplementedException();
            case UnitTypeEnum.Artillery:
                throw new NotImplementedException();
            default:
                throw new InvalidProgramException("Unreacheable code path.");
        }
	}
    #endregion
}
