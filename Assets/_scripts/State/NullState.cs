using System.Collections.Generic;

public class NullState : IState
{
	public bool Save () { return true; }
	public void HoldAutoSave (bool hold) { }
	public void Reset (bool resetRules = false) { }

	#region CONFIG
	public int VersionMajor { get; set; }
	public int VersionMiddle { get; set; }
	public int VersionMinor { get; set; }
	#endregion

	#region RULES
	public List<Quest> QuestRecords { get; set; }
	public List<Phrase> PhraseRecords { get; set; }

	public int TotalDays { get; set; }
	public int MaxAP { get; set; }

	public int EnterSectorAPCost { get; set; }
	public int LootCharges { get; set; }
	public List<SectorParameters> SectorsParameters { get; set; }

	public List<Artifact> Artifacts { get; set; }
	public int AnalyzeArtifactAPCost { get; set; }

	public int FixEngineAPCost { get; set; }
	public SerializableDictionary<BreakageType, int[]> FixEngineRequirements { get; set; }

	public int FuelSynthAPCost { get; set; }
	public int FuelSynthGrace { get; set; }
	#endregion

	#region STATE
	public GameStatus GameStatus { get; set; }

	public BreakageType BreakageType { get; set; }
	public bool EngineFixed { get; set; }

	public int[] FuelSynthFormula { get; set; }
	public List<int[]> FuelSynthProbes { get; set; }
	public bool FuelSynthed { get; set; }

	public int CurrentDay { get; set; }
	public int CurrentAP { get; set; }

	public int MineralA { get; set; }
	public int MineralB { get; set; }
	public int MineralC { get; set; }

	public int Wiring { get; set; }
	public int Alloy { get; set; }
	public int Chips { get; set; }
	#endregion
}