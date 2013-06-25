using System;
using UnityEngine;

public class Artillery : Unit
{
	private bool canUse = true;
	private const float radius = 30.0f;

    public override void Attack(Unit enemy)
    {
        throw new NotSupportedException();
    }

    public override void Attack(Vector3 target)
    {
        if (!isBusy && CanAttack(target))
        {
            --AttackStatistics.RemainingQuantity;
            StartCoroutine(ProcessAttack(target));
        }
    }
	
	/// <summary>
	/// Uses the special ability which is attack all enemies into sphere with half attack value.
	/// </summary>
	/// <param name='position'>
	/// Position where use clicked.
	/// </param>
    public void UseSpecial(Vector3 position)
    {
		if(canUse)
		{
			Collider[] colliders = Physics.OverlapSphere(position, radius);
			for(int i = 0 ; i < colliders.Length ; ++i)
			{
				Unit u = colliders[i].GetComponent<Unit>();
				if(u && u.PlayerOwner != this.PlayerOwner)
				{
					u.GetDamadge(AttackStatistics.Power / 2.0f, this);
				}
			}
			canUse = false;
		}
    }  
 
	public override void EndTurn ()
	{
        base.EndTurn();
		canUse = true;
	}

	public override void SelectSpecialAbility ()
	{
		if(canUse)
			SelectRange(SelectionMode.SpecialAbility, 0.0f);
		else
			SelectRange(SelectionMode.NoAction, 0.0f);
	}
}
