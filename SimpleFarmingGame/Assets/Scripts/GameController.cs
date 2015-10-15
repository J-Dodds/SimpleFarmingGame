using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class GameController : MonoBehaviour {
	private enum InterfaceStatus {
		Missing,
		WrongSignature,
		IntegrityCheckFailed,
		Valid
	}
	
	private static BindingFlags SearchFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
	
	public static GameController Instance {get; private set;}
	
	public int NumCrops = 5;
	public GameObject[] CropPrefabs;

	private object gameState;
	
	private string menuItemIdSelected;
	private GameObject selectedCropPrefab;

	// Default object types	
	System.Type gameStateType = null;
	System.Type cropType = null;		
		
	// Reflection interfaces
	Assembly fallbackAssembly;
	MethodInfo clearCropMethod;
	MethodInfo plantCropMethod;
	MethodInfo attemptToHarvestCropMethod;
	MethodInfo updateMethod;
	MethodInfo getCropStateMethod;
	MethodInfo getInfoForCropAtIndexMethod;
	MethodInfo uniqueIdForCropAtIndexMethod;
	FieldInfo moneyField;
	FieldInfo plantedCropsField;
	
	public bool usingStudentCropClass = false;
	public bool usingStudentGameStateClass = false;
	
	void Awake() {
		Instance = this;

		// Load the fallback core		
		fallbackAssembly = FallbackLoader.LoadFallbackGameCore();
		gameStateType = fallbackAssembly.GetType("FallbackGameState");
		cropType = fallbackAssembly.GetType("FallbackCrop");
		
		// Run the validation code to patch in the appropriate types
		usingStudentCropClass = ValidateCropClass();
		usingStudentGameStateClass = ValidateGameStateClass();
		
		// Check if we are using the fallback game state
		if (gameStateType.Name == "FallbackGameState") {
			ConstructorInfo gameStateConstructor = gameStateType.GetConstructor(new []{typeof(int), typeof(System.Type)});
			gameState = gameStateConstructor.Invoke(new [] {(object)NumCrops, cropType});
		} // Otherwise we are using the student provided gamestate
		else {
			ConstructorInfo gameStateConstructor = gameStateType.GetConstructor(new []{typeof(int)});
			gameState = gameStateConstructor.Invoke(new [] {(object)NumCrops});
		}
		
		// Retrieve the required methods for the game state
		clearCropMethod 				= gameStateType.GetMethod("ClearCrop");
		plantCropMethod 				= gameStateType.GetMethod("PlantCrop");
		attemptToHarvestCropMethod 		= gameStateType.GetMethod("AttemptToHarvestCrop");
		updateMethod 					= gameStateType.GetMethod("Update");
		getCropStateMethod 				= gameStateType.GetMethod("GetCropState");
		getInfoForCropAtIndexMethod 	= gameStateType.GetMethod("GetInfoForCropAtIndex");
		uniqueIdForCropAtIndexMethod 	= gameStateType.GetMethod("UniqueIdForCropAtIndex");
		moneyField						= gameStateType.GetField("Money");
		plantedCropsField				= gameStateType.GetField("PlantedCrops");		
	}
	
	void Start() {
		// Update the subsystem state
		UIController.SetCropStatus(usingStudentCropClass);
		UIController.SetGameStateStatus(usingStudentGameStateClass);
	}
	
	bool ValidateGameStateClass() {
		bool gameStateValid = true;
		
		// Retrieve the type of the student supplied gamestate
		System.Type potentialGameStateType = typeof(GameState);
		
		// Test the variables
		gameStateValid &= EvaluateVariableStatus(potentialGameStateType, "Money", typeof(int)) == InterfaceStatus.Valid;
		gameStateValid &= EvaluateVariableStatus(potentialGameStateType, "AvailableCrops", typeof(List<object>)) == InterfaceStatus.Valid;
		gameStateValid &= EvaluateVariableStatus(potentialGameStateType, "PlantedCrops", typeof(Dictionary<MonoBehaviour, object>)) == InterfaceStatus.Valid;
		
		// Test the constructors
		gameStateValid &= EvaluateConstructorStatus(potentialGameStateType, new[]{typeof(int)}) == InterfaceStatus.Valid;
		
		// Test the functions
		gameStateValid &= EvaluateMethodStatus(potentialGameStateType, "PlantCrop", typeof(bool), 
							new[]{typeof(string), typeof(MonoBehaviour)}, new[]{false, false}) == InterfaceStatus.Valid;
		gameStateValid &= EvaluateMethodStatus(potentialGameStateType, "ClearCrop", typeof(void), 
							new[]{typeof(MonoBehaviour)}, new[]{false}) == InterfaceStatus.Valid;
		gameStateValid &= EvaluateMethodStatus(potentialGameStateType, "AttemptToHarvestCrop", typeof(void), 
							new[]{typeof(MonoBehaviour)}, new[]{false}) == InterfaceStatus.Valid;
		gameStateValid &= EvaluateMethodStatus(potentialGameStateType, "Update", typeof(void), 
							new[]{typeof(float)}, new[]{false}) == InterfaceStatus.Valid;
		gameStateValid &= EvaluateMethodStatus(potentialGameStateType, "GetCropState", typeof(void), 
							new[]{typeof(MonoBehaviour), typeof(float).MakeByRefType(), typeof(bool).MakeByRefType(), typeof(bool).MakeByRefType()}, 
							new[]{false, true, true, true}) == InterfaceStatus.Valid;
		gameStateValid &= EvaluateMethodStatus(potentialGameStateType, "UniqueIdForCropAtIndex", typeof(string), 
							new[]{typeof(int)}, new[]{false}) == InterfaceStatus.Valid;
		gameStateValid &= EvaluateMethodStatus(potentialGameStateType, "GetInfoForCropAtIndex", typeof(void), 
							new[]{typeof(int), typeof(string).MakeByRefType(), typeof(string).MakeByRefType(), typeof(int).MakeByRefType()}, 
							new[]{false, true, true, true}) == InterfaceStatus.Valid;

		// If all tests passed then set the game state type
		if (gameStateValid) {
			gameStateType = potentialGameStateType;
		}
		
		return gameStateValid;
	}
	
	bool ValidateCropClass() {
		bool cropValid = true;
		
		// Retrieve the type of the student supplied crop
		System.Type potentialCropType = typeof(Crop);
		
		// Test the variables
		cropValid &= EvaluateVariableStatus(potentialCropType, "UniqueId", typeof(string)) == InterfaceStatus.Valid;
		cropValid &= EvaluateVariableStatus(potentialCropType, "TimeToMature", typeof(float)) == InterfaceStatus.Valid;
		cropValid &= EvaluateVariableStatus(potentialCropType, "DeathChance", typeof(float)) == InterfaceStatus.Valid;
		cropValid &= EvaluateVariableStatus(potentialCropType, "MaturityPercentage", typeof(float)) == InterfaceStatus.Valid;
		cropValid &= EvaluateVariableStatus(potentialCropType, "IntervalBetweenDeathChecks", typeof(float)) == InterfaceStatus.Valid;
		cropValid &= EvaluateVariableStatus(potentialCropType, "IsDead", typeof(bool)) == InterfaceStatus.Valid;
		cropValid &= EvaluateVariableStatus(potentialCropType, "Name", typeof(string)) == InterfaceStatus.Valid;
		cropValid &= EvaluateVariableStatus(potentialCropType, "Cost", typeof(int)) == InterfaceStatus.Valid;
		cropValid &= EvaluateVariableStatus(potentialCropType, "MaxValue", typeof(int)) == InterfaceStatus.Valid;
		
		// Test the properties
		cropValid &= EvaluatePropertyStatus(potentialCropType, "IsMature", typeof(bool)) == InterfaceStatus.Valid;
		cropValid &= EvaluatePropertyStatus(potentialCropType, "Value", typeof(int)) == InterfaceStatus.Valid;
		
		// Test the constructors
		cropValid &= EvaluateConstructorStatus(potentialCropType, new[]{typeof(string)}) == InterfaceStatus.Valid;
		cropValid &= EvaluateConstructorStatus(potentialCropType, new[]{potentialCropType}) == InterfaceStatus.Valid;
		
		// Test the functions
		cropValid &= EvaluateMethodStatus(potentialCropType, "Update", typeof(void), new[]{typeof(float)}, new[]{false}) == InterfaceStatus.Valid;
		
		// If all tests passed then set the crop type
		if (cropValid) {
			cropType = potentialCropType;
		}
		
		return cropValid;
	}
	
	// Update is called once per frame
	void Update () {
		// Clear the selected item if escape is pressed
		if (Input.GetKeyDown(KeyCode.Escape)) {
			menuItemIdSelected = null;
		}
		
		// If the left mouse button is pressed then determine what we need to do
		if (Input.GetMouseButtonDown(0)) {
			// Generate a ray into the scene based on the current mouse location
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			// Perform a raycast through the physics system to determine what the mouse is currently over
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				TileController tile = null;
				
				// We hit a tile so the tile controller is either on the object hit or the parent				
				if (hit.collider.CompareTag(Tags.Tile)) {
					// Retrieve the tile controller for the hit location
					tile = hit.collider.GetComponent<TileController>();
					
					// May validly not have a tile at this point as the tile controller is on the parent.
					// Attempt to retrieve it from the parent.
					if (!tile) {
						tile = hit.collider.GetComponentInParent<TileController>();
					}
				}
				else if (hit.collider.CompareTag(Tags.Crop)) {
					tile = hit.collider.GetComponentInParent<TileController>();
				}
				
				// If a tile was selected then clear or plant a crop as required
				if (tile) {
					// Planting or clearing a crop
					if (menuItemIdSelected != null) {
						if (menuItemIdSelected == UIAction.ClearCrop.ToString()) {
							GameState_ClearCrop(tile);
							tile.RemoveCrop();
						}
						else {
							if (GameState_PlantCrop(menuItemIdSelected, tile)) {
								tile.CropPlanted(selectedCropPrefab);
							}
						}
					} // Otherwise, attempting to harvest the crop
					else {
						GameState_AttemptToHarvestCrop(tile);
						tile.RemoveCrop();
					}
				}
			}
		}
		
		// Update the game state
		GameState_Update(Time.deltaTime);
		
		// Apply the updated game state to the crops
		IDictionary plantedCrops = plantedCropsField.GetValue(gameState) as IDictionary;
		foreach(TileController tile in plantedCrops.Keys) {
			float maturityPercentage;
			bool isMature;
			bool isDead;
			
			// Synchronise the state
			GameState_GetCropState(tile, out maturityPercentage, out isMature, out isDead);
			tile.SetCropState(maturityPercentage, isMature, isDead);
		}
		
		// Update the money
		UIController.SetMoney((int)moneyField.GetValue(gameState));
	}
	
	public List<CropMenuItemInfo> GetCropMenuItemInfos() {
		List<CropMenuItemInfo> menuItemInfos = new List<CropMenuItemInfo>();
		
		// Iterate over the crops and construct the menu item info for each one
		for (int cropIndex = 0; cropIndex < NumCrops; ++cropIndex) {
			CropMenuItemInfo menuItemInfo = new CropMenuItemInfo();

			GameState_GetInfoForCropAtIndex(cropIndex, out menuItemInfo.UniqueId, out menuItemInfo.Name, out menuItemInfo.Cost);			
			
			menuItemInfos.Add(menuItemInfo);
		}
		
		return menuItemInfos;
	}
	
	public static void SelectedCropMenuItem(string menuItemId) {
		// Clear the selected crop prefab
		Instance.selectedCropPrefab = null;
		
		// If the menu item is selected again then deselect it. Otherwise select it.
		if (Instance.menuItemIdSelected == menuItemId) {
			Instance.menuItemIdSelected = null;
		}
		else {
			Instance.menuItemIdSelected = menuItemId;
			
			// Find the index (and the matching prefab) for this crop
			for (int cropIndex = 0; cropIndex < Instance.NumCrops; ++cropIndex) {
				if (Instance.GameState_UniqueIdForCropAtIndex(cropIndex) == menuItemId) {
					Instance.selectedCropPrefab = Instance.CropPrefabs[cropIndex];
					break;
				}
			}
		}
				
		UIController.SetSelectedMenuItem(Instance.menuItemIdSelected);
	}
	
	public static void StartNewGame() {
		Application.LoadLevel(Application.loadedLevel);
	}

	#region Reflection Interfaces
	private void GameState_ClearCrop(MonoBehaviour tile) {
		clearCropMethod.Invoke(gameState, new []{tile});
	}
	
	private bool GameState_PlantCrop(string uniqueId, MonoBehaviour tile) {
		return (bool) plantCropMethod.Invoke(gameState, new []{(object)uniqueId, tile});
	}
	
	private void GameState_AttemptToHarvestCrop(MonoBehaviour tile) {
		attemptToHarvestCropMethod.Invoke(gameState, new []{tile});
	}

	private void GameState_Update(float deltaTime) {
		updateMethod.Invoke(gameState, new[] {(object)deltaTime});
	}

	private void GameState_GetCropState(MonoBehaviour tile, out float maturityPercentage, out bool isMature, out bool isDead) {
		// Assign some values initially - intentionally bad ones so we can pick up on issues quickly
		maturityPercentage = 0.0f;
		isMature = false;
		isDead = false;
		
		// Parameters are setup as an array var so that the out parameters can be read
		object[] parameters = new [] {tile, (object)maturityPercentage, (object)isMature, (object)isDead};
		
		getCropStateMethod.Invoke(gameState, parameters);
		
		maturityPercentage = (float) parameters[1];
		isMature = (bool) parameters[2];
		isDead = (bool) parameters[3];
	}	
	
	private void GameState_GetInfoForCropAtIndex(int cropIndex, out string uniqueId, out string name, out int cost) {
		// Assign some values initially - intentionally bad ones so we can pick up on issues quickly
		uniqueId = "INVALID";
		name = "INVALID";
		cost = 0;
		
		// Parameters are setup as an array var so that the out parameters can be read
		object[] parameters = new [] {(object)cropIndex, (object)uniqueId, (object)name, (object)cost};
		
		getInfoForCropAtIndexMethod.Invoke(gameState, parameters);
		
		uniqueId = (string) parameters[1];
		name = (string) parameters[2];
		cost = (int) parameters[3];
	}			
	
	private string GameState_UniqueIdForCropAtIndex(int index) {
		return (string) uniqueIdForCropAtIndexMethod.Invoke(gameState, new []{(object)index});
	}
	
	private static InterfaceStatus EvaluateVariableStatus(Type classType, string name, Type requiredType) {
		// Check for the presence of the variable using reflection
		try {
			// Search for public variables on the class
			FieldInfo fieldInfo = classType.GetField(name, SearchFlags);
			
			// If the variable was found check the type
			if (fieldInfo.FieldType == requiredType) {
				return InterfaceStatus.Valid;
			}
			else {
				Debug.LogError("The variable " + name + " on the " + classType.Name + " has the wrong type.");
				return InterfaceStatus.WrongSignature;
			}
		}
		catch (NullReferenceException) {
			Debug.LogError("The variable " + name + " could not be found on the " + classType.Name + ".");
			return InterfaceStatus.Missing;
		}
	}
	
	private static InterfaceStatus EvaluatePropertyStatus(Type classType, string name, Type requiredType) {
		// Check for the presence of the property using reflection
		try {
			// Search for public properties on the class
			PropertyInfo propInfo = classType.GetProperty(name, SearchFlags);
			
			// If the property was found check the type
			if (propInfo.PropertyType == requiredType) {
				return InterfaceStatus.Valid;
			}
			else {
				Debug.LogError("The property " + name + " on the " + classType.Name + " has the wrong type.");
				return InterfaceStatus.WrongSignature;
			}
		}
		catch (NullReferenceException) {
			Debug.LogError("The property " + name + " could not be found on the " + classType.Name + ".");
			return InterfaceStatus.Missing;
		}
	}
	
	private static InterfaceStatus EvaluateConstructorStatus(Type classType, Type[] requiredParameters) {
		// Check for the presence of the constructor using reflection
		try {
			// Search for the constructor on the class
			ConstructorInfo constructorInfo = classType.GetConstructor(requiredParameters);
			
			ParameterInfo[] parameters = constructorInfo.GetParameters();

			// Constructor has parameters?
			if (parameters.Length > 0) {
				// Constructor has parameters but none were expected
				if (requiredParameters == null) {
					Debug.LogError("A constructor for " + classType.Name + " does not match the required syntax.");
					return InterfaceStatus.WrongSignature;
				}
				
				// Wrong number of parameters?
				if (parameters.Length  != requiredParameters.Length) {
					Debug.LogError("A constructor for " + classType.Name + " does not match the required syntax.");
					return InterfaceStatus.WrongSignature;
				}
				
				// Test each parameter to make sure the type is correct
				for(int paramIndex = 0; paramIndex < parameters.Length; ++paramIndex) {
					if (parameters[paramIndex].ParameterType != requiredParameters[paramIndex]) {
						Debug.LogError("A constructor for " + classType.Name + " does not match the required syntax.");
						return InterfaceStatus.WrongSignature;
					}
				}
			} // Constructor has no parameters?
			else {
				if ((requiredParameters != null) && (requiredParameters.Length > 0)) {
					Debug.LogError("A constructor for " + classType.Name + " does not match the required syntax.");
					return InterfaceStatus.WrongSignature;
				}
			}
			
			return InterfaceStatus.Valid;
		}
		catch (NullReferenceException) {
			// Build the parameter list
			string parameterList = "";
			foreach(Type type in requiredParameters) {
				if (parameterList.Length > 0) {
					parameterList += ", ";
				}
				parameterList += type.Name;
			}
			
			Debug.LogError("A constructor for " + classType.Name + " could not be found that took the following parameters: " + parameterList);
			return InterfaceStatus.Missing;
		}
	}
	
	private static InterfaceStatus EvaluateMethodStatus(Type classType, string name, Type requiredReturnType, 
														Type[] requiredParameters, bool[] isOutParameter) {
		// Check for the presence of the method using reflection
		try {
			// Search for the method on the class
			MethodInfo methodInfo = classType.GetMethod(name, SearchFlags);
			
			// If the method was found check the return type
			if (methodInfo.ReturnType == requiredReturnType) {
				ParameterInfo[] parameters = methodInfo.GetParameters();

				// Method has parameters?
				if (parameters.Length > 0) {
					// Method has parameters but none were expected
					if (requiredParameters == null) {
						Debug.LogError("The method " + name + " on the " + classType.Name + " does not match the required syntax.");
						return InterfaceStatus.WrongSignature;
					}
					
					// Wrong number of parameters?
					if (parameters.Length  != requiredParameters.Length) {
						Debug.LogError("The method " + name + " on the " + classType.Name + " does not match the required syntax.");
						return InterfaceStatus.WrongSignature;
					}
					
					// Test each parameter to make sure the type is correct
					for(int paramIndex = 0; paramIndex < parameters.Length; ++paramIndex) {
						if (parameters[paramIndex].ParameterType != requiredParameters[paramIndex]) {
							Debug.LogError("The method " + name + " on the " + classType.Name + " does not match the required syntax.");
							return InterfaceStatus.WrongSignature;
						}
						
						if (parameters[paramIndex].IsOut != isOutParameter[paramIndex]) {
							Debug.LogError("The method " + name + " on the " + classType.Name + " does not match the required syntax.");
							return InterfaceStatus.WrongSignature;
						}
					}
				} // Method has no parameters?
				else {
					if ((requiredParameters != null) && (requiredParameters.Length > 0)) {
						Debug.LogError("The method " + name + " on the " + classType.Name + " does not match the required syntax.");
						return InterfaceStatus.WrongSignature;
					}
				}
				
				return InterfaceStatus.Valid;
			}
			else {
				Debug.LogError("The method " + name + " on the " + classType.Name + " does not match the required syntax.");
				return InterfaceStatus.WrongSignature;
			}
		}
		catch (NullReferenceException) {
			Debug.LogError("The method " + name + " could not be found on the " + classType.Name + ".");
			return InterfaceStatus.Missing;
		}
	}
	#endregion
}
