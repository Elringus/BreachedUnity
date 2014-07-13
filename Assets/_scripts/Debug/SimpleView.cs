using UnityEngine;

public class SimpleView : BaseView
{
	private readonly float WIDTH = 600;
	private Vector2 scrollPosition;

	private SimpleController simpleController;
	private BridgeController bridgeController;
	private MapController mapController;

	private bool inFlightMode;

	protected override void Awake ()
	{
		base.Awake();

		//simpleController = new SimpleController();
		bridgeController = new BridgeController();
		mapController = new MapController();
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
		GUILayout.Label(string.Format("Minerals: A{0} B{1} C{2}", 0, 0, 0));
		GUILayout.Label(string.Format("Resources: W{0} A{1} C{2}", 0, 0, 0));
		GUILayout.EndHorizontal();

		if (State.GameProgress == GameProgressType.InProgress)
		{
			if (inFlightMode)
			{
				GUILayout.Space(10);
				GUILayout.Box("Flight mode");

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
				if (GUILayout.Button(string.Format("Enter flight mode [-{0}AP]", State.EnterSectorAPCost)))
					inFlightMode = mapController.EnterSector();

				GUILayout.Space(10);
				GUILayout.Box("Horizon");
			}
		}
		else GUILayout.Box(State.GameProgress.ToString());

		GUILayout.EndScrollView();
		if (GUILayout.Button("Return to menu")) SwitchView(ViewType.MainMenu);
		GUILayout.EndArea();
	}
}