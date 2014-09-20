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
		GUILayout.BeginArea(new Rect(Screen.width / 2 - WIDTH / 2, Screen.height / 2 - Screen.height / 2, WIDTH, Screen.height));
		GUILayout.Box("Breached state editor");
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(WIDTH), GUILayout.Height(Screen.height - 100));

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Main")) selectedPage = StateEditorPage.Main;
		if (GUILayout.Button("Journal")) selectedPage = StateEditorPage.Journal;
		if (GUILayout.Button("Quests")) selectedPage = StateEditorPage.Quests;
		if (GUILayout.Button("Artifacts")) selectedPage = StateEditorPage.Artifacts;
		if (GUILayout.Button("Phrases")) selectedPage = StateEditorPage.Phrases;
		GUILayout.EndHorizontal();

		if (selectedPage == StateEditorPage.Main)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Save data verion: ", GUILayout.Width(300));
			GUILayout.Label(string.Format("{0}.{1}.{2}", State.VersionMajor, State.VersionMiddle, State.VersionMinor));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("✎ Game status: ", GUILayout.Width(300));
			GUILayout.Label(State.GameStatus.ToString());
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Total days: ", GUILayout.Width(300));
			State.TotalDays = int.Parse(GUILayout.TextField(State.TotalDays.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("✎ Current day: ", GUILayout.Width(300));
			State.CurrentDay = int.Parse(GUILayout.TextField(State.CurrentDay.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Max AP: ", GUILayout.Width(300));
			State.MaxAP = int.Parse(GUILayout.TextField(State.MaxAP.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("✎ Current AP: ", GUILayout.Width(300));
			State.CurrentAP = int.Parse(GUILayout.TextField(State.CurrentAP.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix engine AP cost: ", GUILayout.Width(300));
			State.FixEngineAPCost = int.Parse(GUILayout.TextField(State.FixEngineAPCost.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix BRK1 requirements (wiring, alloy, chips): ", GUILayout.Width(300));
			State.FixEngineRequirements[BreakageType.BRK1][0] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK1][0].ToString()));
			State.FixEngineRequirements[BreakageType.BRK1][1] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK1][1].ToString()));
			State.FixEngineRequirements[BreakageType.BRK1][2] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK1][2].ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix BRK2 requirements (wiring, alloy, chips): ", GUILayout.Width(300));
			State.FixEngineRequirements[BreakageType.BRK2][0] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK2][0].ToString()));
			State.FixEngineRequirements[BreakageType.BRK2][1] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK2][1].ToString()));
			State.FixEngineRequirements[BreakageType.BRK2][2] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK2][2].ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix BRK3 requirements (wiring, alloy, chips): ", GUILayout.Width(300));
			State.FixEngineRequirements[BreakageType.BRK3][0] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK3][0].ToString()));
			State.FixEngineRequirements[BreakageType.BRK3][1] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK3][1].ToString()));
			State.FixEngineRequirements[BreakageType.BRK3][2] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK3][2].ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Fix BRK4 requirements (wiring, alloy, chips): ", GUILayout.Width(300));
			State.FixEngineRequirements[BreakageType.BRK4][0] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK4][0].ToString()));
			State.FixEngineRequirements[BreakageType.BRK4][1] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK4][1].ToString()));
			State.FixEngineRequirements[BreakageType.BRK4][2] = int.Parse(GUILayout.TextField(State.FixEngineRequirements[BreakageType.BRK4][2].ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label(string.Format("✎ Current breakage type is {0}. Set it to:",
				State.BreakageType), GUILayout.Width(300));
			if (GUILayout.Button("BRK1")) State.BreakageType = BreakageType.BRK1;
			if (GUILayout.Button("BRK2")) State.BreakageType = BreakageType.BRK2;
			if (GUILayout.Button("BRK3")) State.BreakageType = BreakageType.BRK3;
			if (GUILayout.Button("BRK4")) State.BreakageType = BreakageType.BRK4;
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("✎ Current Wiring, Alloy, Chips: ", GUILayout.Width(300));
			State.Wiring = int.Parse(GUILayout.TextField(State.Wiring.ToString()));
			State.Alloy = int.Parse(GUILayout.TextField(State.Alloy.ToString()));
			State.Chips = int.Parse(GUILayout.TextField(State.Chips.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("✎ Engine is fixed: ", GUILayout.Width(300));
			State.EngineFixed = GUILayout.Toggle(State.EngineFixed, "");
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Synth fuel AP cost: ", GUILayout.Width(300));
			State.FuelSynthAPCost = int.Parse(GUILayout.TextField(State.FuelSynthAPCost.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Synth fuel grace: ", GUILayout.Width(300));
			State.FuelSynthGrace = int.Parse(GUILayout.TextField(State.FuelSynthGrace.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Synth fuel summ: ", GUILayout.Width(300));
			State.FuelSynthSumm = int.Parse(GUILayout.TextField(State.FuelSynthSumm.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("✎ Current fuel synth formula (A, B, C): ", GUILayout.Width(300));
			State.FuelSynthFormula[0] = int.Parse(GUILayout.TextField(State.FuelSynthFormula[0].ToString()));
			State.FuelSynthFormula[1] = int.Parse(GUILayout.TextField(State.FuelSynthFormula[1].ToString()));
			State.FuelSynthFormula[2] = int.Parse(GUILayout.TextField(State.FuelSynthFormula[2].ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("✎ Current minerals count (A, B, C): ", GUILayout.Width(300));
			State.MineralA = int.Parse(GUILayout.TextField(State.MineralA.ToString()));
			State.MineralB = int.Parse(GUILayout.TextField(State.MineralB.ToString()));
			State.MineralC = int.Parse(GUILayout.TextField(State.MineralC.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("✎ Fuel is synthed: ", GUILayout.Width(300));
			State.FuelSynthed = GUILayout.Toggle(State.FuelSynthed, "");
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Enter sector AP cost: ", GUILayout.Width(300));
			State.EnterSectorAPCost = int.Parse(GUILayout.TextField(State.EnterSectorAPCost.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Loot charges: ", GUILayout.Width(300));
			State.LootCharges = int.Parse(GUILayout.TextField(State.LootCharges.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Sector 1 parameters (loot spots, A, B, C): ", GUILayout.Width(300));
			State.SectorsParameters.Find(x => x.SectorID == 1).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).LootSpotCount.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 1).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).MineralA.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 1).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).MineralB.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 1).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).MineralC.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Sector 2 parameters (loot spots, A, B, C): ", GUILayout.Width(300));
			State.SectorsParameters.Find(x => x.SectorID == 2).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).LootSpotCount.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 2).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).MineralA.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 2).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).MineralB.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 2).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).MineralC.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Sector 3 parameters (loot spots, A, B, C): ", GUILayout.Width(300));
			State.SectorsParameters.Find(x => x.SectorID == 3).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).LootSpotCount.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 3).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).MineralA.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 3).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).MineralB.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 3).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).MineralC.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Sector 4 parameters (loot spots, A, B, C): ", GUILayout.Width(300));
			State.SectorsParameters.Find(x => x.SectorID == 4).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).LootSpotCount.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 4).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).MineralA.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 4).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).MineralB.ToString()));
			State.SectorsParameters.Find(x => x.SectorID == 4).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).MineralC.ToString()));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Analyze artifact AP cost: ", GUILayout.Width(300));
			State.AnalyzeArtifactAPCost = int.Parse(GUILayout.TextField(State.AnalyzeArtifactAPCost.ToString()));
			GUILayout.EndHorizontal();
		}

		if (selectedPage == StateEditorPage.Journal)
		{
			for (int i = 0; i < State.JournalRecords.Count; i++)
			{
				GUILayout.Space(20);

				var record = State.JournalRecords[i];

				GUILayout.BeginHorizontal();
				GUILayout.Label("ID: ", GUILayout.Width(30));
				record.ID = GUILayout.TextField(record.ID, GUILayout.Width(100));

				GUILayout.Label("Min day: ", GUILayout.Width(55));
				record.Requirements.MinDay = int.Parse(GUILayout.TextField(record.Requirements.MinDay.ToString(), GUILayout.Width(50)));
				GUILayout.Label("Day: ", GUILayout.Width(35));
				record.Requirements.Day = int.Parse(GUILayout.TextField(record.Requirements.Day.ToString(), GUILayout.Width(50)));
				GUILayout.Label("Max day: ", GUILayout.Width(55));
				record.Requirements.MaxDay = int.Parse(GUILayout.TextField(record.Requirements.MaxDay.ToString(), GUILayout.Width(50)));

				GUILayout.Label("Min AP: ", GUILayout.Width(55));
				record.Requirements.MinAP = int.Parse(GUILayout.TextField(record.Requirements.MinAP.ToString(), GUILayout.Width(50)));
				GUILayout.Label("Max AP: ", GUILayout.Width(55));
				record.Requirements.MaxAP = int.Parse(GUILayout.TextField(record.Requirements.MaxAP.ToString(), GUILayout.Width(50)));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Compl. quests: ", GUILayout.Width(100));
				if (record.Requirements.CompletedQuests.Count < 1)
					record.Requirements.CompletedQuests = new List<string>() { "" };
				record.Requirements.CompletedQuests[0] = GUILayout.TextField(record.Requirements.CompletedQuests[0], GUILayout.Width(189));
				if (record.Requirements.CompletedQuests.Count < 2)
					record.Requirements.CompletedQuests = new List<string>() { record.Requirements.CompletedQuests[0], "" };
				record.Requirements.CompletedQuests[1] = GUILayout.TextField(record.Requirements.CompletedQuests[1], GUILayout.Width(189));
				if (record.Requirements.CompletedQuests.Count < 3)
					record.Requirements.CompletedQuests = new List<string>() { record.Requirements.CompletedQuests[0], record.Requirements.CompletedQuests[1], "" };
				record.Requirements.CompletedQuests[2] = GUILayout.TextField(record.Requirements.CompletedQuests[2], GUILayout.Width(189));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Anl-d artifacts: ", GUILayout.Width(100));
				if (record.Requirements.AnalyzedArtifacts.Count < 1)
					record.Requirements.AnalyzedArtifacts = new List<string>() { "" };
				record.Requirements.AnalyzedArtifacts[0] = GUILayout.TextField(record.Requirements.AnalyzedArtifacts[0], GUILayout.Width(189));
				if (record.Requirements.AnalyzedArtifacts.Count < 2)
					record.Requirements.AnalyzedArtifacts = new List<string>() { record.Requirements.AnalyzedArtifacts[0], "" };
				record.Requirements.AnalyzedArtifacts[1] = GUILayout.TextField(record.Requirements.AnalyzedArtifacts[1], GUILayout.Width(189));
				if (record.Requirements.AnalyzedArtifacts.Count < 3)
					record.Requirements.AnalyzedArtifacts = new List<string>() { record.Requirements.AnalyzedArtifacts[0], record.Requirements.AnalyzedArtifacts[1], "" };
				record.Requirements.AnalyzedArtifacts[2] = GUILayout.TextField(record.Requirements.AnalyzedArtifacts[2], GUILayout.Width(189));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("✎ Assigned day: ", GUILayout.Width(100));
				record.AssignedDay = int.Parse(GUILayout.TextField(record.AssignedDay.ToString(), GUILayout.Width(50)));

				GUILayout.Space(415);

				if (GUILayout.Button("✂", GUILayout.Width(50))) State.JournalRecords.Remove(record);
				if (GUILayout.Button("✚", GUILayout.Width(50))) State.JournalRecords.Insert(i, (new JournalRecord("Journal" + (State.JournalRecords.Count + 1).ToString(), new Requirements(day: -1))));
				GUILayout.EndHorizontal();
			}
		}

		if (selectedPage == StateEditorPage.Quests)
		{
			for (int i = 0; i < State.QuestRecords.Count; i++)
			{
				GUILayout.Space(20);

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
				GUILayout.Label(string.Format("✎ Current quest status is <b>{1}</b>. Change to: ", quest.ID, quest.Status), GUILayout.Width(300));
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
			}
		}

		if (selectedPage == StateEditorPage.Artifacts)
		{
			for (int i = 0; i < State.Artifacts.Count; i++)
			{
				var artifact = State.Artifacts[i];

				GUILayout.Space(20);

				GUILayout.BeginHorizontal();
				GUILayout.Label("Artifact (id, sector, W, A, C):", GUILayout.Width(300));
				artifact.ID = GUILayout.TextField(artifact.ID, GUILayout.Width(215));
				artifact.Sector = int.Parse(GUILayout.TextField(artifact.Sector.ToString(), GUILayout.Width(60)));
				artifact.Wiring = int.Parse(GUILayout.TextField(artifact.Wiring.ToString(), GUILayout.Width(60)));
				artifact.Alloy = int.Parse(GUILayout.TextField(artifact.Alloy.ToString(), GUILayout.Width(60)));
				artifact.Chips = int.Parse(GUILayout.TextField(artifact.Chips.ToString(), GUILayout.Width(60)));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label(string.Format("Identity is <b>{0}</b> Choose another:", artifact.Identity == null ? "NONE" : artifact.Identity.ToString()), GUILayout.Width(300));
				if (GUILayout.Button("NONE")) artifact.Identity = null;
				if (GUILayout.Button("BRK1")) artifact.Identity = BreakageType.BRK1;
				if (GUILayout.Button("BRK2")) artifact.Identity = BreakageType.BRK2;
				if (GUILayout.Button("BRK3")) artifact.Identity = BreakageType.BRK3;
				if (GUILayout.Button("BRK4")) artifact.Identity = BreakageType.BRK4;
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label(string.Format("✎ Current status is <b>{0}</b>. Set it to:",
					artifact.Status.ToString()), GUILayout.Width(300));
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
		if (GUILayout.Button("Total reset (including rules)", GUILayout.Height(30))) State.Reset(true);
		if (GUILayout.Button("Save and return to menu", GUILayout.Height(30))) SaveAndExit();

		GUILayout.EndArea();
	}
}