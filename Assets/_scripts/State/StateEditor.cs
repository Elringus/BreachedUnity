using UnityEngine;
using System.Collections;

public class StateEditor : BaseController
{
	private readonly float WIDTH = 600;

	private Vector2 scrollPosition;

	private void OnGUI ()
	{
		GUILayout.BeginArea(new Rect(Screen.width / 2 - WIDTH / 2, Screen.height / 2 - Screen.height / 2, WIDTH, Screen.height));
		GUILayout.Box("Breached state editor | ʕノ•ᴥ•ʔノ ︵ ┻━┻\n--------------------------------------------------------------------------------------------------------------------------------------");
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(WIDTH), GUILayout.Height(Screen.height - 80));

		GUILayout.Box("Configuration info (not editable)");

		GUILayout.BeginHorizontal();
		GUILayout.Label("Save data verion: ", GUILayout.Width(300));
		GUILayout.Label(string.Format("{0}.{1}.{2}", State.VersionMajor, State.VersionMiddle, State.VersionMinor));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Started game: ", GUILayout.Width(300));
		GUILayout.Label(State.StartedGame.ToString());
		GUILayout.EndHorizontal();

		GUILayout.Box("Main parameters");

		GUILayout.BeginHorizontal();
		GUILayout.Label("Total days: ", GUILayout.Width(300));
		State.TotalDays = int.Parse(GUILayout.TextField(State.TotalDays.ToString()));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Current day: ", GUILayout.Width(300));
		State.CurrentDay = int.Parse(GUILayout.TextField(State.CurrentDay.ToString()));
		GUILayout.EndHorizontal();

		GUILayout.EndScrollView();
		if (GUILayout.Button("Return to menu")) ReturnToMenu();
		GUILayout.EndArea();
	}

	private void ReturnToMenu ()
	{
		Application.LoadLevel("scn_MainMenu");
	}
}