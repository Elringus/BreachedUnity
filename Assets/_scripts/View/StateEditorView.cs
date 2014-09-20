using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class StateEditorView : BaseView
{
	private readonly float WIDTH = 800;
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

	private void OnGUI ()
	{
		GUI.skin.customStyles[0] = new GUIStyle(GUI.skin.label);
		GUI.skin.customStyles[0].alignment = TextAnchor.MiddleRight;
		GUI.skin.customStyles[0].fixedWidth = 300;
		GUI.skin.customStyles[0].padding.right = 10;

		GUILayout.BeginArea(new Rect(Screen.width / 2 - WIDTH / 2, Screen.height / 2 - Screen.height / 2, WIDTH, Screen.height));
		GUILayout.Box("Breached state editor");

		GUILayout.BeginHorizontal();
		if (GUILayout.Button(selectedPage == StateEditorPage.Main ? "<b>❖ Main</b>" : "Main", GUILayout.Width(155))) selectedPage = StateEditorPage.Main;
		if (GUILayout.Button(selectedPage == StateEditorPage.Journal ? "<b>❖ Journal</b>" : "Journal", GUILayout.Width(155))) selectedPage = StateEditorPage.Journal;
		if (GUILayout.Button(selectedPage == StateEditorPage.Quests ? "<b>❖ Quests</b>" : "Quests", GUILayout.Width(155))) selectedPage = StateEditorPage.Quests;
		if (GUILayout.Button(selectedPage == StateEditorPage.Artifacts ? "<b>❖ Artifacts</b>" : "Artifacts", GUILayout.Width(155))) selectedPage = StateEditorPage.Artifacts;
		if (GUILayout.Button(selectedPage == StateEditorPage.Phrases ? "<b>❖ Phrases</b>" : "Phrases", GUILayout.Width(155))) selectedPage = StateEditorPage.Phrases;
		GUILayout.EndHorizontal();

		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(WIDTH), GUILayout.Height(Screen.height - 95));

		if (selectedPage == StateEditorPage.Main)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Save data verion: ", GUI.skin.customStyles[0]);
			GUILayout.Label(string.Format("{0}.{1}.{2}", State.VersionMajor, State.VersionMiddle, State.VersionMinor));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Game status:</color> ", GUI.skin.customStyles[0]);
			GUILayout.Label(string.Format("<b>{0}</b>", State.GameStatus.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Total days: ", GUI.skin.customStyles[0]);
			State.TotalDays = int.Parse(GUILayout.TextField(State.TotalDays.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Current day:</color> ", GUI.skin.customStyles[0]);
			State.CurrentDay = int.Parse(GUILayout.TextField(State.CurrentDay.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Max AP: ", GUI.skin.customStyles[0]);
			State.MaxAP = int.Parse(GUILayout.TextField(State.MaxAP.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Current AP:</color> ", GUI.skin.customStyles[0]);
			State.CurrentAP = int.Parse(GUILayout.TextField(State.CurrentAP.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix engine AP cost: ", GUI.skin.customStyles[0]);
			State.FixEngineAPCost = int.Parse(GUILayout.TextField(State.FixEngineAPCost.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix BRK1 requirements (wiring, alloy, chips): ", GUI.skin.customStyles[0]);
			State.FixEngineRequirements[BreakageType.BRK1][0] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK1][0].ToString()));
			State.FixEngineRequirements[BreakageType.BRK1][1] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK1][1].ToString()));
			State.FixEngineRequirements[BreakageType.BRK1][2] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK1][2].ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix BRK2 requirements (wiring, alloy, chips): ", GUI.skin.customStyles[0]);
			State.FixEngineRequirements[BreakageType.BRK2][0] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK2][0].ToString()));
			State.FixEngineRequirements[BreakageType.BRK2][1] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK2][1].ToString()));
			State.FixEngineRequirements[BreakageType.BRK2][2] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK2][2].ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix BRK3 requirements (wiring, alloy, chips): ", GUI.skin.customStyles[0]);
			State.FixEngineRequirements[BreakageType.BRK3][0] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK3][0].ToString()));
			State.FixEngineRequirements[BreakageType.BRK3][1] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK3][1].ToString()));
			State.FixEngineRequirements[BreakageType.BRK3][2] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK3][2].ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix BRK4 requirements (wiring, alloy, chips): ", GUI.skin.customStyles[0]);
			State.FixEngineRequirements[BreakageType.BRK4][0] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK4][0].ToString()));
			State.FixEngineRequirements[BreakageType.BRK4][1] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK4][1].ToString()));
			State.FixEngineRequirements[BreakageType.BRK4][2] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK4][2].ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Current breakage type:</color> ", GUI.skin.customStyles[0]);
			if (GUILayout.Button(State.BreakageType == BreakageType.BRK1 ? "<b>❖ BRK1</b>" : "BRK1", GUILayout.Width(115))) State.BreakageType = BreakageType.BRK1;
			if (GUILayout.Button(State.BreakageType == BreakageType.BRK2 ? "<b>❖ BRK2</b>" : "BRK2", GUILayout.Width(115))) State.BreakageType = BreakageType.BRK2;
			if (GUILayout.Button(State.BreakageType == BreakageType.BRK3 ? "<b>❖ BRK3</b>" : "BRK3", GUILayout.Width(115))) State.BreakageType = BreakageType.BRK3;
			if (GUILayout.Button(State.BreakageType == BreakageType.BRK4 ? "<b>❖ BRK4</b>" : "BRK4", GUILayout.Width(115))) State.BreakageType = BreakageType.BRK4;
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Current Wiring, Alloy, Chips:</color> ", GUI.skin.customStyles[0]);
			State.Wiring = int.Parse(GUILayout.TextField(State.Wiring.ToString()));
			State.Alloy = int.Parse(GUILayout.TextField(State.Alloy.ToString()));
			State.Chips = int.Parse(GUILayout.TextField(State.Chips.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Engine is fixed:</color> ", GUI.skin.customStyles[0]);
			State.EngineFixed = GUILayout.Toggle(State.EngineFixed, "");
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Synth fuel AP cost: ", GUI.skin.customStyles[0]);
			State.FuelSynthAPCost = int.Parse(GUILayout.TextField(State.FuelSynthAPCost.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Synth fuel grace: ", GUI.skin.customStyles[0]);
			State.FuelSynthGrace = int.Parse(GUILayout.TextField(State.FuelSynthGrace.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Synth fuel summ: ", GUI.skin.customStyles[0]);
			State.FuelSynthSumm = int.Parse(GUILayout.TextField(State.FuelSynthSumm.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Current fuel synth formula (A, B, C):</color> ", GUI.skin.customStyles[0]);
			State.FuelSynthFormula[0] = int.Parse(GUILayout.TextField(State.FuelSynthFormula[0].ToString()));
			State.FuelSynthFormula[1] = int.Parse(GUILayout.TextField(State.FuelSynthFormula[1].ToString()));
			State.FuelSynthFormula[2] = int.Parse(GUILayout.TextField(State.FuelSynthFormula[2].ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Current minerals count (A, B, C):</color> ", GUI.skin.customStyles[0]);
			State.MineralA = int.Parse(GUILayout.TextField(State.MineralA.ToString()));
			State.MineralB = int.Parse(GUILayout.TextField(State.MineralB.ToString()));
			State.MineralC = int.Parse(GUILayout.TextField(State.MineralC.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#93dedf>✎ Fuel is synthed:</color> ", GUI.skin.customStyles[0]);
			State.FuelSynthed = GUILayout.Toggle(State.FuelSynthed, "");
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Enter sector AP cost: ", GUI.skin.customStyles[0]);
			State.EnterSectorAPCost = int.Parse(GUILayout.TextField(State.EnterSectorAPCost.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Loot charges: ", GUI.skin.customStyles[0]);
			State.LootCharges = int.Parse(GUILayout.TextField(State.LootCharges.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Sector 1 parameters (loot spots, A, B, C): ", GUI.skin.customStyles[0]);
			State.SectorsParameters.Find(x => x.SectorID == 1).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).LootSpotCount.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 1).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).MineralA.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 1).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).MineralB.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 1).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).MineralC.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Sector 2 parameters (loot spots, A, B, C): ", GUI.skin.customStyles[0]);
			State.SectorsParameters.Find(x => x.SectorID == 2).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).LootSpotCount.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 2).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).MineralA.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 2).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).MineralB.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 2).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).MineralC.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Sector 3 parameters (loot spots, A, B, C): ", GUI.skin.customStyles[0]);
			State.SectorsParameters.Find(x => x.SectorID == 3).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).LootSpotCount.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 3).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).MineralA.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 3).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).MineralB.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 3).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).MineralC.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Sector 4 parameters (loot spots, A, B, C): ", GUI.skin.customStyles[0]);
			State.SectorsParameters.Find(x => x.SectorID == 4).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).LootSpotCount.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 4).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).MineralA.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 4).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).MineralB.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 4).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).MineralC.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Analyze artifact AP cost: ", GUI.skin.customStyles[0]);
			State.AnalyzeArtifactAPCost = int.Parse(GUILayout.TextField(State.AnalyzeArtifactAPCost.ToString()));
			GUILayout.EndHorizontal();
		}

		if (selectedPage == StateEditorPage.Journal)
		{
			for (int i = 0; i < State.JournalRecords.Count; i++)
			{
				var record = State.JournalRecords[i];

				GUILayout.BeginHorizontal();
				GUILayout.Label("Journal record ID: ", GUI.skin.customStyles[0]);
				record.ID = GUILayout.TextField(record.ID);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Journal record text: ", GUI.skin.customStyles[0]);
				GUILayout.TextArea(Text.Get(record.ID));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Day requirements (min, current, max): ", GUI.skin.customStyles[0]);
				record.Requirements.MinDay = int.Parse(GUILayout.TextField(record.Requirements.MinDay.ToString()));
				record.Requirements.Day = int.Parse(GUILayout.TextField(record.Requirements.Day.ToString()));
				record.Requirements.MaxDay = int.Parse(GUILayout.TextField(record.Requirements.MaxDay.ToString()));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("AP requirements (min, max): ", GUI.skin.customStyles[0]);
				record.Requirements.MinAP = int.Parse(GUILayout.TextField(record.Requirements.MinAP.ToString()));
				record.Requirements.MaxAP = int.Parse(GUILayout.TextField(record.Requirements.MaxAP.ToString()));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Quest requirements: ", GUI.skin.customStyles[0]);
				if (record.Requirements.CompletedQuests.Count < 1)
					record.Requirements.CompletedQuests = new List<string>() { "" };
				record.Requirements.CompletedQuests[0] = GUILayout.TextField(record.Requirements.CompletedQuests[0]);
				if (record.Requirements.CompletedQuests.Count < 2)
					record.Requirements.CompletedQuests = new List<string>() { record.Requirements.CompletedQuests[0], "" };
				record.Requirements.CompletedQuests[1] = GUILayout.TextField(record.Requirements.CompletedQuests[1]);
				if (record.Requirements.CompletedQuests.Count < 3)
					record.Requirements.CompletedQuests = new List<string>() { record.Requirements.CompletedQuests[0], record.Requirements.CompletedQuests[1], "" };
				record.Requirements.CompletedQuests[2] = GUILayout.TextField(record.Requirements.CompletedQuests[2]);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Artifact requirements: ", GUI.skin.customStyles[0]);
				if (record.Requirements.AnalyzedArtifacts.Count < 1)
					record.Requirements.AnalyzedArtifacts = new List<string>() { "" };
				record.Requirements.AnalyzedArtifacts[0] = GUILayout.TextField(record.Requirements.AnalyzedArtifacts[0]);
				if (record.Requirements.AnalyzedArtifacts.Count < 2)
					record.Requirements.AnalyzedArtifacts = new List<string>() { record.Requirements.AnalyzedArtifacts[0], "" };
				record.Requirements.AnalyzedArtifacts[1] = GUILayout.TextField(record.Requirements.AnalyzedArtifacts[1]);
				if (record.Requirements.AnalyzedArtifacts.Count < 3)
					record.Requirements.AnalyzedArtifacts = new List<string>() { record.Requirements.AnalyzedArtifacts[0], record.Requirements.AnalyzedArtifacts[1], "" };
				record.Requirements.AnalyzedArtifacts[2] = GUILayout.TextField(record.Requirements.AnalyzedArtifacts[2]);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("<color=#93dedf>✎ Has already been written at day:</color> ", GUI.skin.customStyles[0]);
				record.AssignedDay = int.Parse(GUILayout.TextField(record.AssignedDay.ToString()));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Space(647);
				if (i > 0)
				{
					if (GUILayout.Button("<color=#c8c174>▲</color>", GUILayout.Width(30)))
					{
						State.JournalRecords.Remove(record);
						State.JournalRecords.Insert(i - 1, record);
					}
				}
				else GUILayout.Space(30);
				if (i < State.JournalRecords.Count - 1)
				{
					if (GUILayout.Button("<color=#c8c174>▼</color>", GUILayout.Width(30)))
					{
						State.JournalRecords.Remove(record);
						State.JournalRecords.Insert(i + 1, record);
					}
				}
				else GUILayout.Space(30);
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

				GUILayout.Label("ID: ", GUILayout.Width(30));
				quest.ID = GUILayout.TextField(quest.ID, GUILayout.Width(125));

				GUILayout.Label("Min AP: ", GUILayout.Width(50));
				quest.Requirements.MinAP = int.Parse(GUILayout.TextField(quest.Requirements.MinAP.ToString(), GUILayout.Width(50)));

				GUILayout.Label("Min day: ", GUILayout.Width(50));
				quest.Requirements.MinDay = int.Parse(GUILayout.TextField(quest.Requirements.MinDay.ToString(), GUILayout.Width(50)));

				GUILayout.Label("Compl. quests: ", GUILayout.Width(100));
				if (quest.Requirements.CompletedQuests.Count < 1) quest.Requirements.CompletedQuests = new List<string>() { "" };
				quest.Requirements.CompletedQuests[0] = GUILayout.TextField(quest.Requirements.CompletedQuests[0], GUILayout.Width(100));
				if (quest.Requirements.CompletedQuests.Count < 2) quest.Requirements.CompletedQuests = new List<string>() { quest.Requirements.CompletedQuests[0], "" };
				quest.Requirements.CompletedQuests[1] = GUILayout.TextField(quest.Requirements.CompletedQuests[1], GUILayout.Width(100));
				if (quest.Requirements.CompletedQuests.Count < 3) quest.Requirements.CompletedQuests = new List<string>() { quest.Requirements.CompletedQuests[0], quest.Requirements.CompletedQuests[1], "" };
				quest.Requirements.CompletedQuests[2] = GUILayout.TextField(quest.Requirements.CompletedQuests[2], GUILayout.Width(100));

				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label(string.Format("✎ Current quest status is <b>{1}</b>. Change to: ", quest.ID, quest.Status), GUI.skin.customStyles[0]);
				if (GUILayout.Button("NotStarted", GUILayout.Width(80))) quest.Status = QuestStatus.NotStarted;
				if (GUILayout.Button("Started", GUILayout.Width(80))) quest.Status = QuestStatus.Started;
				if (GUILayout.Button("Completed", GUILayout.Width(80))) quest.Status = QuestStatus.Completed;
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("✎ Current quest block is: ", GUILayout.Width(150));
				quest.CurrentBlock = GUILayout.TextField(quest.CurrentBlock, GUILayout.Width(150));
				GUILayout.Space(340);
				if (i > 0 && GUILayout.Button("▲"))
				{
					State.QuestRecords.Remove(quest);
					State.QuestRecords.Insert(i - 1, quest);
				}
				if (i < State.QuestRecords.Count - 1 && GUILayout.Button("▼"))
				{
					State.QuestRecords.Remove(quest);
					State.QuestRecords.Insert(i + 1, quest);
				}
				GUILayout.Space(10);
				if (GUILayout.Button("✂")) State.QuestRecords.Remove(quest);
				if (GUILayout.Button("✚")) State.QuestRecords.Insert(i, new Quest("Quest" + (State.QuestRecords.Count + 1).ToString(), new Requirements(minAP: -1)));
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
				GUILayout.Label("Artifact (id, sector, W, A, C):", GUI.skin.customStyles[0]);
				artifact.ID = GUILayout.TextField(artifact.ID, GUILayout.Width(215));
				artifact.Sector = int.Parse(GUILayout.TextField(artifact.Sector.ToString(), GUILayout.Width(60)));
				artifact.Wiring = int.Parse(GUILayout.TextField(artifact.Wiring.ToString(), GUILayout.Width(60)));
				artifact.Alloy = int.Parse(GUILayout.TextField(artifact.Alloy.ToString(), GUILayout.Width(60)));
				artifact.Chips = int.Parse(GUILayout.TextField(artifact.Chips.ToString(), GUILayout.Width(60)));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label(string.Format("Identity is <b>{0}</b> Choose another:", artifact.Identity == null ? "NONE" : artifact.Identity.ToString()), GUI.skin.customStyles[0]);
				if (GUILayout.Button("NONE")) artifact.Identity = null;
				if (GUILayout.Button("BRK1")) artifact.Identity = BreakageType.BRK1;
				if (GUILayout.Button("BRK2")) artifact.Identity = BreakageType.BRK2;
				if (GUILayout.Button("BRK3")) artifact.Identity = BreakageType.BRK3;
				if (GUILayout.Button("BRK4")) artifact.Identity = BreakageType.BRK4;
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label(string.Format("✎ Current status is <b>{0}</b>. Set it to:",
					artifact.Status.ToString()), GUI.skin.customStyles[0]);
				if (GUILayout.Button("NotFound")) artifact.Status = ArtifactStatus.NotFound;
				if (GUILayout.Button("Found")) artifact.Status = ArtifactStatus.Found;
				if (GUILayout.Button("Analyzing")) artifact.Status = ArtifactStatus.Analyzing;
				if (GUILayout.Button("Analyzed")) artifact.Status = ArtifactStatus.Analyzed;
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Space(700);
				if (GUILayout.Button("✂")) State.Artifacts.Remove(artifact);
				if (GUILayout.Button("✚")) State.Artifacts.Insert(i, new Artifact("Artifact" + (State.Artifacts.Count + 1).ToString(), 0, null, 0, 0, 0));
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
				GUILayout.Label("ID: ", GUILayout.Width(30));
				phrase.ID = GUILayout.TextField(phrase.ID, GUILayout.Width(100));

				GUILayout.Label("Day: ", GUILayout.Width(30));
				phrase.Requirements.Day = int.Parse(GUILayout.TextField(phrase.Requirements.Day.ToString(), GUILayout.Width(50)));

				GUILayout.Label("Compl. quests: ", GUILayout.Width(100));
				if (phrase.Requirements.CompletedQuests.Count < 1) 
					phrase.Requirements.CompletedQuests = new List<string>() { "" };
				phrase.Requirements.CompletedQuests[0] = GUILayout.TextField(phrase.Requirements.CompletedQuests[0], GUILayout.Width(100));
				if (phrase.Requirements.CompletedQuests.Count < 2) 
					phrase.Requirements.CompletedQuests = new List<string>() { phrase.Requirements.CompletedQuests[0], "" };
				phrase.Requirements.CompletedQuests[1] = GUILayout.TextField(phrase.Requirements.CompletedQuests[1], GUILayout.Width(100));
				if (phrase.Requirements.CompletedQuests.Count < 3) 
					phrase.Requirements.CompletedQuests = new List<string>() { phrase.Requirements.CompletedQuests[0], phrase.Requirements.CompletedQuests[1], "" };
				phrase.Requirements.CompletedQuests[2] = GUILayout.TextField(phrase.Requirements.CompletedQuests[2], GUILayout.Width(100));

				if (GUILayout.Button("✂")) State.Phrases.Remove(phrase);
				if (GUILayout.Button("✚")) State.Phrases.Insert(i, (new Phrase("Phrase" + (State.Phrases.Count + 1).ToString(), new Requirements(day: -1))));
				GUILayout.EndHorizontal();
			}
		}

		GUILayout.EndScrollView();

		GUILayout.Box("", GUILayout.Height(2));
		GUILayout.Space(2);

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Save and return to menu", GUILayout.Height(30))) SaveAndExit();
		if (GUILayout.Button("<size=45><b>☠</b></size>    Total reset\n", GUILayout.Height(30), GUILayout.Width(150))) State.Reset(true);
		GUILayout.EndHorizontal();

		GUILayout.EndArea();
	}
}