using System;
using UnityEngine;
using System.Collections.Generic;

public class Tank : Unit
{
	private bool canUse = true;
	private class collider_unit : IComparable
	{
		public Unit unit;
		public float distance;
		public int CompareTo(object obj)
		{
			collider_unit u = (collider_unit)obj;
			if(this.distance < u.distance) return -1;
			return 1;
		}
	}
    public void UseSpecial(Vector3 position)
    {
		if(canUse)
		{
			position.y = transform.position.y;
			//Debug.DrawLine(position, transform.position, Color.red, 5000.0f);
			//Debug.DrawRay(transform.position, position - transform.position, Color.green, 500.0f);
	 		RaycastHit[] hits;
	        hits = Physics.RaycastAll(transform.position,( position - transform.position).normalized, Mathf.Infinity);
	        int i = 0;
			
			List<collider_unit> hitUnits = new List<collider_unit>();
	        while (i < hits.Length) 
			{
	            RaycastHit hit = hits[i];
				Unit u = hit.collider.GetComponent<Unit>();
				if(u && u.PlayerOwner != this.PlayerOwner)
				{
					hitUnits.Add(new collider_unit
					{
						distance = Vector3.Distance(transform.position, u.transform.position), 
						unit = u
					});
				}
	            i++;
				
	        }
			hitUnits.Sort();
			float currentAttack = AttackStatistics.Power;
			foreach(var u in hitUnits)
			{
				u.unit.GetDamadge(currentAttack, this);
				currentAttack /= 2.0f;
			}
			canUse = false;
		}
		Deselect();
		//SelectRange(SelectionMode.NoAction, 0.0f);
		OnActionCompleted();
    }   
	public override void EndTour ()
	{
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
