using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("GameState")]
public abstract class BaseState : IState
{
	[XmlIgnore]
	protected static bool preventSave;

	#region CONFIG
	private int _versionMajor;
	public int VersionMajor
	{
		get { return _versionMajor; }
		set { _versionMajor = value; }
	}

	private int _versionMiddle;
	public int VersionMiddle
	{
		get { return _versionMiddle; }
		set { _versionMiddle = value; }
	}

	private int _versionMinor;
	public int VersionMinor
	{
		get { return _versionMinor; }
		set { _versionMinor = value; }
	}
	#endregion

	#region RULES
	private List<Quest> _questRecords;
	public List<Quest> QuestRecords
	{
		get { return _questRecords; }
		set { _questRecords = value; Save(); }
	}

	private List<Phrase> _phrases;
	public List<Phrase> Phrases
	{
		get { return _phrases; }
		set { _phrases = value; Save(); }
	}

	private List<JournalRecord> _journalRecords;
	public List<JournalRecord> JournalRecords
	{
		get { return _journalRecords; }
		set { _journalRecords = value; Save(); }
	}

	private int _totalDays;
	public int TotalDays
	{
		get { return _totalDays; }
		set { _totalDays = value; Save(); }
	}

	private int _maxAP;
	public int MaxAP
	{
		get { return _maxAP; }
		set { _maxAP = value; Save(); }
	}

	private int _enterSectorAPCost;
	public int EnterSectorAPCost
	{
		get { return _enterSectorAPCost; }
		set { _enterSectorAPCost = value; Save(); }
	}

	private int _lootCharges;
	public int LootCharges
	{
		get { return _lootCharges; }
		set { _lootCharges = value; Save(); }
	}

	private List<SectorParameters> _sectorsParameters;
	public List<SectorParameters> SectorsParameters
	{
		get { return _sectorsParameters; }
		set { _sectorsParameters = value; Save(); }
	}

	private List<Artifact> _artifacts;
	public List<Artifact> Artifacts
	{
		get { return _artifacts; }
		set { _artifacts = value; Save(); }
	}

	private int _analyzeArtifactAPCost;
	public int AnalyzeArtifactAPCost
	{
		get { return _analyzeArtifactAPCost; }
		set { _analyzeArtifactAPCost = value; Save(); }
	}

	private int _fixEngineAPCost;
	public int FixEngineAPCost
	{
		get { return _fixEngineAPCost; }
		set { _fixEngineAPCost = value; Save(); }
	}

	private SerializableDictionary<BreakageType, int[]> _fixEngineRequirements;
	public SerializableDictionary<BreakageType, int[]> FixEngineRequirements
	{
		get { return _fixEngineRequirements; }
		set { _fixEngineRequirements = value; Save(); }
	}

	private int _fuelSynthAPCost;
	public int FuelSynthAPCost
	{
		get { return _fuelSynthAPCost; }
		set { _fuelSynthAPCost = value; Save(); }
	}

	private int _fuelSynthGrace;
	public int FuelSynthGrace
	{
		get { return _fuelSynthGrace; }
		set { _fuelSynthGrace = value; Save(); }
	}

	private int _fuelSynthSumm;
	public int FuelSynthSumm
	{
		get { return _fuelSynthSumm; }
		set { _fuelSynthSumm = value; Save(); }
	}
	#endregion

	#region STATE
	private GameStatus _gameStatus;
	public GameStatus GameStatus
	{
		get { return _gameStatus; }
		set { _gameStatus = value; Save(); }
	}

	private BreakageType _breakageType;
	public BreakageType BreakageType
	{
		get { return _breakageType; }
		set { _breakageType = value; Save(); }
	}

	private bool _engineFixed;
	public bool EngineFixed
	{
		get { return _engineFixed; }
		set
		{
			if (value && value != _engineFixed)
				Events.RaiseEngineFixed();
			_engineFixed = value; Save();
		}
	}

	private int[] _synthFuelFormula;
	public int[] FuelSynthFormula
	{
		get { return _synthFuelFormula; }
		set { _synthFuelFormula = value; Save(); }
	}

	private List<int[]> _fuelSynthProbes;
	public List<int[]> FuelSynthProbes
	{
		get { return _fuelSynthProbes; }
		set { _fuelSynthProbes = value; Save(); }
	}

	private bool _fuelSynthed;
	public bool FuelSynthed
	{
		get { return _fuelSynthed; }
		set
		{
			if (value && value != _fuelSynthed)
				Events.RaiseFuelSynthed();
			_fuelSynthed = value; Save();
		}
	}

