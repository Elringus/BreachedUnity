using UnityEngine;
using System.Collections;

public class MainMenuView : BaseView
{
	private MainMenuController controller;

	protected override void Awake ()
	{
		base.Awake();
		controller = new MainMenuController();
	}

	protected override void OnGUI ()
	{
		base.OnGUI();

		GUI.BeginGroup(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 200));
		GUI.Box(new Rect(0, 0, 300, 200), "Main Menu");
		if (GUI.Button(new Rect(10, 30, 280, 40), "State editor")) controller.Navigate(ScreenType.StateEditor);
		if (GUI.Button(new Rect(10, 70, 280, 40), "New game")) controller.StartNewGame();
		if (State.StartedGame && GUI.Button(new Rect(10, 110, 280, 40), "Continue")) controller.ContinueGame();
		if (GUI.Button(new Rect(10, 150, 280, 40), "Exit")) controller.ExitGame();
		GUI.EndGroup();
	}
}