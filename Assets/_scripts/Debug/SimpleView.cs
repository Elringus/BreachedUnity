using UnityEngine;
using System.Collections.Generic;

public class SimpleView : BaseView
{
	private readonly float WIDTH = 600;
	private Vector2 scrollPosition;

	private SimpleController simpleController;
	private BridgeController bridgeController;
	private MapController mapController;
	private FlightController flightController;

	private bool inFlightMode;
	private int lootCharges;
	private List<Loot> sectorLoot = new List<Loot>();

	protected override void Awake ()
	{
		base.Awake();

		//simpleController = new SimpleController();
		bridgeController = new BridgeController();
		mapController = new MapController();
		flightController = new FlightController();
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
		GUILayout.Label(string.Format("Fuel tank: empty"));
		GUILayout.Label(string.Format("Breakage: BRK1"));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label(string.Format("AP: {0}/{1}", State.CurrentAP, State.MaxAP));
		GUILayout.Label(string.Format("Minerals: A{0} B{1} C{2}", State.MineralA, State.MineralB, State.MineralC));
		GUILayout.Label(string.Format("Resources: W{0} A{1} C{2}", 0, 0, 0));
		GUILayout.EndHorizontal();

		if (State.GameProgress == GameProgressType.InProgress)
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
		else GUILayout.Box(State.GameProgress.ToString());

		GUILayout.EndScrollView();
		if (GUILayout.Button("Return to menu")) SwitchView(ViewType.MainMenu);
		GUILayout.EndArea();
	}

	private void InitFlightMode (int sector)
	{
		if (!mapController.EnterSector()) return;

		inFlightMode = true;

		lootCharges = State.LootCharges;

		var sectorParams = State.SectorsParameters.Find(x => x.SectorID == sector);
		sectorLoot.Clear();
		for (int i = 0; i < sectorParams.LootSpotCount; i++)
			sectorLoot.Add(flightController.GenerateLoot(sectorParams));
	}
}