using UnityEngine;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

public class SimpleView : BaseView
{
	private readonly float WIDTH = 800;
	private Vector2 scrollPosition;

	private BridgeController bridgeController;
	private MapController mapController;
	private FlightController flightController;
	private WorkshopController workshopController;
	private HorizonController horizonController;

	private static QuestController questController;

	private bool showSynthFormula;
	private bool inFlightMode;
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
					questController.StartQuest(quest);
			};
		}
	}

	protected override void Awake ()
	{
		base.Awake();

		bridgeController = new BridgeController();
		mapController = new MapController();
		flightController = new FlightController();
		workshopController = new WorkshopController();
		horizonController = new HorizonController();

		questController = new QuestController();
	}

	private void OnGUI ()
	{
		GUILayout.BeginArea(new Rect(Screen.width / 2 - WIDTH / 2, Screen.height / 2 - Screen.height / 2, WIDTH, Screen.height));
		GUILayout.Box("Breached simple view\n--------------------------------------------------------------------------------------------------------------------------------------");
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(WIDTH), GUILayout.Height(Screen.height - 120));

		GUILayout.Box("Info");

		GUILayout.BeginHorizontal();
		GUILayout.Label(string.Format("Day: {0}/{1}", State.CurrentDay, State.TotalDays));
		GUILayout.Label(string.Format("AP: {0}/{1}", State.CurrentAP, State.MaxAP));
		GUILayout.Label(string.Format("Fuel tank: {0}", State.FuelSynthed ? "FULL" : "EMPTY"));
		GUILayout.Label(string.Format("Breakage: {0}", State.EngineFixed ? "FIXED" : State.BreakageType.ToString()));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label(string.Format("Minerals: A{0} B{1} C{2}", State.MineralA, State.MineralB, State.MineralC), GUILayout.Width(250));
		showSynthFormula = GUILayout.Toggle(showSynthFormula, "show sync formula");
		if (showSynthFormula)
		{
			GUILayout.Label(string.Format("Fuel synth formula: A{0} B{1} C{2}",
				State.FuelSynthFormula[0],
				State.FuelSynthFormula[1],
				State.FuelSynthFormula[2]));
		}
		GUILayout.EndHorizontal();


		GUILayout.BeginHorizontal();
		GUILayout.Label(string.Format("Resources: W{0} A{1} C{2}", State.Wiring, State.Alloy, State.Chips), GUILayout.Width(250));
		GUILayout.Label(string.Format("Fix engine requirments: W{0} A{1} C{2}", 
			State.FixEngineRequirements[State.BreakageType][0], 
			State.FixEngineRequirements[State.BreakageType][1], 
			State.FixEngineRequirements[State.BreakageType][2]));
		GUILayout.EndHorizontal();

		GUILayout.Label("Fuel synth probes:");
		foreach (var probe in State.FuelSynthProbes)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(string.Format("Probe №{0} [A: {1}, B: {2}, C: {3}] is {4}.",
				State.FuelSynthProbes.FindIndex(x => x == probe), probe[0], probe[1], probe[2], 
				workshopController.MeasureProbe(probe)), GUILayout.Width(300));
			GUILayout.EndHorizontal();
		}

		GUILayout.Label("Analyzed artifacts:");
		foreach (var artifact in State.Artifacts.FindAll(x => x.Status == ArtifactStatus.Analyzed)) GUILayout.Label(artifact.Name);

		if (State.GameStatus == GameStatus.InProgress)
		{
			if (inFlightMode)
			{
				GUILayout.Space(10);
				GUILayout.Box(string.Format("In flight mode. Loot charges: {0}/{1}", lootCharges, State.LootCharges));

				for (int i = 0; i < sectorLoot.Count; i++ )
					if (GUILayout.Button(string.Format("Loot spot of {0}", sectorLoot[i].LootType)))
					{
						flightController.RecieveLoot(sectorLoot[i]);
						sectorLoot.Remove(sectorLoot[i]);
						lootCharges--;
						if (lootCharges <= 0) inFlightMode = false;
					}

				if (GUILayout.Button("Recall dron")) inFlightMode = false;
			}
			else if (questController.GetCurrentQuest() != null)
			{
				GUILayout.Space(10);
				GUILayout.Box(string.Format("In quest mode. Quest name: {0}. Quest progress: {1}", 
					questController.GetCurrentQuest().Name, questController.GetCurrentQuest().CurrentBlock), GUILayout.Width(590));
				GUILayout.Label(XDocument.Parse(Text.Get(questController.GetCurrentQuest().CurrentBlock)).Root.Value, GUILayout.Width(590));
				var choises = XDocument.Parse(Text.Get(questController.GetCurrentQuest().CurrentBlock)).Root.Elements("choise");
				if (choises.Count() == 0) { if (GUILayout.Button("End quest")) questController.EndQuest(); }
				else foreach (var choise in choises) if (GUILayout.Button(choise.Value, GUILayout.Width(590))) questController.MakeChoise(choise.Value);
			}
			else
			{
				GUILayout.Space(10);
				GUILayout.Box("Bridge");
				if (GUILayout.Button(string.Format("End day [AP = {0}]", State.MaxAP))) bridgeController.EndDay();

				GUILayout.Space(10);
				GUILayout.Box("Workshop");
				if (workshopController.CanFixEngine() &&
					GUILayout.Button(string.Format("Fix engine [-{0}AP]", State.FixEngineAPCost))) workshopController.FixEngine();
				if (!State.FuelSynthed)
				{
					GUILayout.BeginHorizontal();
					if (GUILayout.Button(string.Format("Synth fuel [-{0}AP] (A + B + C must be {1})", State.FuelSynthAPCost, State.FuelSynthSumm)))
					{
						workshopController.SynthFuel(synthProbe);
						synthProbe = new int[3];
					}
					synthProbe[0] = int.Parse(GUILayout.TextField(synthProbe[0].ToString()));
					synthProbe[1] = int.Parse(GUILayout.TextField(synthProbe[1].ToString()));
					synthProbe[2] = int.Parse(GUILayout.TextField(synthProbe[2].ToString()));
					GUILayout.EndHorizontal();
				}
				foreach (var artifact in State.Artifacts.FindAll(x => x.Status == ArtifactStatus.Found))
					if (GUILayout.Button(string.Format("Start analyzing {0} [-{1}AP]", artifact.Name, State.AnalyzeArtifactAPCost)))
						workshopController.AnalyzeArtifact(artifact);

				GUILayout.Space(10);
				GUILayout.Box("Map");
				if (GUILayout.Button(string.Format("Enter 1st sector [-{0}AP]", State.EnterSectorAPCost))) InitFlightMode(1);
				if (GUILayout.Button(string.Format("Enter 2nt sector [-{0}AP]", State.EnterSectorAPCost))) InitFlightMode(2);
				if (GUILayout.Button(string.Format("Enter 3rd sector [-{0}AP]", State.EnterSectorAPCost))) InitFlightMode(3);
				if (GUILayout.Button(string.Format("Enter 4th sector [-{0}AP]", State.EnterSectorAPCost))) InitFlightMode(4);

				GUILayout.Space(10);
				GUILayout.Box("Horizon");
				foreach (var phrase in horizonController.GetPhrases())
					GUILayout.Label(string.Format("[{0}] {1}", phrase.Name, Text.Get(phrase.Name)));
			}
		}
		else GUILayout.Box(State.GameStatus.ToString());

		GUILayout.EndScrollView();
		if (GUILayout.Button("Reset and start new game", GUILayout.Height(30)))
		{
					State.Reset();
		State.GameStatus = GameStatus.InProgress;
		}
		if (GUILayout.Button("Return to menu", GUILayout.Height(30))) SwitchView(ViewType.MainMenu);
		GUILayout.EndArea();
	}

	private void InitFlightMode (int sectorID)
	{
		if (!mapController.EnterSector()) return;

		inFlightMode = true;
		lootCharges = State.LootCharges;
		flightController.GenerateLoot(sectorID, out sectorLoot);
	}
}