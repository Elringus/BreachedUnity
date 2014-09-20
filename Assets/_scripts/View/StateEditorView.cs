using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class StateEditorView : BaseView
{
	private float width = 800;

	private float LABEL_W = 300;

	private float NAV_W = 155;

	private float SINGLE_W = 458;
	private float DOUBLE_W = 226.55f;
	private float TRIPLE_W = 149.8f;
	private float QUADRO_W = 111.5f;
	private float TETTRO_W = 88.4f;

	private float CONTROL_W = 633.5f;

	private Vector2 scrollPosition;
	private StateEditorPage selectedPage;

	protected override void Awake ()
	{
		base.Awake();

		State.HoldAutoSave(true);
	}

	private void SaveAndExit ()
	{
		State.HoldAutoSave(false);
		SwitchView(ViewType.MainMenu);
	}

	protected override void Update ()
	{
		base.Update();

		if (Screen.width < 800) width = Screen.width;
		else width = 800;

		LABEL_W = width / 2.667f * (width < 800 ? .97f : 1);

		NAV_W = width / 5.161f * (width < 800 ? .97f : 1);

		SINGLE_W = width / 1.747f * (width < 800 ? .97f : 1);
		DOUBLE_W = width / 3.531f * (width < 800 ? .97f : 1);
		TRIPLE_W = width / 5.340f * (width < 800 ? .97f : 1);
		QUADRO_W = width / 7.175f * (width < 800 ? .97f : 1);
		TETTRO_W = width / 9.050f * (width < 800 ? .97f : 1);

		CONTROL_W = width / 1.263f * (width < 800 ? .9f : 1);
	}

	private void OnGUI ()
	{
		GUI.skin.customStyles[0] = new GUIStyle(GUI.skin.label);
		GUI.skin.customStyles[0].alignment = TextAnchor.MiddleRight;
		GUI.skin.customStyles[0].fixedWidth = LABEL_W;
		GUI.skin.customStyles[0].padding.right = 10;

		GUI.Box(new Rect(Screen.width / 2 - width / 2 + 4.5f, 25, width - 10, Screen.height - 64), "");
		GUI.Box(new Rect(Screen.width / 2 - width / 2 + 5, 27, width - 10, 3), "");
		GUI.Box(new Rect(Screen.width / 2 - width / 2 + 5, Screen.height - 38, width - 10, 3), "");

		GUILayout.BeginArea(new Rect(Screen.width / 2 - width / 2, 0, width, Screen.height));

		GUILayout.Space(2);

		GUILayout.BeginHorizontal();
		if (GUILayout.Button(selectedPage == StateEditorPage.Main ? "<b>❖ Main</b>" : "Main", GUILayout.Width(NAV_W))) selectedPage = StateEditorPage.Main;
		if (GUILayout.Button(selectedPage == StateEditorPage.Journal ? "<b>❖ Journal</b>" : "Journal", GUILayout.Width(NAV_W))) selectedPage = StateEditorPage.Journal;
		if (GUILayout.Button(selectedPage == StateEditorPage.Quests ? "<b>❖ Quests</b>" : "Quests", GUILayout.Width(NAV_W))) selectedPage = StateEditorPage.Quests;
		if (GUILayout.Button(selectedPage == StateEditorPage.Artifacts ? "<b>❖ Artifacts</b>" : "Artifacts", GUILayout.Width(NAV_W))) selectedPage = StateEditorPage.Artifacts;
		if (GUILayout.Button(selectedPage == StateEditorPage.Phrases ? "<b>❖ Phrases</b>" : "Phrases", GUILayout.Width(NAV_W))) selectedPage = StateEditorPage.Phrases;
		GUILayout.EndHorizontal();

		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width - 5), GUILayout.Height(Screen.height - 64));

		GUILayout.Space(10);

		if (selectedPage == StateEditorPage.Main)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Save data verion: ", GUI.skin.customStyles[0]);
			GUILayout.Label(string.Format("{0}.{1}.{2}", State.VersionMajor, State.VersionMiddle, State.VersionMinor), GUILayout.Width(SINGLE_W));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Game status:</color> ", GUI.skin.customStyles[0]);
			GUILayout.Label(string.Format("<b>{0}</b>", State.GameStatus.ToString(), GUILayout.Width(SINGLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.Space(20);

			GUILayout.BeginHorizontal();
			GUILayout.Label("Total days: ", GUI.skin.customStyles[0]);
			State.TotalDays = int.Parse(GUILayout.TextField(State.TotalDays.ToString(), GUILayout.Width(SINGLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Current day:</color> ", GUI.skin.customStyles[0]);
			State.CurrentDay = int.Parse(GUILayout.TextField(State.CurrentDay.ToString(), GUILayout.Width(SINGLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.Space(20);

			GUILayout.BeginHorizontal();
			GUILayout.Label("Max AP: ", GUI.skin.customStyles[0]);
			State.MaxAP = int.Parse(GUILayout.TextField(State.MaxAP.ToString(), GUILayout.Width(SINGLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Current AP:</color> ", GUI.skin.customStyles[0]);
			State.CurrentAP = int.Parse(GUILayout.TextField(State.CurrentAP.ToString(), GUILayout.Width(SINGLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.Space(20);

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix engine AP cost: ", GUI.skin.customStyles[0]);
			State.FixEngineAPCost = int.Parse(GUILayout.TextField(State.FixEngineAPCost.ToString(), GUILayout.Width(SINGLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix BRK1 requirements (wiring, alloy, chips): ", GUI.skin.customStyles[0]);
			State.FixEngineRequirements[BreakageType.BRK1][0] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK1][0].ToString(), GUILayout.Width(TRIPLE_W)));
			State.FixEngineRequirements[BreakageType.BRK1][1] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK1][1].ToString(), GUILayout.Width(TRIPLE_W)));
			State.FixEngineRequirements[BreakageType.BRK1][2] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK1][2].ToString(), GUILayout.Width(TRIPLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix BRK2 requirements (wiring, alloy, chips): ", GUI.skin.customStyles[0]);
			State.FixEngineRequirements[BreakageType.BRK2][0] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK2][0].ToString(), GUILayout.Width(TRIPLE_W)));
			State.FixEngineRequirements[BreakageType.BRK2][1] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK2][1].ToString(), GUILayout.Width(TRIPLE_W)));
			State.FixEngineRequirements[BreakageType.BRK2][2] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK2][2].ToString(), GUILayout.Width(TRIPLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix BRK3 requirements (wiring, alloy, chips): ", GUI.skin.customStyles[0]);
			State.FixEngineRequirements[BreakageType.BRK3][0] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK3][0].ToString(), GUILayout.Width(TRIPLE_W)));
			State.FixEngineRequirements[BreakageType.BRK3][1] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK3][1].ToString(), GUILayout.Width(TRIPLE_W)));
			State.FixEngineRequirements[BreakageType.BRK3][2] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK3][2].ToString(), GUILayout.Width(TRIPLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix BRK4 requirements (wiring, alloy, chips): ", GUI.skin.customStyles[0]);
			State.FixEngineRequirements[BreakageType.BRK4][0] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK4][0].ToString(), GUILayout.Width(TRIPLE_W)));
			State.FixEngineRequirements[BreakageType.BRK4][1] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK4][1].ToString(), GUILayout.Width(TRIPLE_W)));
			State.FixEngineRequirements[BreakageType.BRK4][2] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK4][2].ToString(), GUILayout.Width(TRIPLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Current breakage type:</color> ", GUI.skin.customStyles[0]);
			if (GUILayout.Button(State.BreakageType == BreakageType.BRK1 ? "<b>❖ BRK1</b>" : "BRK1", GUILayout.Width(QUADRO_W))) State.BreakageType = BreakageType.BRK1;
			if (GUILayout.Button(State.BreakageType == BreakageType.BRK2 ? "<b>❖ BRK2</b>" : "BRK2", GUILayout.Width(QUADRO_W))) State.BreakageType = BreakageType.BRK2;
			if (GUILayout.Button(State.BreakageType == BreakageType.BRK3 ? "<b>❖ BRK3</b>" : "BRK3", GUILayout.Width(QUADRO_W))) State.BreakageType = BreakageType.BRK3;
			if (GUILayout.Button(State.BreakageType == BreakageType.BRK4 ? "<b>❖ BRK4</b>" : "BRK4", GUILayout.Width(QUADRO_W))) State.BreakageType = BreakageType.BRK4;
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Current Wiring, Alloy, Chips:</color> ", GUI.skin.customStyles[0]);
			State.Wiring = int.Parse(GUILayout.TextField(State.Wiring.ToString(), GUILayout.Width(TRIPLE_W)));
			State.Alloy = int.Parse(GUILayout.TextField(State.Alloy.ToString(), GUILayout.Width(TRIPLE_W)));
			State.Chips = int.Parse(GUILayout.TextField(State.Chips.ToString(), GUILayout.Width(TRIPLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Engine is fixed:</color> ", GUI.skin.customStyles[0]);
			State.EngineFixed = GUILayout.Toggle(State.EngineFixed, "");
			GUILayout.EndHorizontal();

			GUILayout.Space(20);

			GUILayout.BeginHorizontal();
			GUILayout.Label("Synth fuel AP cost: ", GUI.skin.customStyles[0]);
			State.FuelSynthAPCost = int.Parse(GUILayout.TextField(State.FuelSynthAPCost.ToString(), GUILayout.Width(SINGLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Synth fuel grace: ", GUI.skin.customStyles[0]);
			State.FuelSynthGrace = int.Parse(GUILayout.TextField(State.FuelSynthGrace.ToString(), GUILayout.Width(SINGLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Synth fuel summ: ", GUI.skin.customStyles[0]);
			State.FuelSynthSumm = int.Parse(GUILayout.TextField(State.FuelSynthSumm.ToString(), GUILayout.Width(SINGLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Current fuel synth formula (A, B, C):</color> ", GUI.skin.customStyles[0]);
			State.FuelSynthFormula[0] = int.Parse(GUILayout.TextField(State.FuelSynthFormula[0].ToString(), GUILayout.Width(TRIPLE_W)));
			State.FuelSynthFormula[1] = int.Parse(GUILayout.TextField(State.FuelSynthFormula[1].ToString(), GUILayout.Width(TRIPLE_W)));
			State.FuelSynthFormula[2] = int.Parse(GUILayout.TextField(State.FuelSynthFormula[2].ToString(), GUILayout.Width(TRIPLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Current minerals count (A, B, C):</color> ", GUI.skin.customStyles[0]);
			State.MineralA = int.Parse(GUILayout.TextField(State.MineralA.ToString(), GUILayout.Width(TRIPLE_W)));
			State.MineralB = int.Parse(GUILayout.TextField(State.MineralB.ToString(), GUILayout.Width(TRIPLE_W)));
			State.MineralC = int.Parse(GUILayout.TextField(State.MineralC.ToString(), GUILayout.Width(TRIPLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Fuel is synthed:</color> ", GUI.skin.customStyles[0]);
			State.FuelSynthed = GUILayout.Toggle(State.FuelSynthed, "");
			GUILayout.EndHorizontal();

			GUILayout.Space(20);

			GUILayout.BeginHorizontal();
			GUILayout.Label("Enter sector AP cost: ", GUI.skin.customStyles[0]);
			State.EnterSectorAPCost = int.Parse(GUILayout.TextField(State.EnterSectorAPCost.ToString(), GUILayout.Width(SINGLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Loot charges: ", GUI.skin.customStyles[0]);
			State.LootCharges = int.Parse(GUILayout.TextField(State.LootCharges.ToString(), GUILayout.Width(SINGLE_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Sector 1 parameters (loot spots, A, B, C): ", GUI.skin.customStyles[0]);
			State.SectorsParameters.Find(x => x.SectorID == 1).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).LootSpotCount.ToString(), GUILayout.Width(QUADRO_W)));
			State.SectorsParameters.Find(x => x.SectorID == 1).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).MineralA.ToString(), GUILayout.Width(QUADRO_W)));
			State.SectorsParameters.Find(x => x.SectorID == 1).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).MineralB.ToString(), GUILayout.Width(QUADRO_W)));
			State.SectorsParameters.Find(x => x.SectorID == 1).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).MineralC.ToString(), GUILayout.Width(QUADRO_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Sector 2 parameters (loot spots, A, B, C): ", GUI.skin.customStyles[0]);
			State.SectorsParameters.Find(x => x.SectorID == 2).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).LootSpotCount.ToString(), GUILayout.Width(QUADRO_W)));
			State.SectorsParameters.Find(x => x.SectorID == 2).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).MineralA.ToString(), GUILayout.Width(QUADRO_W)));
			State.SectorsParameters.Find(x => x.SectorID == 2).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).MineralB.ToString(), GUILayout.Width(QUADRO_W)));
			State.SectorsParameters.Find(x => x.SectorID == 2).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).MineralC.ToString(), GUILayout.Width(QUADRO_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Sector 3 parameters (loot spots, A, B, C): ", GUI.skin.customStyles[0]);
			State.SectorsParameters.Find(x => x.SectorID == 3).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).LootSpotCount.ToString(), GUILayout.Width(QUADRO_W)));
			State.SectorsParameters.Find(x => x.SectorID == 3).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).MineralA.ToString(), GUILayout.Width(QUADRO_W)));
			State.SectorsParameters.Find(x => x.SectorID == 3).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).MineralB.ToString(), GUILayout.Width(QUADRO_W)));
			State.SectorsParameters.Find(x => x.SectorID == 3).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).MineralC.ToString(), GUILayout.Width(QUADRO_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Sector 4 parameters (loot spots, A, B, C): ", GUI.skin.customStyles[0]);
			State.SectorsParameters.Find(x => x.SectorID == 4).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).LootSpotCount.ToString(), GUILayout.Width(QUADRO_W)));
			State.SectorsParameters.Find(x => x.SectorID == 4).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).MineralA.ToString(), GUILayout.Width(QUADRO_W)));
			State.SectorsParameters.Find(x => x.SectorID == 4).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).MineralB.ToString(), GUILayout.Width(QUADRO_W)));
			State.SectorsParameters.Find(x => x.SectorID == 4).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).MineralC.ToString(), GUILayout.Width(QUADRO_W)));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Analyze artifact AP cost: ", GUI.skin.customStyles[0]);
			State.AnalyzeArtifactAPCost = int.Parse(GUILayout.TextField(State.AnalyzeArtifactAPCost.ToString(), GUILayout.Width(SINGLE_W)));
			GUILayout.EndHorizontal();
		}

		if (selectedPage == StateEditorPage.Journal)
		{
			for (int i = 0; i < State.JournalRecords.Count; i++)
			{
				var record = State.JournalRecords[i];

				GUILayout.BeginHorizontal();
				GUILayout.Label("Journal record ID: ", GUI.skin.customStyles[0]);
				record.ID = GUILayout.TextField(record.ID, GUILayout.Width(SINGLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Journal record text: ", GUI.skin.customStyles[0]);
				GUILayout.TextArea(Text.Get(record.ID), GUILayout.Width(SINGLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Day requirements (min, specific, max): ", GUI.skin.customStyles[0]);
				record.Requirements.MinDay = int.Parse(GUILayout.TextField(record.Requirements.MinDay.ToString(), GUILayout.Width(TRIPLE_W)));
				record.Requirements.Day = int.Parse(GUILayout.TextField(record.Requirements.Day.ToString(), GUILayout.Width(TRIPLE_W)));
				record.Requirements.MaxDay = int.Parse(GUILayout.TextField(record.Requirements.MaxDay.ToString(), GUILayout.Width(TRIPLE_W)));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("AP requirements (min, max): ", GUI.skin.customStyles[0]);
				record.Requirements.MinAP = int.Parse(GUILayout.TextField(record.Requirements.MinAP.ToString(), GUILayout.Width(DOUBLE_W)));
				record.Requirements.MaxAP = int.Parse(GUILayout.TextField(record.Requirements.MaxAP.ToString(), GUILayout.Width(DOUBLE_W)));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Quest requirements: ", GUI.skin.customStyles[0]);
				if (record.Requirements.CompletedQuests.Count < 1)
					record.Requirements.CompletedQuests = new List<string>() { "" };
				record.Requirements.CompletedQuests[0] = GUILayout.TextField(record.Requirements.CompletedQuests[0], GUILayout.Width(TRIPLE_W));
				if (record.Requirements.CompletedQuests.Count < 2)
					record.Requirements.CompletedQuests = new List<string>() { record.Requirements.CompletedQuests[0], "" };
				record.Requirements.CompletedQuests[1] = GUILayout.TextField(record.Requirements.CompletedQuests[1], GUILayout.Width(TRIPLE_W));
				if (record.Requirements.CompletedQuests.Count < 3)
					record.Requirements.CompletedQuests = new List<string>() { record.Requirements.CompletedQuests[0], record.Requirements.CompletedQuests[1], "" };
				record.Requirements.CompletedQuests[2] = GUILayout.TextField(record.Requirements.CompletedQuests[2], GUILayout.Width(TRIPLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Artifact requirements: ", GUI.skin.customStyles[0]);
				if (record.Requirements.AnalyzedArtifacts.Count < 1)
					record.Requirements.AnalyzedArtifacts = new List<string>() { "" };
				record.Requirements.AnalyzedArtifacts[0] = GUILayout.TextField(record.Requirements.AnalyzedArtifacts[0], GUILayout.Width(TRIPLE_W));
				if (record.Requirements.AnalyzedArtifacts.Count < 2)
					record.Requirements.AnalyzedArtifacts = new List<string>() { record.Requirements.AnalyzedArtifacts[0], "" };
				record.Requirements.AnalyzedArtifacts[1] = GUILayout.TextField(record.Requirements.AnalyzedArtifacts[1], GUILayout.Width(TRIPLE_W));
				if (record.Requirements.AnalyzedArtifacts.Count < 3)
					record.Requirements.AnalyzedArtifacts = new List<string>() { record.Requirements.AnalyzedArtifacts[0], record.Requirements.AnalyzedArtifacts[1], "" };
				record.Requirements.AnalyzedArtifacts[2] = GUILayout.TextField(record.Requirements.AnalyzedArtifacts[2], GUILayout.Width(TRIPLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("<color=#93dedf>✎ Has already been written at day:</color> ", GUI.skin.customStyles[0]);
				record.AssignedDay = int.Parse(GUILayout.TextField(record.AssignedDay.ToString(), GUILayout.Width(SINGLE_W)));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Space(CONTROL_W);
				if (i > 0)
				{
					if (GUILayout.Button("<color=#c8c174>▲</color>", GUILayout.Width(30)))
					{
						State.JournalRecords.Remove(record);
						State.JournalRecords.Insert(i - 1, record);
					}
				}
				else GUILayout.Space(35);
				if (i < State.JournalRecords.Count - 1)
				{
					if (GUILayout.Button("<color=#c8c174>▼</color>", GUILayout.Width(30)))
					{
						State.JournalRecords.Remove(record);
						State.JournalRecords.Insert(i + 1, record);
					}
				}
				else GUILayout.Space(35);
				if (GUILayout.Button("<color=red>─</color>", GUILayout.Width(30))) 
					State.JournalRecords.Remove(record);
				if (GUILayout.Button("<color=green>✚</color>", GUILayout.Width(30))) 
					State.JournalRecords.Insert(i, (new JournalRecord("Journal" + (State.JournalRecords.Count + 1).ToString(), new Requirements(day: -1))));
				GUILayout.EndHorizontal();

				GUILayout.Space(20);
			}
		}

		if (selectedPage == StateEditorPage.Quests)
		{
			for (int i = 0; i < State.QuestRecords.Count; i++)
			{
				var quest = State.QuestRecords[i];

				GUILayout.BeginHorizontal();
				GUILayout.Label("Quest ID: ", GUI.skin.customStyles[0]);
				quest.ID = GUILayout.TextField(quest.ID, GUILayout.Width(SINGLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Day requirements (min, specific, max): ", GUI.skin.customStyles[0]);
				quest.Requirements.MinDay = int.Parse(GUILayout.TextField(quest.Requirements.MinDay.ToString(), GUILayout.Width(TRIPLE_W)));
				quest.Requirements.Day = int.Parse(GUILayout.TextField(quest.Requirements.Day.ToString(), GUILayout.Width(TRIPLE_W)));
				quest.Requirements.MaxDay = int.Parse(GUILayout.TextField(quest.Requirements.MaxDay.ToString(), GUILayout.Width(TRIPLE_W)));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("AP requirements (min, max): ", GUI.skin.customStyles[0]);
				quest.Requirements.MinAP = int.Parse(GUILayout.TextField(quest.Requirements.MinAP.ToString(), GUILayout.Width(DOUBLE_W)));
				quest.Requirements.MaxAP = int.Parse(GUILayout.TextField(quest.Requirements.MaxAP.ToString(), GUILayout.Width(DOUBLE_W)));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Quest requirements: ", GUI.skin.customStyles[0]);
				if (quest.Requirements.CompletedQuests.Count < 1)
					quest.Requirements.CompletedQuests = new List<string>() { "" };
				quest.Requirements.CompletedQuests[0] = GUILayout.TextField(quest.Requirements.CompletedQuests[0], GUILayout.Width(TRIPLE_W));
				if (quest.Requirements.CompletedQuests.Count < 2)
					quest.Requirements.CompletedQuests = new List<string>() { quest.Requirements.CompletedQuests[0], "" };
				quest.Requirements.CompletedQuests[1] = GUILayout.TextField(quest.Requirements.CompletedQuests[1], GUILayout.Width(TRIPLE_W));
				if (quest.Requirements.CompletedQuests.Count < 3)
					quest.Requirements.CompletedQuests = new List<string>() { quest.Requirements.CompletedQuests[0], quest.Requirements.CompletedQuests[1], "" };
				quest.Requirements.CompletedQuests[2] = GUILayout.TextField(quest.Requirements.CompletedQuests[2], GUILayout.Width(TRIPLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Artifact requirements: ", GUI.skin.customStyles[0]);
				if (quest.Requirements.AnalyzedArtifacts.Count < 1)
					quest.Requirements.AnalyzedArtifacts = new List<string>() { "" };
				quest.Requirements.AnalyzedArtifacts[0] = GUILayout.TextField(quest.Requirements.AnalyzedArtifacts[0], GUILayout.Width(TRIPLE_W));
				if (quest.Requirements.AnalyzedArtifacts.Count < 2)
					quest.Requirements.AnalyzedArtifacts = new List<string>() { quest.Requirements.AnalyzedArtifacts[0], "" };
				quest.Requirements.AnalyzedArtifacts[1] = GUILayout.TextField(quest.Requirements.AnalyzedArtifacts[1], GUILayout.Width(TRIPLE_W));
				if (quest.Requirements.AnalyzedArtifacts.Count < 3)
					quest.Requirements.AnalyzedArtifacts = new List<string>() { quest.Requirements.AnalyzedArtifacts[0], quest.Requirements.AnalyzedArtifacts[1], "" };
				quest.Requirements.AnalyzedArtifacts[2] = GUILayout.TextField(quest.Requirements.AnalyzedArtifacts[2], GUILayout.Width(TRIPLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("<color=#93dedf>✎ Current quest status:</color> ", GUI.skin.customStyles[0]);
				if (GUILayout.Button(quest.Status == QuestStatus.NotStarted ? "<b>❖ NotStarted</b>" : "NotStarted", GUILayout.Width(TRIPLE_W))) quest.Status = QuestStatus.NotStarted;
				if (GUILayout.Button(quest.Status == QuestStatus.Started ? "<b>❖ Started</b>" : "Started", GUILayout.Width(TRIPLE_W))) quest.Status = QuestStatus.Started;
				if (GUILayout.Button(quest.Status == QuestStatus.Completed ? "<b>❖ Completed</b>" : "Completed", GUILayout.Width(TRIPLE_W))) quest.Status = QuestStatus.Completed;
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("<color=#93dedf>✎ Current quest block:</color> ", GUI.skin.customStyles[0]);
				quest.CurrentBlock = GUILayout.TextField(quest.CurrentBlock, GUILayout.Width(SINGLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Space(CONTROL_W);
				if (i > 0)
				{
					if (GUILayout.Button("<color=#c8c174>▲</color>", GUILayout.Width(30)))
					{
						State.QuestRecords.Remove(quest);
						State.QuestRecords.Insert(i - 1, quest);
					}
				}
				else GUILayout.Space(35);
				if (i < State.QuestRecords.Count - 1)
				{
					if (GUILayout.Button("<color=#c8c174>▼</color>", GUILayout.Width(30)))
					{
						State.QuestRecords.Remove(quest);
						State.QuestRecords.Insert(i + 1, quest);
					}
				}
				else GUILayout.Space(35);
				if (GUILayout.Button("<color=red>─</color>", GUILayout.Width(30)))
					State.QuestRecords.Remove(quest);
				if (GUILayout.Button("<color=green>✚</color>", GUILayout.Width(30)))
					State.QuestRecords.Insert(i, new Quest("Quest" + (State.QuestRecords.Count + 1).ToString(), new Requirements(minAP: -1)));
				GUILayout.EndHorizontal();

				GUILayout.Space(20);
			}
		}

		if (selectedPage == StateEditorPage.Artifacts)
		{
			for (int i = 0; i < State.Artifacts.Count; i++)
			{
				var artifact = State.Artifacts[i];

				GUILayout.BeginHorizontal();
				GUILayout.Label("Artifact ID: ", GUI.skin.customStyles[0]);
				artifact.ID = GUILayout.TextField(artifact.ID, GUILayout.Width(SINGLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Artifact name: ", GUI.skin.customStyles[0]);
				GUILayout.TextField(Text.Get(artifact.ID), GUILayout.Width(SINGLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Artifact scan info: ", GUI.skin.customStyles[0]);
				GUILayout.TextArea(Text.Get(artifact.ID + "ScanInfo"), GUILayout.Width(SINGLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Assigned sector (0 for any-random): ", GUI.skin.customStyles[0]);
				artifact.Sector = int.Parse(GUILayout.TextField(artifact.Sector.ToString(), GUILayout.Width(SINGLE_W)));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Artifact contents (wiring, alloy, chips): ", GUI.skin.customStyles[0]);
				artifact.Wiring = int.Parse(GUILayout.TextField(artifact.Wiring.ToString(), GUILayout.Width(TRIPLE_W)));
				artifact.Alloy = int.Parse(GUILayout.TextField(artifact.Alloy.ToString(), GUILayout.Width(TRIPLE_W)));
				artifact.Chips = int.Parse(GUILayout.TextField(artifact.Chips.ToString(), GUILayout.Width(TRIPLE_W)));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Artifact identity: ", GUI.skin.customStyles[0]);
				if (GUILayout.Button(artifact.Identity == null ? "<b>❖ NONE</b>" : "NONE", GUILayout.Width(TETTRO_W))) artifact.Identity = null;
				if (GUILayout.Button(artifact.Identity == BreakageType.BRK1 ? "<b>❖ BRK1</b>" : "BRK1", GUILayout.Width(TETTRO_W))) artifact.Identity = BreakageType.BRK1;
				if (GUILayout.Button(artifact.Identity == BreakageType.BRK2 ? "<b>❖ BRK2</b>" : "BRK2", GUILayout.Width(TETTRO_W))) artifact.Identity = BreakageType.BRK2;
				if (GUILayout.Button(artifact.Identity == BreakageType.BRK3 ? "<b>❖ BRK3</b>" : "BRK3", GUILayout.Width(TETTRO_W))) artifact.Identity = BreakageType.BRK3;
				if (GUILayout.Button(artifact.Identity == BreakageType.BRK4 ? "<b>❖ BRK4</b>" : "BRK4", GUILayout.Width(TETTRO_W))) artifact.Identity = BreakageType.BRK4;
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("<color=#93dedf>✎ Artifact current status:</color> ", GUI.skin.customStyles[0]);
				if (GUILayout.Button(artifact.Status == ArtifactStatus.NotFound ? "<b>❖ NotFound</b>" : "NotFound", GUILayout.Width(QUADRO_W))) artifact.Status = ArtifactStatus.NotFound;
				if (GUILayout.Button(artifact.Status == ArtifactStatus.Found ? "<b>❖ Found</b>" : "Found", GUILayout.Width(QUADRO_W))) artifact.Status = ArtifactStatus.Found;
				if (GUILayout.Button(artifact.Status == ArtifactStatus.Analyzing ? "<b>❖ Analyzing</b>" : "Analyzing", GUILayout.Width(QUADRO_W))) artifact.Status = ArtifactStatus.Analyzing;
				if (GUILayout.Button(artifact.Status == ArtifactStatus.Analyzed ? "<b>❖ Analyzed</b>" : "Analyzed", GUILayout.Width(QUADRO_W))) artifact.Status = ArtifactStatus.Analyzed;
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Space(CONTROL_W);
				if (i > 0)
				{
					if (GUILayout.Button("<color=#c8c174>▲</color>", GUILayout.Width(30)))
					{
						State.Artifacts.Remove(artifact);
						State.Artifacts.Insert(i - 1, artifact);
					}
				}
				else GUILayout.Space(35);
				if (i < State.Artifacts.Count - 1)
				{
					if (GUILayout.Button("<color=#c8c174>▼</color>", GUILayout.Width(30)))
					{
						State.Artifacts.Remove(artifact);
						State.Artifacts.Insert(i + 1, artifact);
					}
				}
				else GUILayout.Space(35);
				if (GUILayout.Button("<color=red>─</color>", GUILayout.Width(30)))
					State.Artifacts.Remove(artifact);
				if (GUILayout.Button("<color=green>✚</color>", GUILayout.Width(30)))
					State.Artifacts.Insert(i, new Artifact("Artifact" + (State.Artifacts.Count + 1).ToString(), 0, null, 0, 0, 0));
				GUILayout.EndHorizontal();

				GUILayout.Space(20);
			}
		}

		if (selectedPage == StateEditorPage.Phrases)
		{
			for (int i = 0; i < State.Phrases.Count; i++)
			{
				var phrase = State.Phrases[i];

				GUILayout.BeginHorizontal();
				GUILayout.Label("Phrase ID: ", GUI.skin.customStyles[0]);
				phrase.ID = GUILayout.TextField(phrase.ID, GUILayout.Width(SINGLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Phrase text: ", GUI.skin.customStyles[0]);
				GUILayout.TextArea(Text.Get(phrase.ID), GUILayout.Width(SINGLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Day requirements (min, specific, max): ", GUI.skin.customStyles[0]);
				phrase.Requirements.MinDay = int.Parse(GUILayout.TextField(phrase.Requirements.MinDay.ToString(), GUILayout.Width(TRIPLE_W)));
				phrase.Requirements.Day = int.Parse(GUILayout.TextField(phrase.Requirements.Day.ToString(), GUILayout.Width(TRIPLE_W)));
				phrase.Requirements.MaxDay = int.Parse(GUILayout.TextField(phrase.Requirements.MaxDay.ToString(), GUILayout.Width(TRIPLE_W)));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("AP requirements (min, max): ", GUI.skin.customStyles[0]);
				phrase.Requirements.MinAP = int.Parse(GUILayout.TextField(phrase.Requirements.MinAP.ToString(), GUILayout.Width(DOUBLE_W)));
				phrase.Requirements.MaxAP = int.Parse(GUILayout.TextField(phrase.Requirements.MaxAP.ToString(), GUILayout.Width(DOUBLE_W)));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Quest requirements: ", GUI.skin.customStyles[0]);
				if (phrase.Requirements.CompletedQuests.Count < 1)
					phrase.Requirements.CompletedQuests = new List<string>() { "" };
				phrase.Requirements.CompletedQuests[0] = GUILayout.TextField(phrase.Requirements.CompletedQuests[0], GUILayout.Width(TRIPLE_W));
				if (phrase.Requirements.CompletedQuests.Count < 2)
					phrase.Requirements.CompletedQuests = new List<string>() { phrase.Requirements.CompletedQuests[0], "" };
				phrase.Requirements.CompletedQuests[1] = GUILayout.TextField(phrase.Requirements.CompletedQuests[1], GUILayout.Width(TRIPLE_W));
				if (phrase.Requirements.CompletedQuests.Count < 3)
					phrase.Requirements.CompletedQuests = new List<string>() { phrase.Requirements.CompletedQuests[0], phrase.Requirements.CompletedQuests[1], "" };
				phrase.Requirements.CompletedQuests[2] = GUILayout.TextField(phrase.Requirements.CompletedQuests[2], GUILayout.Width(TRIPLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Artifact requirements: ", GUI.skin.customStyles[0]);
				if (phrase.Requirements.AnalyzedArtifacts.Count < 1)
					phrase.Requirements.AnalyzedArtifacts = new List<string>() { "" };
				phrase.Requirements.AnalyzedArtifacts[0] = GUILayout.TextField(phrase.Requirements.AnalyzedArtifacts[0], GUILayout.Width(TRIPLE_W));
				if (phrase.Requirements.AnalyzedArtifacts.Count < 2)
					phrase.Requirements.AnalyzedArtifacts = new List<string>() { phrase.Requirements.AnalyzedArtifacts[0], "" };
				phrase.Requirements.AnalyzedArtifacts[1] = GUILayout.TextField(phrase.Requirements.AnalyzedArtifacts[1], GUILayout.Width(TRIPLE_W));
				if (phrase.Requirements.AnalyzedArtifacts.Count < 3)
					phrase.Requirements.AnalyzedArtifacts = new List<string>() { phrase.Requirements.AnalyzedArtifacts[0], phrase.Requirements.AnalyzedArtifacts[1], "" };
				phrase.Requirements.AnalyzedArtifacts[2] = GUILayout.TextField(phrase.Requirements.AnalyzedArtifacts[2], GUILayout.Width(TRIPLE_W));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Space(CONTROL_W);
				if (i > 0)
				{
					if (GUILayout.Button("<color=#c8c174>▲</color>", GUILayout.Width(30)))
					{
						State.Phrases.Remove(phrase);
						State.Phrases.Insert(i - 1, phrase);
					}
				}
				else GUILayout.Space(35);
				if (i < State.Phrases.Count - 1)
				{
					if (GUILayout.Button("<color=#c8c174>▼</color>", GUILayout.Width(30)))
					{
						State.Phrases.Remove(phrase);
						State.Phrases.Insert(i + 1, phrase);
					}
				}
				else GUILayout.Space(35);
				if (GUILayout.Button("<color=red>─</color>", GUILayout.Width(30)))
					State.Phrases.Remove(phrase);
				if (GUILayout.Button("<color=green>✚</color>", GUILayout.Width(30)))
					State.Phrases.Insert(i, (new Phrase("Phrase" + (State.Phrases.Count + 1).ToString(), new Requirements(day: -1))));
				GUILayout.EndHorizontal();

				GUILayout.Space(20);
			}
		}

		GUILayout.EndScrollView();

		GUILayout.Space(2);

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("<b><color=#60c367><size=45>☑</size></color>    Save and return to menu</b>\n", GUILayout.Height(30))) SaveAndExit();
		if (GUILayout.Button("<b><color=#d9614d><size=45>☒</size></color>    Total reset</b>\n", GUILayout.Height(30), GUILayout.Width(150))) State.Reset(true);
		if (GUILayout.Button("<b><color=#6b9fb8><size=45>☯</size></color>    Update text</b>\n", GUILayout.Height(30), GUILayout.Width(150))) ServiceLocator.Text = new GoogleText();
		GUILayout.EndHorizontal();

		GUILayout.EndArea();
	}
}