using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : BaseView
{
	private MainMenuController MainMenuController;
	private static bool connecting;
	private GameObject googlePanel;

	static MainMenuView ()
	{
		if (Text.GetType() == typeof(GoogleText))
		{
			Events.TextUpdated += (c, e) => connecting = false;
			if (Text.Get("STATE") == "NONE") connecting = true;
		}
	}

	protected override void Awake ()
	{
		base.Awake();

		MainMenuController = new MainMenuController();
	}

	protected override void Start ()
	{
		base.Start();

		var versionText = AddUIElement("panel_version").GetComponentInChildren<Text>();
		versionText.text = string.Format("Breached {0}\nver. {1}.{2}.{3}",
			GlobalConfig.RELEASE_TYPE, GlobalConfig.VERSION_MAJOR, GlobalConfig.VERSION_MIDDLE, GlobalConfig.VERSION_MINOR);

		Transform menu;

		if (GlobalConfig.RELEASE_TYPE == ReleaseType.alpha)
		{
			menu = AddUIElement("panel_main-menu-dev").transform;
			menu.FindChild("button_state-editor").GetComponent<Button>()
				.OnClick(() => SwitchView(ViewType.StateEditor));
			menu.FindChild("button_simple-view").GetComponent<Button>()
				.OnClick(() => SwitchView(ViewType.SimpleView));
			menu.FindChild("button_update-text").GetComponent<Button>()
				.OnClick(() =>
				{
					connecting = true;
					ServiceLocator.Text = new GoogleText();
					googlePanel = AddUIElement("panel_google-text");
				});

			if (Text.GetType() == typeof(GoogleText))
				googlePanel = AddUIElement("panel_google-text");
		}
		else menu = AddUIElement("panel_main-menu").transform;

		menu.FindChild("button_new-game").GetComponent<Button>()
			.OnClick(() => { MainMenuController.StartNewGame(); SwitchView(ViewType.Intro); });
		menu.FindChild("button_continue").GetComponent<Button>()
			.OnClick(() => { SwitchView(ViewType.Bridge); });
		menu.FindChild("button_continue").GetComponent<Button>().interactable = State.GameStatus == GameStatus.InProgress;
		menu.FindChild("button_settings").GetComponent<Button>()
			.OnClick(() => { });
		menu.FindChild("button_exit").GetComponent<Button>()
			.OnClick(() => Application.Quit());
	}

	protected override void Update ()
	{
		base.Update();

		if (!connecting && googlePanel) Destroy(googlePanel);
		else if (connecting)
		{
			var slider = GameObject.Find("slider_progress").GetComponent<Slider>();
			var progress = float.Parse(Text.Get("PROGRESS"));

			slider.value = progress >= .8f ? 1 :
				Mathf.Lerp(slider.value, progress, Time.deltaTime * 5);
		}
	}
}