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
	
	public float motionSpeed = 5;
	public float rotationSpeed = 20;

	public int HP = 100;
	public byte PlayerOwner;
	public UnitTypeEnum UnitType;
	public float MaximalMoveDistance;
	public float MaximalShootDistance;
	public float MoveDistanceLeft;
	public DifficultTerrainMoveAbilityEnum DifficultTerrainMoveAbility;
	public float AttackValue = 5; //zmienilem nazwe aby nie bylo konfliktu z metoda Attack
	public float Deffence = 4;
	public float AttackAreaRadious;
	
	public event EventHandler Clicked;
	void OnMouseUpAsButton () 
	{

		if (Clicked != null) 
		{
			Clicked (this, new EventArgs());
		}
	}
	
	void Awake () {
		selfTransform = transform;
		rigidbody.isKinematic = true;
	}
	
	#region Public Methods
	//nie wiem jak powinna dzialac ta metoda
	public void Attack(Unit enemy, Action callback)
	{
		if(!isBusy) StartCoroutine(Attacking(enemy, callback));
	}
	
	//nie wiem jak ma dzialac ani jaki typ zwracac
	public int GetDamadge(int damage, Unit attacker)
	{
		// Na razie podstawowa funkcjonalność.
		HP -= damage;
		// if (HP <= 0) Die();
		return HP; // Co ma być wartością zwracana ?
	}
	
	//nie wiem czy to dobra implementacja
	public void MoveToPosition(Vector3 worldPosition, Action callback)
	{
		//gameObject.transform.position = worldPosition;
		if(!isBusy) StartCoroutine(Moving(worldPosition, callback));
	}
	
	//nie wiem jak powinna dzialac
	public void UseSpecial()
	{
		throw new NotImplementedException();
	}
	
	//nie wiem jak implementowac tu pewnie maja leciec jakies efekty umierania najpierw a potem usuwanie ze sceny
	public void Die()
	{
		throw new NotImplementedException();
	}
	
	//nie wiem jak ma dzialac ta metoda, zakladam ze ma wyswietlac pole gdzie moze byc pokazany zasieg ataku
	public void DisplayAttackDistance()
	{
		throw new NotImplementedException();
	}
	
	//jako ze nie iwem jak wyswietlac to pole ataku to nie wiem w jaki sposob je ukrywac
	public void StopDisplayingAttackDistance()
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
		
		target.GetDamadge((int)AttackValue, this);
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
