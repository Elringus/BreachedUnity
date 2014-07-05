using UnityEngine;
using System.Collections;

public class MainMenu : BaseController
{
	private void OnGUI ()
	{
		GUI.BeginGroup(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 200));
		GUI.Box(new Rect(0, 0, 300, 200), 
			string.Format("Breached {0} ver. {1}.{2}.{3}", 
			GlobalConfig.RELEASE_TYPE, GlobalConfig.VERSION_MAJOR, GlobalConfig.VERSION_MIDDLE, GlobalConfig.VERSION_MINOR));
		if (GUI.Button(new Rect(10, 40, 280, 30), "State editor")) StateEditor();
		if (GUI.Button(new Rect(10, 80, 280, 30), "New game")) StartNewGame();
		if (GUI.Button(new Rect(10, 120, 280, 30), "Continue")) ContinueGame();
		if (GUI.Button(new Rect(10, 160, 280, 30), "Exit")) ExitGame();
		GUI.EndGroup();
	}

	private void StateEditor ()
	{
		Application.LoadLevel("scn_StateEditor");
	}

	private void StartNewGame ()
	{

	}

	private void ContinueGame ()
	{

	}

	private void ExitGame ()
	{
		Application.Quit();
	}
}