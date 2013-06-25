﻿using UnityEngine;

public class TankSpecialAttackSelectedState : SelectedState
{
    /// <summary>
    /// Selected tank unit.
    /// </summary>
	protected Tank tank;

    /// <summary>
    /// Create tank special attack selection state.
    /// </summary>
    /// <param name="ui">UI reference.</param>
    /// <param name="player">Player info reference.</param>
    /// <param name="unit">Selected unit.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when any parameter is null.</exception>
    /// <exception cref="System.InvalidCastException">Thrown when unit is not Tank.</exception>
    internal TankSpecialAttackSelectedState(InGameUI ui, PlayerInfo player, Unit unit) : base(ui, player, unit)
	{
		tank = (Tank)unit;
	}

    /// <summary>
    /// State entry behaviour, called in case of in-transition occurrence.
    /// </summary>
    /// <remarks>
    /// Marks unit for special ability.
    /// </remarks>
    public override void Enter()
    {
        unit.SelectSpecialAbility();
    }

    #region Events
    /// <summary>
    /// Unit selected event behaviour.
    /// </summary>
    /// <remarks>
    /// Triggers special ability if enemy unit selected and returns selected state for current unit,
    /// returns selected state if same player unit selected.
    /// </remarks>
    /// <param name="unit">Unit being selected.</param>
    /// <returns>New state of single player turn state machine.</returns>
    public override TurnState UnitSelected(Unit enemy)
    {
        if (enemy.PlayerOwner == player.Index)
        {
            return base.UnitSelected(enemy);
        }
        tank.UseSpecial(enemy.transform.position);
        return new SelectedState(ui, player, unit);
    }

    /// <summary>
    /// Terrain position selected event behaviour.
    /// </summary>
    /// <remarks>
    /// Triggers special ability and returns selected state for current unit.
    /// </remarks>
    /// <param name="unit">Terrain position being selected.</param>
    /// <returns>New state of single player turn state machine.</returns>
	public override TurnState TerrainPositionSelected (Vector3 position)
	{
		tank.UseSpecial(position);
		return new SelectedState(ui, player, unit);
	}	
    #endregion
}

