using UnityEngine;

public class TankSpecialAttackSelectedState : SelectedState
{
	protected Tank tank;
	
    internal TankSpecialAttackSelectedState(InGameUI ui, PlayerInfo player, Unit unit) : base(ui, player, unit)
	{
		tank = (Tank)unit;
	}
	
	
    public override void Enter()
    {
        base.Enter();
        unit.SelectSpecialAbility();
    }

    #region Events
	public override TurnState TerrainPositionSelected (Vector3 position)
	{
		tank.UseSpecial(position);
		return new ActionExecutionState(ui, player, unit);
	}
    public override TurnState UnitSelected(Unit enemy)
    {	
        if (enemy.PlayerOwner == player.Index)
        {
            return base.UnitSelected(enemy);
        }
		tank.UseSpecial(enemy.transform.position);
        return new ActionExecutionState(ui, player, unit);
    }
    #endregion
}


