using UnityEngine;

public class StateEditorView : BaseView
{
	private readonly float WIDTH = 600;
	private Vector2 scrollPosition;

	private StateEditorController controller;

	protected override void Awake ()
	{
		base.Awake();

		controller = new StateEditorController();
	}

	protected override void OnGUI ()
	{
		base.OnGUI();

		GUILayout.BeginArea(new Rect(Screen.width / 2 - WIDTH / 2, Screen.height / 2 - Screen.height / 2, WIDTH, Screen.height));
		GUILayout.Box("Breached state editor | ʕノ•ᴥ•ʔノ ︵ ┻━┻\n--------------------------------------------------------------------------------------------------------------------------------------");
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(WIDTH), GUILayout.Height(Screen.height - 120));

		#region CONFIG
		GUILayout.Box("Configuration info (not editable)");

		GUILayout.BeginHorizontal();
		GUILayout.Label("Save data verion: ", GUILayout.Width(300));
		GUILayout.Label(string.Format("{0}.{1}.{2}", State.VersionMajor, State.VersionMiddle, State.VersionMinor));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Game progress: ", GUILayout.Width(300));
		GUILayout.Label(State.GameProgress.ToString());
		GUILayout.EndHorizontal();
		#endregion

		#region RULES
		GUILayout.Box("Rules constants (cross-session invariant)");

		GUILayout.BeginHorizontal();
		GUILayout.Label("Total days: ", GUILayout.Width(300));
		State.TotalDays = int.Parse(GUILayout.TextField(State.TotalDays.ToString()));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Max AP: ", GUILayout.Width(300));
		State.MaxAP = int.Parse(GUILayout.TextField(State.MaxAP.ToString()));
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
		GUILayout.Label("Sector 1 parameters (loot spots, A,B,C): ", GUILayout.Width(300));
		State.SectorsParameters.Find(x => x.SectorID == 1).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).LootSpotCount.ToString()));
		State.SectorsParameters.Find(x => x.SectorID == 1).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).MineralA.ToString()));
		State.SectorsParameters.Find(x => x.SectorID == 1).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).MineralB.ToString()));
		State.SectorsParameters.Find(x => x.SectorID == 1).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 1).MineralC.ToString()));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Sector 2 parameters (loot spots, A,B,C): ", GUILayout.Width(300));
		State.SectorsParameters.Find(x => x.SectorID == 2).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).LootSpotCount.ToString()));
		State.SectorsParameters.Find(x => x.SectorID == 2).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).MineralA.ToString()));
		State.SectorsParameters.Find(x => x.SectorID == 2).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).MineralB.ToString()));
		State.SectorsParameters.Find(x => x.SectorID == 2).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 2).MineralC.ToString()));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Sector 3 parameters (loot spots, A,B,C): ", GUILayout.Width(300));
		State.SectorsParameters.Find(x => x.SectorID == 3).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).LootSpotCount.ToString()));
		State.SectorsParameters.Find(x => x.SectorID == 3).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).MineralA.ToString()));
		State.SectorsParameters.Find(x => x.SectorID == 3).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).MineralB.ToString()));
		State.SectorsParameters.Find(x => x.SectorID == 3).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 3).MineralC.ToString()));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Sector 4 parameters (loot spots, A,B,C): ", GUILayout.Width(300));
		State.SectorsParameters.Find(x => x.SectorID == 4).LootSpotCount = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).LootSpotCount.ToString()));
		State.SectorsParameters.Find(x => x.SectorID == 4).MineralA = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).MineralA.ToString()));
		State.SectorsParameters.Find(x => x.SectorID == 4).MineralB = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).MineralB.ToString()));
		State.SectorsParameters.Find(x => x.SectorID == 4).MineralC = int.Parse(GUILayout.TextField(State.SectorsParameters.Find(x => x.SectorID == 4).MineralC.ToString()));
		GUILayout.EndHorizontal();
		#endregion

		#region STATE
		GUILayout.Box("State variables (specific for the current game session)");

		GUILayout.BeginHorizontal();
		GUILayout.Label("Current day: ", GUILayout.Width(300));
		State.CurrentDay = int.Parse(GUILayout.TextField(State.CurrentDay.ToString()));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Current AP: ", GUILayout.Width(300));
		State.CurrentAP = int.Parse(GUILayout.TextField(State.CurrentAP.ToString()));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Minerals A, B, C: ", GUILayout.Width(300));
		State.MineralA = int.Parse(GUILayout.TextField(State.MineralA.ToString()));
		State.MineralB = int.Parse(GUILayout.TextField(State.MineralB.ToString()));
		State.MineralC = int.Parse(GUILayout.TextField(State.MineralC.ToString()));
		GUILayout.EndHorizontal();
		#endregion

		GUILayout.EndScrollView();
		if (GUILayout.Button("Total reset (including rules)", GUILayout.Height(30))) controller.TotalReset();
		if (GUILayout.Button("Return to menu", GUILayout.Height(30))) SwitchView(ViewType.MainMenu);
		GUILayout.EndArea();
	}
}