using System;
using UnityEngine;

public class Tank : Unit
{
    public void UseSpecial(Vector3 position)
    {
		position.y = transform.position.y;
 		RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, position, 10000.0F);
        int i = 0;
		float currentAttack = AttackStatistics.Power;
        while (i < hits.Length) 
		{
            RaycastHit hit = hits[i];
			Unit u = hit.collider.GetComponent<Unit>();
			if(u && u.PlayerOwner != this.PlayerOwner)
			{
				u.GetDamadge(currentAttack, this);
				currentAttack /= 2;
				
			}
            i++;
			
        }
		OnActionCompleted();
    }   
}
