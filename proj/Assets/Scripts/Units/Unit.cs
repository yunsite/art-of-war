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
	}
	
	public UnitTypeEnum UnitType;
	
	#region Selection items

    public int PlayerOwner;
	public SelectionMarker Selector;
	public RangeProjector RangeProjector;
	
	public void Select () {
		SelectionMode mode;
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

    public virtual void SelectSpecialAbility()
    {
		/*
		 * Nie bardzo rozumiem tą implementację więc ją zakomentowywuję
		 * zmieniam metode na virtualną (Marek Kokot) (dot. issue 0056)
        SelectionMode mode;
        if (CanAttack())
        {
            mode = SelectionMode.SpecialAbility;
        }
        else
        {
            mode = SelectionMode.NoAction;
        }

        SelectRange(mode, AttackStatistics.Range);
        */
    }
	
	protected void SelectRange(SelectionMode mode, float range) {
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
	
	public void MoveToPosition(Vector3 worldPosition)
	{
		float distance = (worldPosition - selfTransform.position).magnitude;
		if(!isBusy && CanMove(worldPosition)) {
			MovementStatistics.RemainingRange -= distance;
			StartCoroutine(Moving(worldPosition));
		}
	}
	
	public bool CanMove () {
		return MovementStatistics.RemainingRange > 2;
	}
	
	public bool CanMove (Vector3 targetPosition) {
		float distance = (targetPosition - selfTransform.position).magnitude;
		return CanMove() && MovementStatistics.RemainingRange >= distance;
	}
	
	public void ResetMovementPoints() {
		MovementStatistics.RemainingRange = MovementStatistics.TotalRange;
	}
	
	#endregion
	
	#region Attack items
	
	[Serializable]
	public class UnitAttackStatistics {
		public int RemainingQuantity = 2;
		public int TotalQuantity = 2;
		public float Power = 5;
		public float Range = 60;
		public AttackAreaEnum Area;
	}
	
	public UnitAttackStatistics AttackStatistics = new UnitAttackStatistics();
	
	public void Attack(Unit enemy)
	{
		if(!isBusy && CanAttack(enemy.transform.position)) {
			--AttackStatistics.RemainingQuantity;
			StartCoroutine(Attacking(enemy));
		}
	}

    public void Attack(Vector3 target)
    {
        throw new NotImplementedException();
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
		if (HealthStatistics.RemainingPoints <= 0) Die();
		return HealthStatistics.RemainingPoints; // Co ma być wartością zwracana ?
	}
	
	//nie wiem jak implementowac tu pewnie maja leciec jakies efekty umierania najpierw
	public void Die()
	{
		Destroy(gameObject);
	}
	
	#endregion
	
	public float motionSpeed = 5;
	public float rotationSpeed = 20;
	
	#region Events

    public event EventHandler Clicked;
    protected void OnClicked(object sender, EventArgs e)
    {
        if (Clicked != null)
        {
            Clicked(sender, e);
        }
    }

    void OnMouseUpAsButton()
    {
        OnClicked(this, new EventArgs());
    }

    public event EventHandler ActionCompleted;
	protected void OnActionCompleted()
	{
		if(ActionCompleted != null)
			ActionCompleted(this, new EventArgs());
	}
	#endregion
	
	#region Public Methods
	
	
	public virtual void EndTour()
	{
		ResetMovementPoints();
		return;
	}

	//W trakcie implementacji
	public virtual void UseSpecial()
	{
		return;
		//throw new NotImplementedException();
	}
	
	#endregion
	
	#region Coroutines
	IEnumerator Attacking (Unit target) {
		isBusy = true;
		AnimationClip clip = animation.GetClip("fire");
		if (clip != null) {
			animation.Play(clip.name);
			yield return new WaitForSeconds(clip.length);
		}
		
		UnityEngine.Object ammo = Instantiate(Resources.Load("ammo"));
		ammo.name = "ammo";
		
		GameObject obj = GameObject.Find("ammo");
		
		AnimationClip turn;
		Vector3 direction = (Vector3) target.selfTransform.position - selfTransform.position;
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
		
		yield return StartCoroutine(Shoot(obj.transform, selfTransform.position, target.transform.position, 2.0f));
		Destroy(obj);
		
		target.GetDamadge(AttackStatistics.Power, this);
		animation.CrossFade("none");
        if (ActionCompleted != null) ActionCompleted(this, new EventArgs());
		isBusy = false;
	}
	
	IEnumerator Shoot (Transform thisTransform, Vector3 startPos, Vector3 endPos, float time) {
    	float i = 0.0f;
    	float rate = 1.0f / time;
    	while (i < 1.0f) {
        	i += Time.deltaTime * rate;
        	thisTransform.position = Vector3.Lerp(startPos, endPos, i);
			Debug.Log(i);
			yield return new WaitForFixedUpdate();
    	}
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
	
	IEnumerator Moving (Vector3 target) {
		isBusy = true;
        audio.Play();
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
        audio.Stop();
        if (ActionCompleted != null) ActionCompleted(this, new EventArgs());
		isBusy = false;
	}
	#endregion
}

public enum UnitTypeEnum {
	IFV, Tank, HeavyTank, Helicopter, Artillery
}

public enum DifficultTerrainMoveAbilityEnum {
	Poor, Medium, Good
}

public enum AttackAreaEnum {
	Unit, Field
}
