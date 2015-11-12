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
        Money = 1000;
        AvailableCrops.Add(new Crop("Crop_0"));
        AvailableCrops.Add(new Crop("Crop_1"));
        AvailableCrops.Add(new Crop("Crop_2"));
        AvailableCrops.Add(new Crop("Crop_3"));
        AvailableCrops.Add(new Crop("Crop_4"));
        numberOfCrops = AvailableCrops.Count;
    }

// Returns whether or not the crop planting worked, if it works, it takes the cost from the players money
    public bool PlantCrop (string _uniqueId, MonoBehaviour tileToPlant)
    {
        Crop CropInfo = null;

        if (PlantedCrops.ContainsKey(tileToPlant))
        {
            return false;
        }

        for (int value = 0; value < 5; ++value)
        {
            if (AvailableCrops[value].UniqueId == _uniqueId)
            {
                CropInfo = AvailableCrops[value];
            }
        }

        if (CropInfo != null && Money > CropInfo.Cost)
        {
            PlantedCrops.Add(tileToPlant, new Crop(CropInfo));
            Money = Money - CropInfo.Cost;
            return true;
        }
        else
        {
            return false;
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

// Current state of a crop on a certain tile
    public void GetCropState (MonoBehaviour tileDetail, out float maturityPercentage, out bool isMature, out bool isDead)
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

// Stores information from the crop, based on the crops index number
    public void GetInfoForCropAtIndex (int indexOfCrop, out string uniqueIdOfCrop, out string nameOfCrop, out int costOfCrop)
    {
        uniqueIdOfCrop = AvailableCrops[indexOfCrop].UniqueId;
        nameOfCrop = AvailableCrops[indexOfCrop].Name;
        costOfCrop = AvailableCrops[indexOfCrop].Cost;
    }
}