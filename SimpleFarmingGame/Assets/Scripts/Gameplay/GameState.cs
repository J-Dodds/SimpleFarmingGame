using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class GameState 
{
//  5. Implement the variables required for the GameState class:
//  	a. AvailableCrops - a list of Crop objects.
    public List<Crop> AvailableCrops = new List<Crop>();

//  	b. PlantedCrops - a dictionary that maps from MonoBehaviour objects 
// 						  to Crop objects.
    public Dictionary<MonoBehaviour, Crop> PlantedCrops = new Dictionary<MonoBehaviour, Crop>();

//  	c. Money - an integer representing the player's current funds.
    public int Money;

//  6. Implement the constructor for the GameState:
//  	a. The constructor must take in a single parameter that is the number of crops.
//  	b. The constructor must populate the AvailableCrops list and set the initial 
//		   funds that the player has.
    public GameState (int numberOfCrops)
    {
        Money = 500;
        AvailableCrops.Add(new Crop("Crop_1"));
        AvailableCrops.Add(new Crop("Crop_2"));
        AvailableCrops.Add(new Crop("Crop_3"));
        AvailableCrops.Add(new Crop("Crop_4"));
        AvailableCrops.Add(new Crop("Crop_5"));
        numberOfCrops = AvailableCrops.Count;
    }

//  7. Implement the following functions in the GameState class:
//  	a. PlantCrop - Plants a crop on a specific tile.
//  		i.   The function takes two parameters. The first parameter is a string 
//				 representing the unique identifier of the crop to plant. The second 
//				 is a MonoBehaviour representing the tile on which to plant the crop.
//  		ii.  The function returns a boolean that indicates if the planting was 
//				 successful. If the crop was planted the function returns true. 
//			     Otherwise, the function returns false.
//  		iii. If the tile already contains a crop then the function must return 
//				 false and take no other action.
//  		iv.  If the tile is free then the function must locate the specific crop 
//				 to plant (from the AvailableCrops list).
//  			1. The function must then check if there are sufficient funds to plant 
//				   the crop. If there are not enough funds then it must return false.
//  			2. If there are enough funds then it must update the player's funds, 
//				   plant a new crop on the tile and return true.
    public bool PlantCrop (string _uniqueId, MonoBehaviour cropToPlant)
    {
        Crop CropInfo = null;

        for (int value = 0; value <= 5; ++ value)
        {
            if(AvailableCrops[value].UniqueId == _uniqueId)
            {
                CropInfo = AvailableCrops[value];
            }
        }

        if (cropToPlant == true)
        {
            return false;
        }
        else
        {
            Money = Money - CropInfo.Cost;
            return true;
        }
    }

//  	b. ClearCrop - Removes a crop from a specific tile.
//  		i.   The function takes a single parameter that is a MonoBehaviour 
//				 representing the tile to clear.
//  		ii.  The function has no return value. 
    public void ClearCrop(MonoBehaviour tileToClear)
    {
        PlantedCrops.Remove(tileToClear);
    }
  
//  	c. AttemptToHarvestCrop - Harvests a crop on a specific tile (if the crop is present).
//  		i.   The function takes a single parameter that is a MonoBehaviour representing 
//				 the tile to try to harvest.
//  		ii.  The function has no return value.
//  		iii. The function must: 
//  			1. Check if a crop is present.
//  			2. Update the player's funds based upon the value of the crop at the time it 
//				   was cleared.
//  			3. Clear the crop from the tile.
    public void AttemptToHarvestCrop (MonoBehaviour tileToHarvest)
    {
            Money = Money + PlantedCrops[tileToHarvest].Value;
            PlantedCrops.Remove(tileToHarvest);
    }

//  	d. Update - Updates the state of all of the crops.
//  		i.   The function takes a single parameter that represents the time elapsed 
//			     (delta time) since the last time update was called.
//  		ii.  The function has no return value.
//  		iii. The function must update all of the crops.
    public void Update(float timeElapsed)
    {
        foreach(KeyValuePair<MonoBehaviour, Crop> cropPresent in PlantedCrops)
        {
            cropPresent.Value.Update(timeElapsed);
        }
    }

//  	e. GetCropState - Retrieves the current state of a crop on a specific tile.
//  		i.   The function takes four parameters:
//  			1. A MonoBehaviour representing the tile which contains the crop that 
//				   the details are being retrieved for.
//  			2. A floating point number that is the maturity percentage. 
//				   This is an out parameter.
//  			3. A boolean that indicates if the crop is mature. This is an out parameter.
//  			4. A boolean that indicates if the crop is dead. This is an out parameter.
//  		ii.  The function returns no values.
//  		iii. The function must locate the crop for the specific tile and must 
//				 retrieve the required information.
    public void GetCropState (MonoBehaviour tileDetail, out float maturityPercentage, bool isMature, bool isDead)
    {
        isMature = PlantedCrops[tileDetail].IsMature;
        isDead = PlantedCrops[tileDetail].IsDead;
        maturityPercentage = PlantedCrops[tileDetail].MaturityPercentage;
    }

//  	f. UniqueIdForCropAtIndex - Retrieves the unique identifier for an available crop.
//  		i.   The function takes a single parameter that is an integer. The integer is 
//				 the index of the crop in the AvailableCrops list that the details 
//				 are being retrieved for.
//  		ii.  The function returns a string that is the unique identifier for the crop 
//				 at the specified index.


//  	g. GetInfoForCropAtIndex - Retrieves the details for an available crop.
//  		i.   The function returns no values but takes four parameters:
//  			1. An integer that is the index of the crop in the AvailableCrops list 
//				   that the details are being retrieved for.
//  			2. A string that is the unique identifier for the crop. This is an out parameter.
//  			3. A string that is the name for the crop. This is an out parameter.
//  			4. An integer that is the cost of the crop. This is an out parameter.
//  		ii.  The function must locate the crop in the AvailableCrops list and must 
//				 then retrieve the required information.
}