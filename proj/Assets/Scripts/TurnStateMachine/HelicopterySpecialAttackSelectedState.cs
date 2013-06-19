using UnityEngine;
using System.Collections.Generic;

public class HelicopterSpecialAttackSelectedState : SelectedState
{
	protected Helicopter helicopter;
	private List<Unit> unitsToAttack = new List<Unit>();
    internal HelicopterSpecialAttackSelectedState(InGameUI ui, PlayerInfo player, Unit unit) : base(ui, player, unit)
	{
		helicopter = (Helicopter)unit;
	}
	
	
    public override void Enter()
    {
        //base.Enter();
        unit.SelectSpecialAbility();
    }

    #region Events

    public override TurnState UnitSelected(Unit enemy)
    {			
		if(helicopter.CanUseSpecial())
		{
	        if (enemy.PlayerOwner != player.Index)
	        {
				enemy.SelectAsTargetForHelicopterSpecial();
				unitsToAttack.Add(enemy);
				if(unitsToAttack.Count == 4)
				{
					DeselectAllInList();
					helicopter.UseSpecial(unitsToAttack);
					return new SelectedState(ui, player, unit);
				}
				return this;
	        }
			DeselectAllInList();
			return new SelectedState(ui, player, enemy);
		}
        return this;
    }
	
	public override TurnState SpecialActionSelected ()
	{
		DeselectAllInList();
		if(unitsToAttack.Count >= 2)
		{
			
			helicopter.UseSpecial(unitsToAttack);
			return new SelectedState(ui, player, unit);
		}
		unitsToAttack.Clear();
		return this;
	}
    #endregion
	
	private void DeselectAllInList()
	{
		foreach(Unit u in unitsToAttack)
			u.Deselect();
	}
}




