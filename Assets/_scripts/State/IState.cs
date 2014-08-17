using System.Collections.Generic;

public interface IState
{
	bool Save ();
	void HoldAutoSave (bool hold);
	void Reset (bool resetRules = false);

	#region CONFIG
	int VersionMajor { get; set; }
	int VersionMiddle { get; set; }
	int VersionMinor { get; set; }
	#endregion

	#region RULES
	int TotalDays { get; set; }
	int MaxAP { get; set; }

	int EnterSectorAPCost { get; set; }
	int LootCharges { get; set; }
	List<SectorParameters> SectorsParameters { get; set; }

	List<Artifact> Artifacts { get; set; }
	int AnalyzeArtifactAPCost { get; set; }

	int FixEngineAPCost { get; set; }
	SerializableDictionary<BreakageType, int[]> FixEngineRequirements { get; set; }

	int FuelSynthAPCost { get; set; }
	int FuelSynthGrace { get; set; }
	#endregion

	#region STATE
	GameStatus GameStatus { get; set; }

	string CurrentQuest { get; set; }
	SerializableDictionary<string, string> QuestRecords { get; set; }

	BreakageType BreakageType { get; set; }
	bool EngineFixed { get; set; }

	int[] FuelSynthFormula { get; set; }
	List<int[]> FuelSynthProbes { get; set; }
	bool FuelSynthed { get; set; }

	int CurrentDay { get; set; }
	int CurrentAP { get; set; }

	int MineralA { get; set; }
	int MineralB { get; set; }
	int MineralC { get; set; }

	int Wiring { get; set; }
	int Alloy { get; set; }
	int Chips { get; set; }
	#endregion
}