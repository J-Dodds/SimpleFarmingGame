using UnityEngine;
using System;
using System.Collections.Generic;

public class Crop 
{

//  1. Implement the variables required for the Crop class:
//  	a. UniqueId - a string representing the unique identifier for the crop. 
//					  It must be different for every crop.
    public string UniqueId = "crop1";
//  	b. TimeToMature - a floating point number that represents the time in 
//						  seconds for a crop to fully mature.
    public float TimeToMature = 15f;
//  	c. DeathChance - a floating point number between 0 and 1 that represents 
//						 the percentage chance of a crop dying every time the death 
//						 check happens.
    public float DeathChance = 0.2f;
//  	d. MaturityPercentage - a floating point number between 0 and 1 that represents 
//								the maturity percentage of a crop. A fully mature crop 
//								will have a maturity percentage of 1.
    public float MaturityPercentage = 0.0f;
//  	e. IntervalBetweenDeathChecks - the time in seconds between performing death 
//										checks for a crop.
    public float IntervalBetweenDeathChecks = 4;
//  	f. IsDead - a boolean that is true if a crop is dead and false if a crop is alive.
    public bool IsDead = false;
//  	g. Name - a string that represents the name to display for a crop.
    public string Name = "Grumpy Cat Grass";
//  	h. Cost - an integer that represents how much it costs to plant a crop.
    public int Cost = 100;
//  	i. MaxValue - an integer that represents the maximum income from a crop.
    public int MaxValue = 200;

//  2. Implement the properties required for the Crop class:
//  	a. IsMature - a boolean that is true if, and only if, a crop is mature 
//					  (and not dead). This property is never set.
    public bool IsMature 
    { 
        get
        {
            if (MaturityPercentage == 1f && IsDead == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        } 
    }
//  	b. Value - an integer that represents the current value of a crop. The 
//				   value must change with maturity percentage and based upon if 
//				   the crop is dead or alive. This property is never set.

//  3. Implement the constructors for the Crop class. There must be two:
//  	a. One constructor takes a string as a parameter. The string represents the 
//		   unique identifier to use for the crop.
//  		i.  The variables (Name, Cost etc) must all be set in the constructor.
//  		ii. The values may be set based on the unique identifier for the crop.
//  	b. The other constructor takes another crop as a parameter.
//  		i.  The constructor must copy all of the values from the passed in crop.

//  4. Implement the Update function for the Crop class:
//  	a. The Update function takes a single parameter that is a floating point 
//		   representing the time elapsed (delta time) since Update was last called.
//  	b. The Update function does not return any values.
//  	c. It must update the maturity percentage for the crop (if the crop is alive).
//  	d. The Update function must perform the death check and flag the crop as dead 
//		   if required. The death check must be performed, at the required interval, 
//		   on any living crop regardless of maturity. 
}
