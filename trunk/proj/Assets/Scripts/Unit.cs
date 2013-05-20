using System;
using UnityEngine;

public class Unit : MonoBehaviour
{

	public int HP { get; set; }
	public byte PlayerOwner { get ; set; }
	public UnitTypeEnum UnitType { get ;set; }
	public float MaximalMoveDistance { get ;set ;}
	public float MaximalShootDistance { get ;set; }
	public float MoveDistanceLeft { get; set; }
	public DifficultTerrainMoveAbilityEnum DifficultTerrainMoveAbility { get; set; }
	public float AttackValue { get ;set; } //zmienilem nazwe aby nie bylo konfliktu z metoda Attack
	public float Deffence { get; set; }
	public float AttackAreaRadious { get; set; }
	
	public EventHandler Clicked;
	
	public void OnClick () 
	{
		if (Clicked != null) 
		{
			Clicked (gameObject, new EventArgs());
		}
	}
	
	//nie wiem jak powinna dzialac ta metoda
	public void Attack(Unit enemy)
	{
		
		throw new NotImplementedException();
	}
	
	//nie wiem jak ma dzialac ani jaki typ zwracac
	public int GetDamadge(int damage, Unit aattacker)
	{
		throw new NotImplementedException();
	}
	
	//nie wiem czy to dobra implementacja
	public void MoveToPosition(Vector3 worldPosition)
	{
		gameObject.transform.position = worldPosition;
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
}
