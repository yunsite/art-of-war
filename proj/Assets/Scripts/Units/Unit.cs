using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Unit : MonoBehaviour
{
	private Transform selfTransform;
	private bool isBusy = false;
	
	void Awake () {
		selfTransform = transform;
		rigidbody.isKinematic = true;
	}
	
	public UnitTypeEnum UnitType;
	
	#region Selection items
	
	public SelectionMarker Selector;
	public RangeProjector RangeProjector;
	
	public void Select () {
		SelectionMode mode;
		float range;
		if (CanMove() || CanAttack()) {
			mode = SelectionMode.Default;
		} else {
			mode = SelectionMode.NoAction;
		}
		
		SelectRange(mode, 0);
	}
	
	public void SelectMovement () {
		SelectionMode mode;
		float range;
		if (CanMove()) {
			mode = SelectionMode.Movement;
			range = MovementStatistics.RemainingRange;
		} else {
			mode = SelectionMode.NoAction;
			range = MovementStatistics.TotalRange;
		}
		
		SelectRange(mode, range);
	}
	
	public void SelectAttack () {
		SelectionMode mode;
		if (CanAttack()) {
			mode = SelectionMode.Attack;
		} else {
			mode = SelectionMode.NoAction;
		}
		
		SelectRange(mode, AttackStatistics.Range);
	}
	
	private void SelectRange(SelectionMode mode, float range) {
		Selector.gameObject.SetActive(true);
		Selector.SetMode(mode);
		if (range > 0) {
			RangeProjector.gameObject.SetActive(true);
			RangeProjector.SetColor(Selector.GetModeColor(mode));
			RangeProjector.SetRange(range);
		} else {
			RangeProjector.gameObject.SetActive(false);
		}
	}
	
	public void Deselect () {
		Selector.gameObject.SetActive(false);
		RangeProjector.gameObject.SetActive(false);
	}
	
	#endregion
	
	#region Movement items
	
	[Serializable]
	public class UnitMovementStatistics {
		public float RemainingRange = 40;
		public float TotalRange = 40;
		public DifficultTerrainMoveAbilityEnum DifficultTerrainMoveAbility;
	}
	
	public UnitMovementStatistics MovementStatistics = new UnitMovementStatistics();
	
	public void MoveToPosition(Vector3 worldPosition, Action callback)
	{
		float distance = (worldPosition - selfTransform.position).magnitude;
		if(!isBusy && CanMove(worldPosition)) {
			MovementStatistics.RemainingRange -= distance;
			StartCoroutine(Moving(worldPosition, callback));
		}
	}
	
	public bool CanMove () {
		return MovementStatistics.RemainingRange > 2;
	}
	
	public bool CanMove (Vector3 targetPosition) {
		float distance = (targetPosition - selfTransform.position).magnitude;
		return CanMove() && MovementStatistics.RemainingRange >= distance;
	}
	
	#endregion
	
	#region Attack items
	
	[Serializable]
	public class UnitAttackStatistics {
		public int RemainingQuantity = 2;
		public int TotalQuantity = 2;
		public float Power = 5;
		public float Range = 60;
	}
	
	public UnitAttackStatistics AttackStatistics = new UnitAttackStatistics();
	
	public void Attack(Unit enemy, Action callback)
	{
		if(!isBusy && CanAttack(enemy.transform.position)) {
			--AttackStatistics.RemainingQuantity;
			StartCoroutine(Attacking(enemy, callback));
		}
	}
	
	public bool CanAttack () {
		return AttackStatistics.RemainingQuantity > 0;
	}
	
	public bool CanAttack (Vector3 targetPosition) {
		float distance = (targetPosition - selfTransform.position).magnitude;
		return CanAttack() && distance <= AttackStatistics.Range;
	}
	
	#endregion
	
	#region Health items
	
	[Serializable]
	public class UnitHealthStatistics {
		public float RemainingPoints = 100;
		public float TotalPoints = 100;
		public float Deffence = 4;
	}
	
	public UnitHealthStatistics HealthStatistics = new UnitHealthStatistics();
	
	public float GetDamadge(float damage, Unit attacker)
	{
		// Na razie podstawowa funkcjonalność.
		HealthStatistics.RemainingPoints -= damage;
		// if (HP <= 0) Die();
		return HealthStatistics.RemainingPoints; // Co ma być wartością zwracana ?
	}
	
	//nie wiem jak implementowac tu pewnie maja leciec jakies efekty umierania najpierw a potem usuwanie ze sceny
	public void Die()
	{
		throw new NotImplementedException();
	}
	
	#endregion
	
	public float motionSpeed = 5;
	public float rotationSpeed = 20;
	
	#region Events
	
	public event EventHandler Clicked;
	void OnMouseUpAsButton () 
	{

		if (Clicked != null) 
		{
			Clicked (this, new EventArgs());
		}
	}
	
	#endregion
	
	#region Public Methods
	
	//nie wiem jak powinna dzialac
	public void UseSpecial()
	{
		throw new NotImplementedException();
	}
	
	#endregion
	
	#region Coroutines
	IEnumerator Attacking (Unit target, Action callback) {
		isBusy = true;
		AnimationClip clip = animation.GetClip("fire");
		if (clip != null) {
			animation.Play(clip.name);
			yield return new WaitForSeconds(clip.length);
		}
		
		target.GetDamadge(AttackStatistics.Power, this);
		animation.CrossFade("none");
		if (callback != null) callback();
		isBusy = false;
	}
	
	IEnumerator Move (float length) {
		while (length > 0) {
			float deltaLength = Mathf.Min(length, motionSpeed * Time.fixedDeltaTime);
			selfTransform.Translate(selfTransform.forward * deltaLength, Space.World);
			length -= deltaLength;
			yield return new WaitForFixedUpdate();
		}
	}
	
	IEnumerator Turn (float angle) {
		float sign = Mathf.Sign(angle);
		angle *= sign;
		while (angle > 0) {
			float deltaAngle = Mathf.Min(angle, rotationSpeed * Time.fixedDeltaTime);
			selfTransform.Rotate(0, sign * Mathf.Min(angle, rotationSpeed * Time.fixedDeltaTime), 0, Space.World);
			angle -= deltaAngle;
			yield return new WaitForFixedUpdate();
		}
	}
	
	IEnumerator Moving (Vector3 target, Action callback) {
		isBusy = true;
		AnimationClip forward = animation.GetClip("forward");
		AnimationClip turn;
		Vector3 direction = target - selfTransform.position;
		direction.y = 0;
		float lenth = direction.magnitude;
		direction /= lenth;
		float angle = Quaternion.FromToRotation(
			selfTransform.forward,
			direction).eulerAngles.y;
		if (angle > 180) {
			angle -= 360;
			turn = animation.GetClip("turnLeft");
		} else {
			turn = animation.GetClip("turnRight");
		}
		
		if (turn != null) animation.Play(turn.name);
		yield return StartCoroutine(Turn(angle));
		if (forward != null) animation.Play(forward.name);
		yield return StartCoroutine(Move(lenth));
		animation.CrossFade("none");
		if (callback != null) callback();
		isBusy = false;
	}
	#endregion
}