using System;
using UnityEngine;
using System.Collections.Generic;
public class Helicopter : Unit
{
	private bool canUse = true;
    public void UseSpecial(List<Unit> unitsToAttack)
    {
		if(canUse)
		{			
			float attackValue = AttackStatistics.Power / unitsToAttack.Count;
			foreach(Unit u in unitsToAttack)
			{
				if(u.PlayerOwner != this.PlayerOwner)
				{
					u.GetDamadge(attackValue, this);
				}
			}
			canUse = false;
		}
    }   
	public override void EndTour ()
	{
		canUse = true;
	}
	public bool CanUseSpecial()
	{
		return canUse;
	}
	public override void SelectSpecialAbility ()
	{
		if(canUse)
			SelectRange(SelectionMode.SpecialAbility, 0.0f);
		else
			SelectRange(SelectionMode.NoAction, 0.0f);
	}
}
