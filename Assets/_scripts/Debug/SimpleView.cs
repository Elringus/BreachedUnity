using UnityEngine;
using System.Collections.Generic;

public class SimpleView : BaseView
{
	private readonly float WIDTH = 600;
	private Vector2 scrollPosition;

	private BridgeController bridgeController;
	private MapController mapController;
	private FlightController flightController;
	private WorkshopController workshopController;

	private bool inFlightMode;
	private int lootCharges;
	private List<Loot> sectorLoot = new List<Loot>();

	private int[] synthProbe = new int[3];

	protected override void Awake ()
	{
		base.Awake();

		bridgeController = new BridgeController();
		mapController = new MapController();
		flightController = new FlightController();
		workshopController = new WorkshopController();
	}

	protected override void OnGUI ()
	{
		base.OnGUI();

		GUILayout.BeginArea(new Rect(Screen.width / 2 - WIDTH / 2, Screen.height / 2 - Screen.height / 2, WIDTH, Screen.height));
		GUILayout.Box("Breached simple view\n--------------------------------------------------------------------------------------------------------------------------------------");
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(WIDTH), GUILayout.Height(Screen.height - 80));

		GUILayout.Box("Info");

		GUILayout.BeginHorizontal();
		GUILayout.Label(string.Format("Day: {0}/{1}", State.CurrentDay, State.TotalDays));
		GUILayout.Label(string.Format("AP: {0}/{1}", State.CurrentAP, State.MaxAP));
		GUILayout.Label(string.Format("Fuel tank: {0}", State.FuelSynthed ? "FULL" : "EMPTY"));
		GUILayout.Label(string.Format("Breakage: {0}", State.EngineFixed ? "FIXED" : State.BreakageType.ToString()));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label(string.Format("Minerals: A{0} B{1} C{2}", State.MineralA, State.MineralB, State.MineralC));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label(string.Format("Resources: W{0} A{1} C{2}", State.Wiring, State.Alloy, State.Chips));
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
			else
			{
				GUILayout.Space(10);
				GUILayout.Box("Bridge");
				if (GUILayout.Button(string.Format("End day [AP = {0}]", State.MaxAP))) bridgeController.EndDay();

				GUILayout.Space(10);
				GUILayout.Box("Workshop");
				if (workshopController.CanFixEngine() &&
					GUILayout.Button(string.Format("Fix engine [AP = {0}]", State.FixEngineAPCost))) workshopController.FixEngine();
				if (!State.FuelSynthed)
				{
					GUILayout.BeginHorizontal();
					if (GUILayout.Button(string.Format("Synth fuel [AP = {0}] (A + B + C must be 9)", State.FuelSynthAPCost)))
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
			}
		}
		else GUILayout.Box(State.GameStatus.ToString());

		GUILayout.EndScrollView();
		if (GUILayout.Button("Return to menu")) SwitchView(ViewType.MainMenu);
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