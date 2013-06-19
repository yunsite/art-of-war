using UnityEngine;

public class ArtillerySpecialAttackSelectedState : SelectedState
{
	protected Artillery artillery;
	
    internal ArtillerySpecialAttackSelectedState(InGameUI ui, PlayerInfo player, Unit unit) : base(ui, player, unit)
	{
		artillery = (Artillery)unit;
	}
	
	
    public override void Enter()
    {
        //base.Enter();
        unit.SelectSpecialAbility();
    }

    #region Events
	public override TurnState TerrainPositionSelected (Vector3 position)
	{
		artillery.UseSpecial(position);
		return new SelectedState(ui, player, unit);
	}
    public override TurnState UnitSelected(Unit enemy)
    {	
        if (enemy.PlayerOwner == player.Index)
        {
            return base.UnitSelected(enemy);
        }
		artillery.UseSpecial(enemy.transform.position);
        return new SelectedState(ui, player, unit);
    }
    #endregion
}