	private int _currentDay;
	public int CurrentDay
	{
		get { return _currentDay; }
		set { _currentDay = value; Save(); }
	}

	private int _currentAP;
	public int CurrentAP
	{
		get { return _currentAP; }
		set { _currentAP = value; Save(); }
	}

	private int _mineralA;
	public int MineralA
	{
		get { return _mineralA; }
		set { _mineralA = value; Save(); }
	}

	private int _mineralB;
	public int MineralB
	{
		get { return _mineralB; }
		set { _mineralB = value; Save(); }
	}

	private int _mineralC;
	public int MineralC
	{
		get { return _mineralC; }
		set { _mineralC = value; Save(); }
	}

	private int _wiring;
	public int Wiring
	{
		get { return _wiring; }
		set { _wiring = value; Save(); }
	}

	private int _alloy;
	public int Alloy
	{
		get { return _alloy; }
		set { _alloy = value; Save(); }
	}

	private int _chips;
	public int Chips
	{
		get { return _chips; }
		set { _chips = value; Save(); }
	}
	#endregion

	public virtual bool Save ()
	{
		if (preventSave) return false;

		Events.RaiseStateUpdated();

		VersionMajor = GlobalConfig.VERSION_MAJOR;
		VersionMiddle = GlobalConfig.VERSION_MIDDLE;
		VersionMinor = GlobalConfig.VERSION_MINOR;

		return true;
	}

	public void HoldAutoSave (bool hold)
	{
		preventSave = hold;
		if (!hold) Save();
	}

