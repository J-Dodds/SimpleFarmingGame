using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class GameState 
{
//List of Crops
    public List<Crop> AvailableCrops = new List<Crop>();

//Dictionary of MonoBehavior and Crops
    public Dictionary<MonoBehaviour, Crop> PlantedCrops = new Dictionary<MonoBehaviour, Crop>();

//Players current amount of money
    public int Money;

//Populate AvailableCrops list, creating UniqueIds for Crops
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

// Returns whether or not the crop planting worked, if it works, it takes the cost from the players money
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

//Function for removing a crop, without gaining money from it
    public void ClearCrop(MonoBehaviour tileToClear)
    {
        PlantedCrops.Remove(tileToClear);
    }
  
// Function for harvesting crops and giving the player money
    public void AttemptToHarvestCrop (MonoBehaviour tileToHarvest)
    {
            Money = Money + PlantedCrops[tileToHarvest].Value;
            PlantedCrops.Remove(tileToHarvest);
    }

// Updates each crops vale, based on the amount of values present in PlantedCrops
    public void Update(float timeElapsed)
    {
        foreach(KeyValuePair<MonoBehaviour, Crop> cropPresent in PlantedCrops)
        {
            cropPresent.Value.Update(timeElapsed);
        }
    }

// Returns current state of a tile
    public void GetCropState (MonoBehaviour tileDetail, out float maturityPercentage, bool isMature, bool isDead)
    {
        isMature = PlantedCrops[tileDetail].IsMature;
        isDead = PlantedCrops[tileDetail].IsDead;
        maturityPercentage = PlantedCrops[tileDetail].MaturityPercentage;
    }

// Returns the UniqueId of a crop based on the given index of AvailableCrops list
    public string UniqueIdForCropAtIndex (int cropIndex)
    {
        return AvailableCrops[cropIndex].UniqueId;
    }

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