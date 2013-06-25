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
            StartCoroutine(ProcessMotion(worldPosition));
        }
    }

    public override void Attack(Unit enemy)
    {
        // Jeszcze nie gotowe
        throw new System.NotImplementedException();
    }

	private bool canUse = true;
	/// <summary>
	/// Uses the special ability which is attack 2 or 3 or 4 enemy units with full valude devided by
	/// attacked enemies count.
	/// </summary>
	/// <param name='unitsToAttack'>
	/// Units to attack.
	/// </param>
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
	public override void EndTurn ()
	{
        base.EndTurn();
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

    IEnumerator ProcessMotion(Vector3 target)
    {
        Vector3 offset = target - transform.position;
        offset.y = 0;
        Vector3 direction = offset.normalized;
        float distance = offset.magnitude;
        audio.Play();
        while (distance > MovementStatistics.TargetRadius)
        {
            Vector3 cross = Vector3.Cross(transform.forward, direction);
            if (Vector3.Dot(transform.forward, direction) < 0) cross.Normalize();
            rigidbody.angularVelocity =
                cross * MovementStatistics.RotationSpeed * Mathf.Min(distance / MovementStatistics.TargetRadius, 1);
            rigidbody.velocity =
                transform.forward * Mathf.Min(distance * MovementStatistics.TargetRadius, MovementStatistics.MotionSpeed);
            audio.volume = rigidbody.velocity.magnitude / MovementStatistics.MotionSpeed;
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