	public void Reset (bool resetRules = false)
	{
		bool autoSaveWasTurnedOff = preventSave;
		preventSave = true;

		#region RULES
		if (resetRules)
		{
			QuestRecords = new List<Quest>()
			{
				new Quest("Abroad", new Requirements(minAP: 5, day: 2)),
				new Quest("Dalia", new Requirements(minAP: 4, minDay: 5, completedQuests: new List<string>() {"QuestAbroad#2"})),
				new Quest("Echoes", new Requirements(day: 8)),

				new Quest("Test1"),
				new Quest("Test2"),
				new Quest("Test3"),
			};

			Phrases = new List<Phrase>()
			{
				new Phrase("Phrase1", new Requirements(day: 1)),
				new Phrase("Phrase2", new Requirements(day: 1)),
				new Phrase("Phrase3", new Requirements(day: 1)),
				new Phrase("Phrase4", new Requirements(day: 1)),
				new Phrase("Phrase5", new Requirements(day: 1)),

				new Phrase("Phrase6", new Requirements(day: 2)),
				new Phrase("Phrase7", new Requirements(day: 2)),
				new Phrase("Phrase8", new Requirements(day: 2)),
				new Phrase("Phrase9", new Requirements(day: 2)),
				new Phrase("Phrase10", new Requirements(day: 2)),

				new Phrase("Phrase11", new Requirements(day: 3)),
				new Phrase("Phrase12", new Requirements(day: 3)),
				new Phrase("Phrase13", new Requirements(day: 3)),
				new Phrase("Phrase14", new Requirements(day: 3)),
				new Phrase("Phrase15", new Requirements(day: 3)),

				new Phrase("Phrase16", new Requirements(day: 4)),
				new Phrase("Phrase17", new Requirements(day: 4)),
				new Phrase("Phrase18", new Requirements(day: 4)),
				new Phrase("Phrase19", new Requirements(day: 4)),
				new Phrase("Phrase20", new Requirements(day: 4)),

				new Phrase("Phrase21", new Requirements(day: 5)),
				new Phrase("Phrase22", new Requirements(day: 5)),
				new Phrase("Phrase23", new Requirements(day: 5)),
				new Phrase("Phrase24", new Requirements(day: 5)),
				new Phrase("Phrase25", new Requirements(day: 5)),

				new Phrase("Phrase26", new Requirements(day: 6)),
				new Phrase("Phrase27", new Requirements(day: 6)),
				new Phrase("Phrase28", new Requirements(day: 6)),
				new Phrase("Phrase29", new Requirements(day: 6)),
				new Phrase("Phrase30", new Requirements(day: 6)),

				new Phrase("Phrase31", new Requirements(day: 7)),
				new Phrase("Phrase32", new Requirements(day: 7)),
				new Phrase("Phrase33", new Requirements(day: 7)),
				new Phrase("Phrase34", new Requirements(day: 7)),
				new Phrase("Phrase35", new Requirements(day: 7)),
			};

			JournalRecords = new List<JournalRecord>()
			{
				new JournalRecord("Journal1", new Requirements(day: 1)),
				new JournalRecord("Journal2", new Requirements(day: 2, completedQuests: new List<string>() {"QuestAbroad#8"})),
				new JournalRecord("Journal3", new Requirements(analyzedArtifacts: new List<string>() {"Artifact1"})),
				new JournalRecord("Journal4", new Requirements(day: 3, maxAP: 5)),
				new JournalRecord("Journal5", new Requirements(day: 2)),
				new JournalRecord("Journal6", new Requirements(day: 3)),
				new JournalRecord("Journal7", new Requirements(day: 4)),
				new JournalRecord("Journal8", new Requirements(day: 5)),
				new JournalRecord("Journal9", new Requirements(day: 6)),
				new JournalRecord("Journal10", new Requirements(day: 7)),
				new JournalRecord("Journal11", new Requirements(day: 8)),
			};

			TotalDays = 8;
			MaxAP = 10;

			EnterSectorAPCost = 3;
			LootCharges = 3;
			SectorsParameters = new List<SectorParameters> { 
				new SectorParameters(1, 10, 0, 3, 3), 
				new SectorParameters(2, 15, 3, 0, 3), 
				new SectorParameters(3, 8, 3, 3, 0), 
				new SectorParameters(4, 5, 2, 2, 2) 
			};

			Artifacts = new List<Artifact> { 
				new Artifact("Artifact1", 0,	BreakageType.BRK1, 30, 20, 15), 
				new Artifact("Artifact2", 0,	null,              05, 00, 15), 
				new Artifact("Artifact3", 0,	null,              15, 10, 00), 
				new Artifact("Artifact4", 0,	null,              00, 30, 00), 
				new Artifact("Artifact5", 0,	null,              00, 00, 35), 
				new Artifact("Artifact6", 0,	BreakageType.BRK2, 40, 10, 15), 
				new Artifact("Artifact7", 0,	null,              20, 05, 15), 
				new Artifact("Artifact8", 0,	null,              15, 00, 15), 
				new Artifact("Artifact9", 0,	null,              05, 00, 30), 
				new Artifact("Artifact10", 0,	BreakageType.BRK3, 00, 50, 05), 
				new Artifact("Artifact11", 0,	null,              15, 00, 15), 
				new Artifact("Artifact12", 0,	null,              20, 00, 00),
				new Artifact("Artifact13", 0,	null,              10, 10, 15),
				new Artifact("Artifact14", 0,	null,              05, 15, 10),
				new Artifact("Artifact15", 0,	BreakageType.BRK4, 30, 10, 20),
			};
			AnalyzeArtifactAPCost = 2;

			FixEngineAPCost = 5;
			FixEngineRequirements = new SerializableDictionary<BreakageType, int[]>
			{
				{BreakageType.BRK1, new int[3] {30, 35, 40}},
				{BreakageType.BRK2, new int[3] {35, 40, 30}},
				{BreakageType.BRK3, new int[3] {40, 30, 35}},
				{BreakageType.BRK4, new int[3] {30, 40, 35}},
			};

			FuelSynthAPCost = 2;
			FuelSynthGrace = 1;
			FuelSynthSumm = 9;
		}
		#endregion

		#region STATE
		GameStatus = GameStatus.FirstLaunch;

		if (QuestRecords != null) foreach (var quest in QuestRecords) quest.ResetProgress();

		if (JournalRecords != null) foreach (var record in JournalRecords) record.AssignedDay = 0;

		BreakageType[] possibleBRK = (BreakageType[])Enum.GetValues(typeof(BreakageType));
		BreakageType = possibleBRK[Rand.RND.Next(0, 4)];
		EngineFixed = false;

		FuelSynthFormula = new int[3];
		while ((FuelSynthFormula[0] + FuelSynthFormula[1] + FuelSynthFormula[2]) != FuelSynthSumm)
		{
			FuelSynthFormula[0] = Rand.RND.Next(1, 10);
			FuelSynthFormula[1] = Rand.RND.Next(1, 10);
			FuelSynthFormula[2] = Rand.RND.Next(1, 10);
		}
		FuelSynthProbes = new List<int[]>();
		FuelSynthed = false;

		CurrentDay = 1;
		CurrentAP = MaxAP;

		MineralA = 0;
		MineralB = 0;
		MineralC = 0;

		Wiring = 0;
		Alloy = 0;
		Chips = 0;

		foreach (var artifact in Artifacts)
			artifact.Status = ArtifactStatus.NotFound;
		#endregion

		preventSave = autoSaveWasTurnedOff;
		this.Save();
	}
}