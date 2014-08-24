using UnityEngine;

public class MainMenuView : BaseView
{
	private MainMenuController controller;

	private bool debugEnabled;
	private bool useSimpleView;
	private static bool usingGoogleText;
	private static bool waitingForTextUpdate;
	private static bool updateFailed;

	static MainMenuView ()
	{
		usingGoogleText = Text.GetType() == typeof(GoogleText);
		waitingForTextUpdate = usingGoogleText;

		if (usingGoogleText)
		{
			Events.TextUpdated += (c, e) =>
			{
				if (Text.Get("Google") == "FAIL") updateFailed = true;
				else waitingForTextUpdate = false;
			};
		}
	}

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

		GUI.Box(new Rect(Screen.width / 2 - 160, Screen.height / 2 - 135, 320, 270), "");
		GUILayout.BeginArea(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 125, 300, 250));
		if (waitingForTextUpdate)
		{
			GUILayout.Box("------------------------------ Updating text. Please wait... ------------------------------");
			if (updateFailed)
			{
				GUILayout.Label("Update failed. Check internet connection and try again.");
				if (GUILayout.Button("Try again", GUILayout.Height(30)))
				{
					updateFailed = false;
					ServiceLocator.Text = new GoogleText();
				}
			}
		}
		else
		{
			GUILayout.Box("------------------------------ Main menu ------------------------------");
			if (debugEnabled) useSimpleView = GUILayout.Toggle(useSimpleView, "Use simple view (debug / design mode)");
			GUILayout.Space(20);
			if (debugEnabled && GUILayout.Button("State editor", GUILayout.Height(30))) SwitchView(ViewType.StateEditor);
			if (usingGoogleText && GUILayout.Button("Update text cache", GUILayout.Height(30)))
			{
				waitingForTextUpdate = true;
				ServiceLocator.Text = new GoogleText();
			}
			if (GUILayout.Button("New game", GUILayout.Height(30)))
			{
				controller.StartNewGame();
				SwitchView(useSimpleView ? ViewType.SimpleView : ViewType.Intro);
			}
			if (State.GameStatus == GameStatus.InProgress && GUILayout.Button("Continue", GUILayout.Height(30)))
				SwitchView(useSimpleView ? ViewType.SimpleView : ViewType.Bridge);
			if (GUILayout.Button("Exit", GUILayout.Height(30))) Application.Quit();
		}
		GUILayout.EndArea();
	}
}