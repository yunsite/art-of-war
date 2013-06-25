using System;
using UnityEngine;

public class HeavyTank : Unit
{
    private int toursToEndSpecial;
    private float defenceBeforeSpecial;
    private float movementRangeBeforeSpecial;
    public HeavyTank()
    {
        toursToEndSpecial = 0;
    }
	
	/// <summary>
	/// Uses the special ability which is increase defence for 2 rounds with block movement ability
	/// </summary>
    public override void UseSpecial()
    {
        if (toursToEndSpecial == 0) //use special only if is not in use already
        {
            toursToEndSpecial = 2;
            defenceBeforeSpecial = HealthStatistics.Deffence;
            HealthStatistics.Deffence *= 2;
            movementRangeBeforeSpecial = MovementStatistics.TotalRange;
            MovementStatistics.TotalRange = 0.0f;
            MovementStatistics.RemainingRange = 0.0f;
        }
    }
	
	/// <summary>
	/// Ends the turn -> make normal defence and movement ability
	/// </summary>
    public override void EndTurn()
    {
        base.EndTurn();
        if (toursToEndSpecial > 0)
            --toursToEndSpecial;
        if (toursToEndSpecial == 0)
        {
            HealthStatistics.Deffence = defenceBeforeSpecial;
            MovementStatistics.TotalRange = MovementStatistics.RemainingRange = movementRangeBeforeSpecial;
        }
    }
}
