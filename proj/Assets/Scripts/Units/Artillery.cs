using System;
using UnityEngine;

/// <summary>
/// Represents artillery type of unit.
/// </summary>
public class Artillery : Unit
{
	private bool canUse = true;
	private const float radius = 30.0f;

    /// <summary>
    /// Not supported unit attack type.
    /// </summary>
    /// <param name="enemy">Enemy unit target.</param>
    public override void Attack(Unit enemy)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Processes attack field attack on target point.
    /// </summary>
    /// <param name="target">Target point.</param>
    public override void Attack(Vector3 target)
    {
        if (CanAttack(target))
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
 
    /// <summary>
    /// Resets per turn statistics.
    /// </summary>
	public override void EndTurn ()
	{
        base.EndTurn();
		canUse = true;
	}

    /// <summary>
    /// Selects unit for special ability.
    /// </summary>
	public override void SelectSpecialAbility ()
	{
		if(canUse)
			SelectRange(SelectionMode.SpecialAbility, 0.0f);
		else
			SelectRange(SelectionMode.NoAction, 0.0f);
	}
}
