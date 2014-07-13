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

		GUILayout.Box("Configuration info (not editable)");

		GUILayout.BeginHorizontal();
		GUILayout.Label("Save data verion: ", GUILayout.Width(300));
		GUILayout.Label(string.Format("{0}.{1}.{2}", State.VersionMajor, State.VersionMiddle, State.VersionMinor));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Game progress: ", GUILayout.Width(300));
		GUILayout.Label(State.GameProgress.ToString());
		GUILayout.EndHorizontal();

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

		GUILayout.Box("State variables (specific for the current game session)");

		GUILayout.BeginHorizontal();
		GUILayout.Label("Current day: ", GUILayout.Width(300));
		State.CurrentDay = int.Parse(GUILayout.TextField(State.CurrentDay.ToString()));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Current AP: ", GUILayout.Width(300));
		State.CurrentAP = int.Parse(GUILayout.TextField(State.CurrentAP.ToString()));
		GUILayout.EndHorizontal();

		GUILayout.EndScrollView();
		if (GUILayout.Button("Total reset (including rules)", GUILayout.Height(30))) controller.TotalReset();
		if (GUILayout.Button("Return to menu", GUILayout.Height(30))) SwitchView(ViewType.MainMenu);
		GUILayout.EndArea();
	}
}