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
    protected InGameUI ui;
    protected Unit unit;

    protected internal SelectedState(InGameUI ui, Unit unit)
    {
        if (ui == null)
        {
            throw new ArgumentNullException("ui");
        }

        if (unit == null)
        {
            throw new ArgumentNullException("enemy");
        }

        this.ui = ui;
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
        if (unit == this.unit)
        {
            return this;
        }
        else
        {
            return new SelectedState(ui, unit);
        }
    }

    public override TurnState TerrainPositionSelected(Vector3 position)
    {
        return new ReadyState(ui);
    }

    public override TurnState MoveActionSelected()
    {
        return new MoveSelectedState(ui, unit);
    }

    public override TurnState AttackActionSelected()
    {
        switch (unit.AttackStatistics.Area)
        {
            case AttackAreaEnum.Unit:
                return new UnitAttackSelectedState(ui, unit);
            case AttackAreaEnum.Field:
                return new FieldAttackSelectedState(ui, unit); 
            default:
                throw new InvalidProgramException("Unreacheable code path.");
        }
    }
	
	public override TurnState SpecialActionSelected ()
	{
		return new SpecialActionSelectedState(ui, unit);
	}
    #endregion
}
