using UnityEngine;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

public class SimpleView : BaseView
{
	private float width = 800;
	private Vector2 scrollPosition;

	private SimpleViewPage selectedPage;
	private bool showSynthFormula;
	private bool showProbes;
	private bool showArtifacts;
	private bool inFlightMode;
	private bool loadSectors;
	private int lootCharges;
	private List<Loot> sectorLoot = new List<Loot>();

	private int[] synthProbe = new int[3];

	static SimpleView ()
	{
		// disable quests invoking if we don't have the text provider
		if (Text.GetType() != typeof(NullText))
		{
			Events.StateUpdated += (c, e) =>
			{
				foreach (var quest in State.QuestRecords.Where(q => q.Status == QuestStatus.NotStarted))
					QuestController.StartQuest(quest);
				foreach (var record in State.JournalRecords.Where(r => r.Check()))
					record.AssignedDay = State.CurrentDay;
			};
		}
	}

	protected override void Update ()
	{
		base.Update();

		if (Screen.width < 800) width = Screen.width;
		else width = 800;
	}

	private void OnGUI ()
	{
		GUILayout.BeginArea(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - Screen.height / 2, width, Screen.height));
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width), GUILayout.Height(Screen.height - 35));

		GUILayout.BeginHorizontal();
		if (!inFlightMode && QuestController.GetCurrentQuest() == null)
		{
			if (GUILayout.Button(selectedPage == SimpleViewPage.Bridge ? "<b>❖ Bridge</b>" : "Bridge")) selectedPage = SimpleViewPage.Bridge;
			if (GUILayout.Button(selectedPage == SimpleViewPage.Workshop ? "<b>❖ Workshop</b>" : "Workshop")) selectedPage = SimpleViewPage.Workshop;
			if (GUILayout.Button(selectedPage == SimpleViewPage.Map ? "<b>❖ Map</b>" : "Map")) selectedPage = SimpleViewPage.Map;
			if (GUILayout.Button(selectedPage == SimpleViewPage.Horizon ? "<b>❖ Horizon</b>" : "Horizon")) selectedPage = SimpleViewPage.Horizon;
		}
		GUILayout.EndHorizontal();

		GUILayout.Box(string.Format("Day: {0}/{1} {2} AP: {3}/{4}",
			State.CurrentDay, State.TotalDays, QuestController.GetCurrentQuest() != null ? string.Format("● In quest mode. Quest name: {0}. Quest progress: {1} ●",
					QuestController.GetCurrentQuest().ID, QuestController.GetCurrentQuest().CurrentBlock) :
			inFlightMode ? string.Format("● In flight mode. Minerals: A{0} B{1} C{2}. Loot charges: {3}/{4} ●", State.MineralA, State.MineralB, State.MineralC, lootCharges, State.LootCharges) : 
			"●", State.CurrentAP, State.MaxAP));

		if (State.GameStatus == GameStatus.InProgress)
		{
			if (inFlightMode)
			{
				for (int i = 0; i < sectorLoot.Count; i++ )
					if (GUILayout.Button(string.Format("Loot spot of {0}", sectorLoot[i].LootType)))
					{
						FlightController.RecieveLoot(sectorLoot[i]);
						sectorLoot.Remove(sectorLoot[i]);
						lootCharges--;
						if (lootCharges <= 0) inFlightMode = false;
					}

				if (GUILayout.Button("Recall dron")) inFlightMode = false;
			}
			else if (QuestController.GetCurrentQuest() != null)
			{
				GUILayout.Label(XDocument.Parse(Text.Get(QuestController.GetCurrentQuest().CurrentBlock)).Root.Value, GUILayout.Width(width - 10));
				var choises = XDocument.Parse(Text.Get(QuestController.GetCurrentQuest().CurrentBlock)).Root.Elements("choise");
				if (choises.Count() == 0) { if (GUILayout.Button("End quest")) QuestController.EndQuest(); }
				else foreach (var choise in choises) if (GUILayout.Button(choise.Value, GUILayout.Width(width - 10))) QuestController.MakeChoise(choise.Value);
			}
			else
			{
				if (selectedPage == SimpleViewPage.Bridge)
				{
					for (int i = 1; i <= State.CurrentDay; i++)
						foreach (var record in State.JournalRecords.Where(r => r.AssignedDay == i))
							GUILayout.Label(Text.Get(record.ID));

					if (GUILayout.Button(string.Format("End day [AP = {0}]", State.MaxAP))) BridgeController.EndDay();
				}

				if (selectedPage == SimpleViewPage.Workshop)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label(string.Format("Fuel tank: <b>{0}</b>", State.FuelSynthed ? "FULL" : "EMPTY"));
					GUILayout.Label(string.Format("Breakage: <b>{0}</b>", State.EngineFixed ? "FIXED" : State.BreakageType.ToString()));
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					GUILayout.Label(string.Format("Resources: W{0} A{1} C{2}", State.Wiring, State.Alloy, State.Chips), GUILayout.Width(width / 2));
					GUILayout.Label(string.Format("Fix engine requirments: W{0} A{1} C{2}",
						State.FixEngineRequirements[State.BreakageType][0],
						State.FixEngineRequirements[State.BreakageType][1],
						State.FixEngineRequirements[State.BreakageType][2]));
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					GUILayout.Label(string.Format("Minerals: A{0} B{1} C{2}", State.MineralA, State.MineralB, State.MineralC), GUILayout.Width(width / 2));
					showSynthFormula = GUILayout.Toggle(showSynthFormula, "show sync formula");
					if (showSynthFormula)
					{
						GUILayout.Label(string.Format("A{0} B{1} C{2}",
							State.FuelSynthFormula[0],
							State.FuelSynthFormula[1],
							State.FuelSynthFormula[2]));
					}
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					GUILayout.BeginVertical(GUILayout.Width(width / 2));
					GUILayout.Label(string.Format("Analyzed artifacts ({0}):", State.Artifacts.FindAll(x => x.Status == ArtifactStatus.Analyzed).Count));
					showArtifacts = GUILayout.Toggle(showArtifacts, "show all analyzed artifacts");
					if (showArtifacts) foreach (var artifact in State.Artifacts.FindAll(x => x.Status == ArtifactStatus.Analyzed))
							GUILayout.Label(string.Format("[{0}] {1} ScanInfo: {2}", artifact.ID, artifact.Name, artifact.ScanInfo));
					GUILayout.EndVertical();

					GUILayout.BeginVertical(GUILayout.Width(width / 2 - 30));
					GUILayout.Label(string.Format("Fuel synth probes ({0}):", State.FuelSynthProbes.Count));
					showProbes = GUILayout.Toggle(showProbes, "show all the probes");
					if (showProbes) foreach (var probe in State.FuelSynthProbes)
						{
							GUILayout.BeginHorizontal();
							GUILayout.Label(string.Format("Probe №{0} [A: {1}, B: {2}, C: {3}] is {4}.",
								State.FuelSynthProbes.FindIndex(x => x == probe), probe[0], probe[1], probe[2],
								WorkshopController.MeasureProbe(probe)));
							GUILayout.EndHorizontal();
						}
					GUILayout.EndVertical();
					GUILayout.EndHorizontal();

					if (WorkshopController.CanFixEngine() &&
						GUILayout.Button(string.Format("Fix engine [-{0}AP]", State.FixEngineAPCost))) WorkshopController.FixEngine();
					if (!State.FuelSynthed)
					{
						GUILayout.BeginHorizontal();
						if (GUILayout.Button(string.Format("Synth fuel [-{0}AP] (A + B + C must be {1})", State.FuelSynthAPCost, State.FuelSynthSumm)))
						{
							WorkshopController.SynthFuel(synthProbe);
							synthProbe = new int[3];
						}
						synthProbe[0] = int.Parse(GUILayout.TextField(synthProbe[0].ToString()));
						synthProbe[1] = int.Parse(GUILayout.TextField(synthProbe[1].ToString()));
						synthProbe[2] = int.Parse(GUILayout.TextField(synthProbe[2].ToString()));
						GUILayout.EndHorizontal();
					}
					foreach (var artifact in State.Artifacts.FindAll(x => x.Status == ArtifactStatus.Found))
						if (GUILayout.Button(string.Format("Start analyzing {0} [-{1}AP]", artifact.ID, State.AnalyzeArtifactAPCost)))
							WorkshopController.AnalyzeArtifact(artifact);
				}

				if (selectedPage == SimpleViewPage.Map)
				{
					loadSectors = GUILayout.Toggle(loadSectors, " Load sector scenes");
					if (GUILayout.Button(string.Format("Enter 1st sector [-{0}AP]", State.EnterSectorAPCost))) InitFlightMode(1);
					if (GUILayout.Button(string.Format("Enter 2nt sector [-{0}AP]", State.EnterSectorAPCost))) InitFlightMode(2);
					if (GUILayout.Button(string.Format("Enter 3rd sector [-{0}AP]", State.EnterSectorAPCost))) InitFlightMode(3);
					if (GUILayout.Button(string.Format("Enter 4th sector [-{0}AP]", State.EnterSectorAPCost))) InitFlightMode(4);
				}

				if (selectedPage == SimpleViewPage.Horizon)
				{
					foreach (var phrase in HorizonController.GetPhrases())
					{
						string phraseText = string.Format("[{0}] {1}", phrase.ID, Text.Get(phrase.ID));
						if (phrase.AssociatedQuest != string.Empty)
						{
							if (GUILayout.Button(phraseText)) phrase.StartAssociatedQuest();
						}
						else GUILayout.Label(string.Format("[{0}] {1}", phrase.ID, Text.Get(phrase.ID)));
					}
				}
			}
		}
		else GUILayout.Box(State.GameStatus.ToString());

		GUILayout.EndScrollView();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Return to menu", GUILayout.Height(30))) SwitchView(ViewType.MainMenu);
		if (GUILayout.Button("Reset and start new game", GUILayout.Height(30)))
		{
			State.Reset();
			State.GameStatus = GameStatus.InProgress;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	private void InitFlightMode (int sectorID)
	{
		if (!MapController.EnterSector()) return;

		if (loadSectors) SwitchView(sectorID == 1 ? ViewType.Sector1 : sectorID == 2 ? ViewType.Sector2 : sectorID == 3 ? ViewType.Sector3 : ViewType.Sector4);
		else
		{
			inFlightMode = true;
			lootCharges = State.LootCharges;
			FlightController.GenerateLoot(sectorID, out sectorLoot);
		}
	}
}