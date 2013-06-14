using System;
using UnityEngine;

public class IFVUnit : Unit
{
	
    private int toursToEndSpecial;
    private float movementRangeBeforeSpecial;
    public IFVUnit()
    {
        toursToEndSpecial = 0;
    }
    public override void UseSpecial()
    {
        if (toursToEndSpecial == 0) //use special only if is not in use already
        {
            toursToEndSpecial = 1;
            movementRangeBeforeSpecial = MovementStatistics.RemainingRange;
            MovementStatistics.RemainingRange *= 2;            
        }
    }

    public override void EndTour()
    {
        if (toursToEndSpecial > 0)
            --toursToEndSpecial;
        if (toursToEndSpecial == 0)
        {
            //HealthStatistics.Deffence = defenceBeforeSpecial;
            //MovementStatistics.TotalRange = MovementStatistics.RemainingRange = movementRangeBeforeSpecial;
        }
    }
    
}
