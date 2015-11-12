using UnityEngine;
using System;
using System.Collections.Generic;

public class Crop
{
    //Able to use random generator
    System.Random random = new System.Random();

    //String unique to each crop
    public string UniqueId;

    //Float representing the amount of time it takes for a crop to mature
    public float TimeToMature;

    //Float representing a crops chance to die in each death check
    public float DeathChance;

    //Float representing the maturity of a crop with 1 being equal to 100%
    public float MaturityPercentage;

    //Float representing the time between each death check
    public float IntervalBetweenDeathChecks;

    //Bool representing whether or not the crop is alive, True = dead, False = Alive
    public bool IsDead;

    //String representing a crops name
    public string Name;

    //Int representing the cost of a crop
    public int Cost;

    //Int representing the max value from a crop when sold
    public int MaxValue;

    //Property that returns true if, and only if, a crop is mature and not dead, else it returns dead
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

    //Property that returns an int that is the amount a crop is worth when sold
    public int Value
    {
        get
        {
            if (IsDead == false)
            {
                return Convert.ToInt32(Cost * MaturityPercentage) * 2;
            }
            else
            {
                return Convert.ToInt32(Cost * MaturityPercentage) / 2;
            }
        }
    }

    //Constructor that gives all the data for a crop value based on the given UniqueId
    public Crop (string _uniqueId)
    {
        switch (_uniqueId)
        {
            case "Crop_1":
                {
                    UniqueId = _uniqueId;
                    TimeToMature = 10f;
                    DeathChance = 0.1f;
                    MaturityPercentage = 0.0f;
                    IntervalBetweenDeathChecks = 5f;
                    IsDead = false;
                    Name = "Source Tree";
                    Cost = 200;
                    MaxValue = 400;
                    break;
                }
            case "Crop_2":
                {
                    UniqueId = _uniqueId;
                    TimeToMature = 11f;
                    DeathChance = 0.2f;
                    MaturityPercentage = 0.0f;
                    IntervalBetweenDeathChecks = 6f;
                    IsDead = false;
                    Name = "Cheesecake Tree";
                    Cost = 300;
                    MaxValue = 600;
                    break;
                }
            case "Crop_3":
                {
                    UniqueId = _uniqueId;
                    TimeToMature = 12f;
                    DeathChance = 0.3f;
                    MaturityPercentage = 0.0f;
                    IntervalBetweenDeathChecks = 7f;
                    IsDead = false;
                    Name = "Post-Apocalyptic Peas";
                    Cost = 400;
                    MaxValue = 800;
                    break;
                }
            case "Crop_4":
                {
                    UniqueId = _uniqueId;
                    TimeToMature = 13f;
                    DeathChance = 0.4f;
                    MaturityPercentage = 0.0f;
                    IntervalBetweenDeathChecks = 8f;
                    IsDead = false;
                    Name = "Radioactive Raddishes";
                    Cost = 500;
                    MaxValue = 1000;
                    break;
                }
            default:
                {
                    UniqueId = _uniqueId;
                    TimeToMature = 14f;
                    DeathChance = 0.5f;
                    MaturityPercentage = 0.0f;
                    IntervalBetweenDeathChecks = 9f;
                    IsDead = false;
                    Name = "Grumpy Cat Grass";
                    Cost = 100;
                    MaxValue = 200;
                    break;
                }
        }
                
    }
    //Constructor that stores previously passed in crops, so that all crops can be present at once
    public Crop(Crop previousCrop)
    {
        UniqueId = previousCrop.UniqueId;
        TimeToMature = previousCrop.TimeToMature;
        DeathChance = previousCrop.DeathChance;
        MaturityPercentage = previousCrop.MaturityPercentage;
        IntervalBetweenDeathChecks = previousCrop.IntervalBetweenDeathChecks;
        IsDead = previousCrop.IsDead;
        Name = previousCrop.Name;
        Cost = previousCrop.Cost;
        MaxValue = previousCrop.MaxValue;
    }

    //Function that performs all the calculation for the crop class
    public void Update(float timeElapsed)
    {
        double RandomNumber = random.NextDouble();
        IntervalBetweenDeathChecks = IntervalBetweenDeathChecks - timeElapsed;              //Lowers IntervalBetweenDeathChecks

        if (MaturityPercentage < 1.0f)
        {
            MaturityPercentage = MaturityPercentage + (timeElapsed / TimeToMature);         //Increases MaturityPercentage

            if (IntervalBetweenDeathChecks <= 0.0f)                                         //Performs the death check
            {
                IntervalBetweenDeathChecks = 5f;
                if (RandomNumber < DeathChance || IsDead == true)
                {
                    IsDead = true;
                }
                else
                {
                    IsDead = false;
                }
            }
        }
        else if (MaturityPercentage >= 1.0f && IsDead == false)                               //Makes sure that the MaturityPercentage wont increase past 1 and will stay at 1 if it reaches 1 before dying.
        {
            MaturityPercentage = 1.0f;             

            if (IntervalBetweenDeathChecks <= 0.0f)
            {
                IntervalBetweenDeathChecks = 5f;
                if (RandomNumber < DeathChance || IsDead == true)
                {
                    IsDead = true;
                }
                else
                {
                    IsDead = false;
                }
            }
        }
    }
}
 