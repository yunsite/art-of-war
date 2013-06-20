using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : Unit
{
    public override void MoveToPosition(Vector3 worldPosition)
    {
        float distance = (worldPosition - selfTransform.position).magnitude;
        if (!isBusy && CanMove(worldPosition))
        {
            MovementStatistics.RemainingRange -= distance;
            StartCoroutine(Motion(worldPosition));
        }
    }

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

    IEnumerator Motion(Vector3 target)
    {
        Vector3 offset = target - transform.position;
        offset.y = 0;
        Vector3 direction = offset.normalized;
        float distance = offset.magnitude;
        audio.Play();
        while (distance > targetRadius)
        {
            Vector3 cross = Vector3.Cross(transform.forward, direction);
            if (Vector3.Dot(transform.forward, direction) < 0) cross.Normalize();
            rigidbody.angularVelocity = cross * rotationSpeed * Mathf.Min(distance / targetRadius, 1);
            rigidbody.velocity = transform.forward * Mathf.Min(distance * targetRadius, motionSpeed);
            audio.volume = rigidbody.velocity.magnitude / motionSpeed;
            yield return new WaitForFixedUpdate();
            offset = target - transform.position;
            offset.y = 0;
            direction = offset.normalized;
            distance = offset.magnitude;
        }

        audio.Stop();
        OnActionCompleted();
    }
}
