using System;
using UnityEngine;

/// <summary>
/// Abstract base class for each state of single player turn state machine.
/// Default event behaviours must be redefined in derived classes in order to support specified event.
/// </summary>
/// <exception cref="System.InvalidOperationException">
///     Thrown as default event result.
/// </exception>
public abstract class TurnState
{
    private const string forbiddenEventErrorMessage = "Forbidden event in current turn state";

    /// <summary>
    /// State entry behaviour, called in case of in-transition occurrence.
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// State exit behaviour, called in case of out-transition occurrence.
    /// </summary>
    public virtual void Exit() { }

    #region Events
    /// <summary>
    /// Unit selected event behaviour.
    /// </summary>
    /// <param name="unit">Unit being selected.</param>
    /// <returns>New state of single player turn state machine.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown as default event behaviour.
    /// </exception>
    public virtual TurnState UnitSelected(Unit unit)
    {
        throw new InvalidOperationException(forbiddenEventErrorMessage);
    }

    /// <summary>
    /// Terrain position selected event behaviour.
    /// </summary>
    /// <param name="unit">Terrain position being selected.</param>
    /// <returns>New state of single player turn state machine.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown as default event behaviour.
    /// </exception>
    public virtual TurnState TerrainPositionSelected(Vector3 position)
    {
        throw new InvalidOperationException(forbiddenEventErrorMessage);
    }

    /// <summary>
    /// Unit move action selected event behaviour.
    /// </summary>
    /// <returns>New state of single player turn state machine.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown as default event behaviour.
    /// </exception>
    public virtual TurnState MoveActionSelected()
    {
        throw new InvalidOperationException(forbiddenEventErrorMessage);
    }

    /// <summary>
    /// Unit attack action selected event behaviour.
    /// </summary>
    /// <returns>New state of single player turn state machine.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown as default event behaviour.
    /// </exception>
    public virtual TurnState AttackActionSelected()
    {
        throw new InvalidOperationException(forbiddenEventErrorMessage);
    }

    /// <summary>
    /// Unit special action selected event behaviour.
    /// </summary>
    /// <returns>New state of single player turn state machine.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown as default event behaviour.
    /// </exception>
    public virtual TurnState SpecialActionSelected()
    {
        throw new InvalidOperationException(forbiddenEventErrorMessage);
    }

    /// <summary>
    /// Unit action completed event behaviour.
    /// </summary>
    /// <returns>New state of single player turn state machine.</returns>
    /// <exception cref="System.InvalidOperationException">
    ///     Thrown as default event behaviour.
    /// </exception>
    public virtual TurnState ActionCompleted()
    {
        throw new InvalidOperationException(forbiddenEventErrorMessage);
    }
    #endregion
}