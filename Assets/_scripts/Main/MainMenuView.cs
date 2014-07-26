using UnityEngine;

public class MainMenuView : BaseView
{
	private MainMenuController controller;

	private bool debugEnabled;
	private bool useSimpleView;

	protected override void Awake ()
	{
		base.Awake();

		controller = new MainMenuController();

		if (GlobalConfig.RELEASE_TYPE == ReleaseType.alpha) debugEnabled = true;
		useSimpleView = debugEnabled;
	}

	protected override void OnGUI ()
	{
		base.OnGUI();

		GUI.BeginGroup(new Rect(Screen.width / 2 - 150, Screen.height / 2 - (debugEnabled ? 120 : 80), 300, debugEnabled ? 240 : 160));
		GUI.Box(new Rect(0, 0, 300, debugEnabled ? 240 : 160), "Main Menu");
		if (debugEnabled) useSimpleView = GUI.Toggle(new Rect(22, 37, 280, 40), useSimpleView, "Use simple view (debug / design mode)");
		if (debugEnabled && GUI.Button(new Rect(10, 70, 280, 40), "State editor")) SwitchView(ViewType.StateEditor);
		if (GUI.Button(new Rect(10, debugEnabled ? 110 : 30, 280, 40), "New game"))
		{
			controller.StartNewGame();
			SwitchView(useSimpleView ? ViewType.SimpleView : ViewType.Intro);
		}
		if (State.GameStatus == GameStatus.InProgress && GUI.Button(new Rect(10, debugEnabled ? 150 : 70, 280, 40), "Continue")) 
			SwitchView(useSimpleView ? ViewType.SimpleView : ViewType.Bridge);
		if (GUI.Button(new Rect(10, debugEnabled ? 190 : 110, 280, 40), "Exit")) Application.Quit();
		GUI.EndGroup();
	}
}