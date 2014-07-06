using UnityEngine;
using System.Collections;

public class SimpleView : BaseController
{
	private readonly float WIDTH = 600;

	private Vector2 scrollPosition;

	private void OnGUI ()
	{
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

		GUILayout.Space(10);
		GUILayout.Box("Bridge");
		if (GUILayout.Button("End day")) EndDay();

		GUILayout.Space(10);
		GUILayout.Box("Workshop");

		GUILayout.Space(10);
		GUILayout.Box("Map");

		GUILayout.Space(10);
		GUILayout.Box("Horizon");

		GUILayout.Space(10);
		GUILayout.Box("Flight");

		GUILayout.EndScrollView();
		if (GUILayout.Button("Return to menu")) ReturnToMenu();
		GUILayout.EndArea();
	}

	private void ReturnToMenu ()
	{
		Application.LoadLevel("scn_MainMenu");
	}

	private void EndDay ()
	{
		State.CurrentDay++;
		State.CurrentAP = State.MaxAP;
	}
}